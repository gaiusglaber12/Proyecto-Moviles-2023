using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompositeDificultyPanel<TComposite> : CompositeEntity where TComposite : CompositeStarPanel
{
    #region EXPOSED_FIELDS
    [SerializeField] private TComposite compositeStartPanels = null;
    [SerializeField] private Image panelImg = null;
    [SerializeField] private TMPro.TMP_Text dificultyText = null;
    #endregion

    #region PUBLIC_METHODS
    public override void Init(object data = null)
    {
        LevelPlayedModel dificultyEnum = data as LevelPlayedModel;
        switch (dificultyEnum.Dificulty)
        {
            case "EASY":
                dificultyText.text = dificultyEnum.Dificulty;
                panelImg.color = Color.green;
                break;
            case "NORMAL":
                dificultyText.text = dificultyEnum.Dificulty;
                panelImg.color = Color.yellow;
                break;
            case "HARD":
                dificultyText.text = dificultyEnum.Dificulty;
                panelImg.color = Color.red;
                break;
        }

        compositeStartPanels.Init(dificultyEnum);
    }
    #endregion
}
