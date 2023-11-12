using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompositeStarPanel : CompositeEntity
{
    #region EXPOSED_FIELDS
    [SerializeField] private TMPro.TMP_Text scoreTxt = null;
    [SerializeField] private Image starImage = null;

    [SerializeField] private Sprite star0Sprite = null;
    [SerializeField] private Sprite star1Sprite = null;
    [SerializeField] private Sprite star2Sprite = null;
    [SerializeField] private Sprite star3Sprite = null;
    #endregion

    #region PUBLIC_METHODS
    public override void Init(object data)
    {
        DificultyModel levelPlayedModel = data as DificultyModel;
        scoreTxt.text = "MAX SCORE: ";
        if (levelPlayedModel.MaxScore != 0)
        {
            scoreTxt.text += levelPlayedModel.MaxScore.ToString();
            switch (levelPlayedModel.ReachedStars)
            {
                case 0:
                    starImage.sprite = star0Sprite;
                    break;
                case 1:
                    starImage.sprite = star1Sprite;
                    break;
                case 2:
                    starImage.sprite = star2Sprite;
                    break;
                case 3:
                    starImage.sprite = star3Sprite;
                    break;
            }
        }
        else
        {
            scoreTxt.gameObject.SetActive(false);
            starImage.gameObject.SetActive(false);
        }
    }
    #endregion
}
