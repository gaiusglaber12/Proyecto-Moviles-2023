using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : SceneController
{
    #region EXPOSED_FIELDS
    [SerializeField] private Button playBtn = null;
    [SerializeField] private Button showConsolseBtn = null;
    [SerializeField] private ScrollRect scrollView = null;
    [SerializeField] private TMPro.TMP_Text showConsoleTxt = null;
    [SerializeField] private TMPro.TMP_Text readFileTxt = null;

    #endregion

    #region PRIVATE_FIELDS
    private bool toggle = false;
    #endregion

    #region UNITY_CALLS
    private IEnumerator Start()
    {
        playBtn.interactable = false;

        var persistent = FindObjectOfType<PersistentView>();
        if (persistent == null)
        {
            var op = SceneManager.LoadSceneAsync("Persistent", LoadSceneMode.Additive);
            while (!op.isDone)
            {
                yield return null;
            }
        }
        StartCoroutine(FadeScene());
        playBtn.interactable = true;
        PersistentView.Instance.ToggleView(false);
    }
    #endregion

    #region PUBLIC_METHODS
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(ChangeScene("MainMenu", sceneName));
    }

    public void ToggleConsoleTab()
    {
        toggle = !toggle;
        if (toggle)
        {
            showConsoleTxt.text = "¡Hide Console!";
        }
        else
        {
            showConsoleTxt.text = "¡Show Console!";
        }
        scrollView.gameObject.SetActive(toggle);
    }

    public void ReadFile()
    {
        readFileTxt.text = FileController.ReadFile();
    }

    public void DeleteLogs()
    {
        FileController.DeleteFile();
    }
    #endregion
}
