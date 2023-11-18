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
        DificultyModel dificultyEnum = data as DificultyModel;
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

        dificultyBtn.onClick.AddListener(
            () =>
            {
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
