using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompositeLevelPanel : CompositeEntity
{
    #region EXPOSED_FIELDS
    [SerializeField] private CompositeDificultyPanel compositesDificultyPanelPrefab;
    [SerializeField] private TMPro.TMP_Text levelTxt = null;
    [SerializeField] private Transform dificultyHolder = null;
    #endregion

    #region ACTIONS
    private Action<string> onChangeScene = null;
    #endregion

    #region PUBLIC_METHODS
    public override void Init(object data)
    {
        if (data == null)
        {
            GameObject go = Instantiate(compositesDificultyPanelPrefab.gameObject, dificultyHolder);
            var instantiatedDificultyPanel = go.GetComponent<CompositeDificultyPanel>();
            instantiatedDificultyPanel.SetChangeScene(onChangeScene);
            instantiatedDificultyPanel.Init((new LevelPlayedModel()
            {
                Level = 1
            }, (new DificultyModel()
            {
                Dificulty = "EASY",
                MaxScore = 0,
                ReachedStars = 0
            })));
        }
        else
        {
            LevelPlayedModel levelPlayedModel = data as LevelPlayedModel;

            if (levelPlayedModel.Dificulties.Count < 3)
            {
                if (levelPlayedModel.Dificulties[levelPlayedModel.Dificulties.Count - 1].MaxScore > 0)
                {
                    DificultyModel dificultyModel;
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

            // Definir un comparador personalizado que ordene los strings según el orden inverso al deseado
            Comparer<string> comparador = Comparer<string>.Create((x, y) =>
            {
                // Si los strings son iguales, devolver 0
                if (x == y) return 0;

                // Si x es "EASY", devolver -1 (x va antes que y)
                if (x == "EASY") return -1;

                // Si y es "EASY", devolver 1 (y va antes que x)
                if (y == "EASY") return 1;

                // Si x es "NORMAL", devolver -1 (x va antes que y)
                if (x == "NORMAL") return -1;

                // Si y es "NORMAL", devolver 1 (y va antes que x)
                if (y == "NORMAL") return 1;

                // Si x es "HARD", devolver -1 (x va antes que y)
                if (x == "HARD") return -1;

                // Si y es "HARD", devolver 1 (y va antes que x)
                if (y == "HARD") return 1;

                // En cualquier otro caso, devolver 0
                return 0;
            });
            levelPlayedModel.Dificulties.Sort((x,y)=>comparador.Compare(x.Dificulty,y.Dificulty));
            levelTxt.text = levelPlayedModel.Level.ToString();
            for (int i = 0; i < levelPlayedModel.Dificulties.Count; i++)
            {
                GameObject go = Instantiate(compositesDificultyPanelPrefab.gameObject, dificultyHolder);
                var instantiatedDificultyPanel = go.GetComponent<CompositeDificultyPanel>();
                instantiatedDificultyPanel.SetChangeScene(onChangeScene);
                instantiatedDificultyPanel.Init((levelPlayedModel, levelPlayedModel.Dificulties[i]));
            }
        }
    }

    public void SetOnChangeScene(Action<string> onChangeScene)
    {
        this.onChangeScene = onChangeScene;
    }

    public void SetOnToggle(Action<CompositeLevelPanel> onToggle)
    {
        var btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(()=>onToggle.Invoke(this));
    }
    #endregion
}
