using UnityEngine;

public class FileController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private TMPro.TMP_Text tmp = null;
    #endregion

    #region PRIVATE_FIELDS
    private const string packName = "com.example.unityplugin";
    private const string loggerClassName = "FileController";

    private AndroidJavaClass fileController = null;
    private AndroidJavaObject fileControllerInstance = null;
    #endregion

    #region PUBLIC_METHODS
    public void ReadFile()
    {
        if (fileControllerInstance == null)
        {
            Init();
        }
        string txt = fileControllerInstance?.Call<string>("ReadFile");
        tmp.text = txt;
    }

    public void WriteFile(string data)
    {
        if (fileControllerInstance == null)
        {
            Init();
        }
        fileControllerInstance?.Call("WriteFile", data);
    }
    #endregion

    #region PRIVATE_METHODS
    private void Init()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        fileController = new AndroidJavaClass(packName + "." + loggerClassName);
        AndroidJavaClass unityJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityJC.GetStatic<AndroidJavaObject>("currentActivity");
        fileController.SetStatic("mainActivity", activity);

        fileControllerInstance = fileController.CallStatic<AndroidJavaObject>("GetInstance");
#endif
    }
    #endregion
}