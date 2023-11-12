using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorPanel : MonoBehaviour
{
    #region CONSTANTS
    public const string levelsSavedKey = "levelsPlayed";
    #endregion

    #region EXPOSED_FIELDS
    [SerializeField] private LevelPanelComposisteConfigSO[] levelPanelComposisteConfigSOs = null;
    [SerializeField] private CompositeLevelPanel<List<CompositeDificultyPanel<CompositeStarPanel>>> levelPanelPrefab = null;
    [SerializeField] private GameObject unplayedLevelsPrefab = null;
    [SerializeField] private int maxLevels = 12;
    [SerializeField] private Transform goHolder = null;
    #endregion

    public void Awake()
    {
        string levelsPlayedRaw = PlayerPrefs.GetString(levelsSavedKey, string.Empty);
        if (string.IsNullOrEmpty(levelsPlayedRaw))
        {
            GameObject go = Instantiate(levelPanelPrefab, goHolder);
        }

        for (int i = 0; i < levelPanelComposisteConfigSOs.Length; i++)
        {
            GameObject go = Instantiate(levelPanelPrefab.gameObject, goHolder);
            var compositeLevelPanel = go.GetComponent<CompositeLevelPanel<List<CompositeDificultyPanel<CompositeStarPanel>>>>();
            compositeLevelPanel.Init(levelPanelComposisteConfigSOs[i]);
        }
    }
}
