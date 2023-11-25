using UnityEngine;

public static class FileController
{
    #region PRIVATE_FIELDS
    private const string packName = "com.example.unityplugin";
    private const string loggerClassName = "FileController";

    private static AndroidJavaClass fileController = null;
    private static AndroidJavaObject fileControllerInstance = null;
    #endregion

    #region PUBLIC_METHODS
    public static string ReadFile()
    {
        if (fileControllerInstance == null)
        {
            Init();
        }
        string txt = fileControllerInstance?.Call<string>("ReadFile");

        return txt;
    }

    public static void WriteFile(string data)
    {
        if (fileControllerInstance == null)
        {
            Init();
        }
        fileControllerInstance?.Call("WriteFile", data + "\n");
    }

    public static void DeleteFile()
    {
        PopupController.Init();
        PopupController.ShowAlertDialog(new string[] { "Are you sure you want to delete logs?", "deleting logs.txt", "Delete", "Cancel" },
            (index) =>
            {
                Debug.Log("Index of button: " + index);
                if (index == -3)
                {
                    if (fileControllerInstance == null)
                    {
                        Init();
                    }
                    fileControllerInstance?.Call("DeleteFile");
                }
            });
    }
    #endregion

    #region PRIVATE_METHODS
    private static void Init()
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