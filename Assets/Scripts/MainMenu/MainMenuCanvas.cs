using System;
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
    [SerializeField] private GameObject saveDataPanel = null;
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
        PersistentView.Instance.ToggleView(false);

        yield return new WaitUntil(() => PersistentView.Instance.initialized == true);

        if (PlayerPrefs.GetString("selectedSlinger") == string.Empty && PlayerPrefs.GetString("levelsPlayed") == string.Empty)
        {
            TryGetRemoteData(
                onSuccess: () =>
                {
                    saveDataPanel.SetActive(true);
                });
        }
        else
        {
            playBtn.interactable = true;
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public async void SetRemoteSaving()
    {
        saveDataPanel.SetActive(false);

        await PersistentView.Instance.RetrieveSpecificData("selectedSlinger",
            onSuccess: (selectedSlingerValue) =>
            {
                PlayerPrefs.SetString("selectedSlinger", selectedSlingerValue);
            }, onFailure: () =>
            {
                playBtn.interactable = true;
            });
        await PersistentView.Instance.RetrieveSpecificData("levelsPlayed",
            onSuccess: (levelsPlayedModel) =>
            {
                PlayerPrefs.SetString("levelsPlayed", levelsPlayedModel);
                playBtn.interactable = true;
            }, onFailure: () =>
            {
                playBtn.interactable = true;
            });
    }

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

    private async void TryGetRemoteData(Action onSuccess)
    {
        await PersistentView.Instance.RetrieveSpecificData("selectedSlinger",
            onSuccess: (selectedSlingerValue) =>
            {
                onSuccess.Invoke();
            }, onFailure: () =>
            {
                Debug.LogWarning("couldnt get remote data using local data instead");
                playBtn.interactable = true;
            });
    }
    #endregion
}
