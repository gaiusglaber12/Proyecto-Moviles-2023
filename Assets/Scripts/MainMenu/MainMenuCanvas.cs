using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private Button playBtn = null;
    [SerializeField] private Button showConsolseBtn = null;
    [SerializeField] private ScrollRect scrollView = null;
    [SerializeField] private TMPro.TMP_InputField inputField = null;
    [SerializeField] private TMPro.TMP_Text readFileTxt = null;

    #endregion

    #region PRIVATE_FIELDS
    private bool toggle = false;
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

    public void ToggleConsoleTab()
    {
        toggle = !toggle;
        scrollView.gameObject.SetActive(toggle);
    }

    public void ReadFile()
    {
        readFileTxt.text = FileController.ReadFile();
    }

    public void WriteFile()
    {
        if (inputField.text != string.Empty)
            FileController.WriteFile(inputField.text + "\n");
    }

    public void DeleteLogs()
    {
        FileController.DeleteFile();
    }
    #endregion
}
