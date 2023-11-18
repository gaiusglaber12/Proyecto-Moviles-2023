using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeLevelPanel : CompositeEntity
{
    #region EXPOSED_FIELDS
    [SerializeField] private CompositeDificultyPanel compositesDificultyPanelPrefab;
    [SerializeField] private TMPro.TMP_Text levelTxt = null;
    [SerializeField] private Transform dificultyHolder = null;
    #endregion

    #region PUBLIC_METHODS
    public override void Init(object data)
    {
        if (data == null)
        {
            GameObject go = Instantiate(compositesDificultyPanelPrefab.gameObject, dificultyHolder);
            var instantiatedDificultyPanel = go.GetComponent<CompositeDificultyPanel>();
            instantiatedDificultyPanel.Init(new DificultyModel()
            {
                Dificulty = "EASY",
                MaxScore = 0,
                ReachedStars = 0
            });
        }
        else
        {
            LevelPlayedModel levelPlayedModel = data as LevelPlayedModel;
            levelTxt.text = levelPlayedModel.Level.ToString();

        }
    }
    #endregion
}
