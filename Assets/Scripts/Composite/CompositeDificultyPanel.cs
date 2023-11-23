using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompositeDificultyPanel : CompositeEntity
{
    #region EXPOSED_FIELDS
    [SerializeField] private Button dificultyBtn = null;
    [SerializeField] private CompositeStarPanel compositeStartPanels = null;
    [SerializeField] private Image panelImg = null;
    [SerializeField] private TMPro.TMP_Text dificultyText = null;
    #endregion

    #region PUBLIC_METHODS
    public override void Init(object data = null)
    {
        LevelPlayedModel levelPlayedModel = (((LevelPlayedModel, DificultyModel))data).Item1;
        DificultyModel dificultyModel = (((LevelPlayedModel, DificultyModel))data).Item2;
        switch (dificultyModel.Dificulty)
        {
            case "EASY":
                dificultyText.text = dificultyModel.Dificulty;
                panelImg.color = Color.green;
                break;
            case "NORMAL":
                dificultyText.text = dificultyModel.Dificulty;
                panelImg.color = Color.yellow;
                break;
            case "HARD":
                dificultyText.text = dificultyModel.Dificulty;
                panelImg.color = Color.red;
                break;
        }

        compositeStartPanels.Init(dificultyModel);

        dificultyBtn.onClick.AddListener(
            () =>
            {
                PersistentView.CurrLevel = levelPlayedModel.Level;
                PersistentView.CurrStringDificulty = dificultyModel.Dificulty;

                dificultyBtn.interactable = false;
                StartCoroutine(StartChangeScene());
                IEnumerator StartChangeScene()
                {
                    var op = SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
                    while (!op.isDone)
                    {
                        yield return null;
                    }

                    SceneManager.SetActiveScene(SceneManager.GetSceneByName("Gameplay"));
                    SceneManager.UnloadSceneAsync("LevelSelector");
                }
            });
    }
    #endregion
}
