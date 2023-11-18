using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Button playBtn = null;
    #endregion

    #region UNITY_CALLS
    private IEnumerator Start()
    {
        playBtn.interactable = false;
        var op = SceneManager.LoadSceneAsync("Persistent", LoadSceneMode.Additive);
        while (!op.isDone)
        {
            yield return null;
        }
        playBtn.interactable = true;
        PersistentView.Instance.ToggleView(false);
    }
    #endregion

    #region PUBLIC_METHODS
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(StartChangeScene());
        IEnumerator StartChangeScene()
        {
            playBtn.interactable = false;
            var op = SceneManager.LoadSceneAsync("LevelSelector", LoadSceneMode.Additive);
            while (!op.isDone)
            {
                yield return null;
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelSelector"));

            SceneManager.UnloadSceneAsync("MainMenu");
        }
    }
    #endregion
}
