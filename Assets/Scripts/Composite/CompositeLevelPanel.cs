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

            if (levelPlayedModel.Dificulties.Count < 3)
            {
                if (levelPlayedModel.Dificulties[levelPlayedModel.Dificulties.Count - 1].MaxScore > 0)
                {
                    DificultyModel dificultyModel = null;
                    switch (levelPlayedModel.Dificulties[levelPlayedModel.Dificulties.Count - 1].Dificulty)
                    {
                        
                        case "EASY":
                            dificultyModel = new DificultyModel()
                            {
                                Dificulty = "NORMAL",
                                MaxScore = 0,
                                ReachedStars = 0
                            };
                            levelPlayedModel.Dificulties.Add(dificultyModel);
                            break;
                        case "NORMAL":
                            dificultyModel = new DificultyModel()
                            {
                                Dificulty = "HARD",
                                MaxScore = 0,
                                ReachedStars = 0
                            };
                            levelPlayedModel.Dificulties.Add(dificultyModel);
                            break;
                    }
                }
            }

            levelTxt.text = levelPlayedModel.Level.ToString();
            for (int i = 0; i < levelPlayedModel.Dificulties.Count; i++)
            {
                GameObject go = Instantiate(compositesDificultyPanelPrefab.gameObject, dificultyHolder);
                var instantiatedDificultyPanel = go.GetComponent<CompositeDificultyPanel>();
                instantiatedDificultyPanel.Init((levelPlayedModel,levelPlayedModel.Dificulties[i]));
            }
        }
    }
    #endregion
}
