using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorPanel : SceneController
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

    #region PRIVATE_FIELDS
    private List<CompositeLevelPanel> levelPlayedViews = null;
    #endregion

    #region UNITY_CALLS
    public void Awake()
    {
        levelPlayedViews = new List<CompositeLevelPanel>();

        PersistentView.Instance.ToggleView(false);

        string levelsPlayedRaw = PlayerPrefs.GetString(levelsSavedKey, string.Empty);
        if (string.IsNullOrEmpty(levelsPlayedRaw))
        {
            GameObject go = Instantiate(levelPanelPrefab.gameObject, goHolder);
            var instantiatedLevelPanelPrefab = go.GetComponent<CompositeLevelPanel>();
            instantiatedLevelPanelPrefab.SetOnChangeScene(ChangeScene);
            instantiatedLevelPanelPrefab.Init(null);
            instantiatedLevelPanelPrefab.SetOnToggle(SetOnToggle);
            levelPlayedViews.Add(instantiatedLevelPanelPrefab);
            for (int i = 0; i < maxLevelsPerPage - 1; i++)
            {
                var clpgo = Instantiate(unplayedLevelsPrefab, goHolder);
            }
        }
        else
        {
            LevelsPlayedModel levelsPlayedModel = JsonConvert.DeserializeObject<LevelsPlayedModel>(levelsPlayedRaw);

            levelsPlayedModel.LevelsPlayedModels = levelsPlayedModel.LevelsPlayedModels.OrderBy(level=>level.Level).ToList();

            if (levelsPlayedModel.LevelsPlayedModels.Count < maxLevelsPerPage)
            {
                if (levelsPlayedModel.LevelsPlayedModels[levelsPlayedModel.LevelsPlayedModels.Count - 1].Dificulties.Count > 0)
                {
                    levelsPlayedModel.LevelsPlayedModels.Add(new LevelPlayedModel()
                    {
                        Level = levelsPlayedModel.LevelsPlayedModels.Count + 1,
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
                instantiatedLevelPanelPrefab.SetOnChangeScene(ChangeScene);
                instantiatedLevelPanelPrefab.Init(levelsPlayedModel.LevelsPlayedModels[i]);
                instantiatedLevelPanelPrefab.SetOnToggle(SetOnToggle);
                levelPlayedViews.Add(instantiatedLevelPanelPrefab);
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
        StartCoroutine(ChangeScene("LevelSelector", sceneName));
    }
    #endregion

    #region PRIVATE_METHODS
    private void SetOnToggle(CompositeLevelPanel compositeLevelPanel)
    {
        for (int i = 0; i < levelPlayedViews.Count; i++)
        {
            levelPlayedViews[i].Toggle(false);
        }
        compositeLevelPanel.Toggle(true);
    }
    #endregion
}
