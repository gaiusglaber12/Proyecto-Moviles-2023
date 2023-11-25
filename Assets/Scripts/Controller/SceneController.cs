using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] public string currentScene = string.Empty;
    [SerializeField] private CanvasGroup canvasGroup = null;
    [SerializeField] private float fadeSpeed = 0.0f;

    protected IEnumerator ChangeScene(string currentScene, string nextScene)
    {
        while (canvasGroup.alpha < 0.99f)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, fadeSpeed);
            yield return null;
        }


        var op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        while (!op.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));

        SceneController[] sceneControllers = FindObjectsOfType<SceneController>();
        SceneController currSceneController = sceneControllers.ToList().Find((scene) => scene.currentScene != currentScene);
        StartCoroutine(currSceneController.FadeScene());
        SceneManager.UnloadSceneAsync(currentScene);
    }

    public IEnumerator FadeScene()
    {
        while (canvasGroup.alpha > 0.01f)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, fadeSpeed);
            yield return null;
        }
    }
}
