using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeLevelPanel<TComposites> : CompositeEntity where TComposites : List<CompositeDificultyPanel<CompositeStarPanel>>
{
    #region EXPOSED_FIELDS
    [SerializeField] private TComposites compositesDificultyPanels;
    [SerializeField] private TMPro.TMP_Text levelTxt = null;
    #endregion

    #region PUBLIC_METHODS
    public override CompositeEntity GetChild(string id)
    {
        return compositesDificultyPanels.Find((composite) => composite.id == id);
    }

    public override void Init(object data)
    {
        LevelPlayedModel levelPlayedModel = data as LevelPlayedModel;
        levelTxt.text = levelPlayedModel.Level.ToString();
        for (int i = 0; i < compositesDificultyPanels.Count; i++)
        {
            compositesDificultyPanels[i].Init(levelPlayedModel);
        }
    }
    #endregion
}
