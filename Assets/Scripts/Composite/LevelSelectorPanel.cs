using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

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
    #endregion

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
    }
}
