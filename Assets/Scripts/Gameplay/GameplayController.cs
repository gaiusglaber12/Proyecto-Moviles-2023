using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using Newtonsoft.Json;

public class GameplayController : SceneController
{
    #region EXPOSED_FIELDS
    [SerializeField] private LevelConfigSO debugConfig = null;
    [SerializeField] private LevelConfigSO[] levels = null;
    [SerializeField] private SlingSkinConfigsSO[] skins = null;
    [SerializeField] private SlingController slingController = null;
    [SerializeField] private CameraController cameraController = null;
    [SerializeField] private Material dayMat = null;
    [SerializeField] private Material bundleMat = null;
    [SerializeField] private Material nightMat = null;
    [SerializeField] private TMPro.TMP_Text scoreTxt = null;
    [SerializeField] private TMPro.TMP_Text timerTxt = null;
    [SerializeField] private CountdownPanel countDownPanel = null;

    [SerializeField] private int multScore = 1;

    [Header("Chunk Settings")]
    [SerializeField] private ChunkView[] chunkViewPrefabs = null;
    [SerializeField] private float depthChunkOffset = 0;
    [SerializeField] private Vector3 initialChunkPosition = Vector3.zero;
    [SerializeField] private int maxPoolSize = 10;
    [SerializeField] private int initialChunks = 0;

    [Header("LoseView")]
    [SerializeField] private Animator loseAnimator = null;
    [SerializeField] private TMPro.TMP_Text loseScoreTxt = null;
    [SerializeField] private CurrencyView losecoinView = null;
    [SerializeField] private Button loseContinueBtn = null;

    [Header("WinView")]
    [SerializeField] private Animator winAnimator = null;
    [SerializeField] private TMPro.TMP_Text winScoreTxt = null;
    [SerializeField] private Image starImg = null;
    [SerializeField] private CurrencyView wincoinView = null;
    [SerializeField] private CurrencyView wingemView = null;
    [SerializeField] private Sprite oneStartImg = null;
    [SerializeField] private Sprite twoStartImg = null;
    [SerializeField] private Sprite threeStarImg = null;
    [SerializeField] private Button winContinueBtn = null;
    #endregion

    #region PRIVATE_FIELDS
    private float timer = 0.0f;
    private int currScore = 0;

    private bool win = false;
    private bool endGame = true;

    private LevelConfigSO currLevel = null;

    private List<VirtualPurchaseDefinition> virtualPurchases = null;
    private Queue<ChunkView> poolChunks = null;
    private Vector3 finalChunkPosition = Vector3.zero;
    private Vector3 currChunkPosition = Vector3.zero;
    private const string levelsSavedKey = "levelsPlayed";

    private int reachedStars = 0;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        poolChunks = new Queue<ChunkView>();
        StartCoroutine(ChangeSkybox());
        countDownPanel.Init(
            onGameStarted: () =>
                {
                    slingController.OnGameBegined = true;
                });

        if (debugConfig != null)
        {
            currLevel = debugConfig; //TEMP
        }
        else
        {
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i].Id == PersistentView.CurrLevel && levels[i].Dificulty == PersistentView.Instance.GetCurrentDificultyEnum())
                {
                    currLevel = levels[i];
                }
            }
        }

        cameraController.Init(currLevel.Speed);

        scoreTxt.text = 0 + "/" + currLevel.MinScore;
        timer = currLevel.Seconds;
        slingController.InitGameplay(currLevel.BallPrefab);


        string selectedSlingerId = PlayerPrefs.GetString("selectedSlinger", string.Empty);
        if (selectedSlingerId == string.Empty)
        {
            selectedSlingerId = "slinger_0";
        }
        selectedSlingerId = selectedSlingerId.ToLower();
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].RemoteId == selectedSlingerId)
            {
                slingController.SetSkin(skins[i].WoodMaterial, skins[i].SlingMaterial);
            }
        }
        currChunkPosition = initialChunkPosition;
        finalChunkPosition = initialChunkPosition;
        for (int i = 0; i < initialChunks; i++)
        {
            finalChunkPosition.z += depthChunkOffset;
            GameObject go = Instantiate(chunkViewPrefabs[Random.Range(0, chunkViewPrefabs.Length)].gameObject, finalChunkPosition, Quaternion.identity);
            ChunkView chunkView = go.GetComponent<ChunkView>();
            chunkView.Init(Random.Range(currLevel.MinCagesPerChunk, currLevel.MaxCagesPerChunk), currLevel.MinAnimalsPerCage, currLevel.MaxAnimalsPerCage, currLevel.CageAnimal, currLevel.IsAquatic, UpdateScore);
            poolChunks.Enqueue(chunkView);
        }

        virtualPurchases = EconomyService.Instance.Configuration.GetVirtualPurchases();
        virtualPurchases.RemoveAll((vp) => vp.Id.Contains("slinger") || vp.Id.Contains("SLINGER"));
    }
    IEnumerator ChangeSkybox()
    {
        yield return new WaitForSecondsRealtime(0.01f); // Wait for scene initialization

        switch (currLevel.Dificulty)
        {
            case LevelConfigSO.DIFICULTY.EASY:
                RenderSettings.skybox = dayMat;
                break;
            case LevelConfigSO.DIFICULTY.NORMAL:
                RenderSettings.skybox = bundleMat;
                break;
            case LevelConfigSO.DIFICULTY.HARD:
                RenderSettings.skybox = nightMat;
                break;
        }
    }
    private void Update()
    {
        if (slingController.transform.position.z > currChunkPosition.z)
        {
            if (poolChunks.Count < maxPoolSize)
            {
                GameObject go = Instantiate(chunkViewPrefabs[Random.Range(0, chunkViewPrefabs.Length)].gameObject, finalChunkPosition, Quaternion.identity);
                ChunkView chunkView = go.GetComponent<ChunkView>();
                chunkView.Init(Random.Range(currLevel.MinCagesPerChunk, currLevel.MaxCagesPerChunk), currLevel.MinAnimalsPerCage, currLevel.MaxAnimalsPerCage, currLevel.CageAnimal, currLevel.IsAquatic, UpdateScore);
                poolChunks.Enqueue(chunkView);
            }
            else
            {
                ChunkView chunkView = poolChunks.Dequeue();
                chunkView.DeInit();
                chunkView.transform.position = finalChunkPosition;
                chunkView.Init(Random.Range(currLevel.MinCagesPerChunk, currLevel.MaxCagesPerChunk), currLevel.MinAnimalsPerCage, currLevel.MaxAnimalsPerCage, currLevel.CageAnimal, currLevel.IsAquatic, UpdateScore);
                poolChunks.Enqueue(chunkView);
            }
            currChunkPosition.z += depthChunkOffset;
            finalChunkPosition.z += depthChunkOffset;
        }
        UpdateTimer();
    }
    #endregion

    #region PUBLIC_METHODS
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeScene("Gameplay", sceneName));
    }
    #endregion

    #region PRIVATE_METHODS
    private void UpdateTimer()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 5)
            {
                timerTxt.color = Color.red;
                timerTxt.ForceMeshUpdate();
            }
            else if (timer < 15)
            {
                timerTxt.color = Color.yellow;
                timerTxt.ForceMeshUpdate();
            }
        }
        else
        {
            if (endGame)
            {
                win = currScore > currLevel.MinScore;

                UpdateReward();
                cameraController.EndGame();
            }
            endGame = false;
        }
        timerTxt.text = ((int)timer).ToString();
    }
    private async void UpdateReward()
    {
        MakeVirtualPurchaseResult purchaseResult = null;
        slingController.OnGameBegined = false;
        if (win)
        {
            winAnimator.SetTrigger("toggle");

            int maxScore = 0;
            string LevelPlayedRaw = "level" + currLevel.Id + "dificuly" + ((int)currLevel.Dificulty);
            bool levelPlayed = PlayerPrefs.GetString(LevelPlayedRaw, string.Empty) != string.Empty;
            if (currScore < currLevel.OneStarScore)
            {
                reachedStars = 0;
                purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync("WIN_0");
                maxScore = currLevel.OneStarScore;
            }
            else if (currScore < currLevel.TwoStarScore)
            {
                starImg.sprite = oneStartImg;
                reachedStars = 1;
                maxScore = currLevel.TwoStarScore;
                if (levelPlayed)
                {
                    purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync("WIN_0");
                }
                else
                {
                    purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync("WIN_1");
                }
            }
            else if (currScore < currLevel.ThreeStarScore)
            {
                starImg.sprite = twoStartImg;
                reachedStars = 2;
                maxScore = currLevel.ThreeStarScore;
                if (levelPlayed)
                {
                    purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync("WIN_0");
                }
                else
                {
                    purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync("WIN_2");
                }
            }
            else
            {
                reachedStars = 3;
                if (levelPlayed)
                {
                    purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync("WIN_0");
                }
                else
                {
                    purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync("WIN_3");
                }
                starImg.sprite = threeStarImg;
                maxScore = -1;
            }

            if (!levelPlayed)
            {
                PlayerPrefs.SetString(LevelPlayedRaw, "true");
            }
            for (int i = 0; i < purchaseResult.Rewards.Currency.Count; i++)
            {
                if (purchaseResult.Rewards.Currency[i].Id == wincoinView.CurrencyId)
                {
                    wincoinView.Init(purchaseResult.Rewards.Currency[i].Amount);
                }
                if (purchaseResult.Rewards.Currency[i].Id == wingemView.CurrencyId)
                {
                    wingemView.Init(purchaseResult.Rewards.Currency[i].Amount);
                }
            }

            winScoreTxt.text = "SCORE: " + currScore + (maxScore >= 0 ? "/" + maxScore : "");
        }
        else
        {
            purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync("LOSE");
            loseAnimator.SetTrigger("toggle");
            loseScoreTxt.text = "SCORE: " + currScore + "/" + currLevel.MinScore;

            for (int i = 0; i < purchaseResult.Rewards.Currency.Count; i++)
            {
                if (purchaseResult.Rewards.Currency[i].Id == losecoinView.CurrencyId)
                {
                    losecoinView.Init(purchaseResult.Rewards.Currency[i].Amount);
                }
            }
        }

        await PersistentView.Instance.UpdateBalance();
        loseContinueBtn.interactable = true;
        winContinueBtn.interactable = true;

        if (currLevel.Id > 0)
        {
            string levelPlayedRaw = PlayerPrefs.GetString(levelsSavedKey);
            LevelsPlayedModel levelsPlayedModel = JsonConvert.DeserializeObject<LevelsPlayedModel>(levelPlayedRaw);
            if (levelsPlayedModel != null)
            {
                LevelPlayedModel levelPlayedModel = levelsPlayedModel.LevelsPlayedModels.Find((level) => level.Level == currLevel.Id);
                if (levelPlayedModel != null)
                {
                    levelsPlayedModel.LevelsPlayedModels.Remove(levelPlayedModel);
                    DificultyModel dificulty = levelPlayedModel.Dificulties.Find((dificulty) => dificulty.Dificulty == PersistentView.CurrStringDificulty);
                    if (dificulty != null)
                    {
                        levelPlayedModel.Dificulties.Remove(dificulty);
                        dificulty.ReachedStars = reachedStars;
                        dificulty.MaxScore = currScore;
                    }
                    else
                    {
                        dificulty = new DificultyModel()
                        {
                            Dificulty = PersistentView.CurrStringDificulty,
                            MaxScore = currScore,
                            ReachedStars = reachedStars
                        };
                    }
                    levelPlayedModel.Dificulties.Add(dificulty);
                    levelsPlayedModel.LevelsPlayedModels.Add(levelPlayedModel);
                }
                else
                {
                    levelsPlayedModel.LevelsPlayedModels.Add(new LevelPlayedModel()
                    {
                        Level = currLevel.Id,
                        Dificulties = new List<DificultyModel>()
                        {
                            new DificultyModel()
                            {
                                Dificulty = "EASY",
                                MaxScore = currScore,
                                ReachedStars = reachedStars
                            }
                        }
                    });
                }
            }
            else
            {
                levelsPlayedModel = new LevelsPlayedModel()
                {
                    LevelsPlayedModels = new List<LevelPlayedModel>()
                {
                    new LevelPlayedModel()
                    {
                        Level = currLevel.Id,
                        Dificulties = new List<DificultyModel>()
                            {
                                new DificultyModel()
                                {
                                    Dificulty = "EASY",
                                    MaxScore = currScore,
                                    ReachedStars = reachedStars
                                }
                            }
                    }
                }
                };
            }

            levelPlayedRaw = JsonConvert.SerializeObject(levelsPlayedModel);
            PlayerPrefs.SetString(levelsSavedKey, levelPlayedRaw);
        }
    }
    private void UpdateScore(int score)
    {
        currScore += (score * multScore);
        int maxScore;
        if (currScore < currLevel.MinScore)
        {
            maxScore = currLevel.MinScore;
        }
        else if (currScore < currLevel.OneStarScore)
        {
            maxScore = currLevel.OneStarScore;
        }
        else if (currScore < currLevel.TwoStarScore)
        {
            maxScore = currLevel.TwoStarScore;
        }
        else if (currScore < currLevel.ThreeStarScore)
        {
            maxScore = currLevel.ThreeStarScore;
        }
        else
        {
            maxScore = -1;
        }
        scoreTxt.text = currScore.ToString() + (maxScore >= 0 ? "/" + maxScore : "");
    }
    #endregion
}
