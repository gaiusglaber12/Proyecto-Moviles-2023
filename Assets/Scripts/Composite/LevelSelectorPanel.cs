using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorPanel : MonoBehaviour
{
    #region CONSTANTS
    public const string levelsSavedKey = "levelsPlayed";
    #endregion

    #region EXPOSED_FIELDS
    [SerializeField] private CompositeLevelPanel levelPanelPrefab = null;
    [SerializeField] private GameObject unplayedLevelsPrefab = null;
    [SerializeField] private int maxLevelsPerPage = 12;
    [SerializeField] private Transform goHolder = null;
    [SerializeField] private Button skinShopBtn = null;
    #endregion

    #region UNITY_CALLS
    public void Awake()
    {
        string levelsPlayedRaw = PlayerPrefs.GetString(levelsSavedKey, string.Empty);
        if (string.IsNullOrEmpty(levelsPlayedRaw))
        {
            GameObject go = Instantiate(levelPanelPrefab.gameObject, goHolder);
            var instantiatedLevelPanelPrefab = go.GetComponent<CompositeLevelPanel>();
            instantiatedLevelPanelPrefab.Init(null);
            for (int i = 0; i < maxLevelsPerPage - 1; i++)
            {
                Instantiate(unplayedLevelsPrefab, goHolder);
            }
        }
        else
        {
            LevelsPlayedModel levelsPlayedModel = JsonConvert.DeserializeObject<LevelsPlayedModel>(levelsPlayedRaw);

            if (levelsPlayedModel.LevelsPlayedModels.Count < maxLevelsPerPage)
            {
                if (levelsPlayedModel.LevelsPlayedModels[levelsPlayedModel.LevelsPlayedModels.Count - 1].Dificulties.Count > 0) 
                {
                    levelsPlayedModel.LevelsPlayedModels.Add(new LevelPlayedModel()
                        {
                            Level = levelsPlayedModel.LevelsPlayedModels.Count,
                            Dificulties = new List<DificultyModel>()
                            {
                                new DificultyModel()
                                {
                                    Dificulty = "EASY",
                                    MaxScore= 0,
                                    ReachedStars = 0
                                }
                            }
                    });
                }
            }

            for (int i = 0; i < levelsPlayedModel.LevelsPlayedModels.Count; i++)
            {
                GameObject go = Instantiate(levelPanelPrefab.gameObject, goHolder);
                var instantiatedLevelPanelPrefab = go.GetComponent<CompositeLevelPanel>();
                instantiatedLevelPanelPrefab.Init(levelsPlayedModel.LevelsPlayedModels[i]);
            }

            for (int i = 0; i < maxLevelsPerPage - levelsPlayedModel.LevelsPlayedModels.Count - 1; i++)
            {
                Instantiate(unplayedLevelsPrefab, goHolder);
            }
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(StartChangeScene());
        IEnumerator StartChangeScene()
        {
            skinShopBtn.interactable = false;
            var op = SceneManager.LoadSceneAsync("SkinsSelector", LoadSceneMode.Additive);
            while (!op.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("SkinsSelector"));
            SceneManager.UnloadSceneAsync("LevelSelector");
        }
    }
    #endregion
}
