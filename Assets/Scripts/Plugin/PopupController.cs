using UnityEngine;

public static class PopupController
{
    #region CLASS
    class AlertViewCallBack : AndroidJavaProxy
    {
        private System.Action<int> alertHandler = null;

        public AlertViewCallBack(System.Action<int> alertHandlerIn) : base(packName + "." + loggerClassName + "$AlertViewCallBack")
        {
            alertHandler = alertHandlerIn;
        }

        public void OnButtonTapped(int index)
        {
            Debug.Log("Button tapped: " + index);
            alertHandler?.Invoke(index);
        }
    }

    #endregion

    #region PRIVATE_FIELDS
    private const string packName = "com.example.unityplugin";
    private const string loggerClassName = "PopupController";

    private static AndroidJavaClass popupController = null;
    private static AndroidJavaObject popupControllerInstance = null;

    private static string title = "TitleText";
    private static string message = "MessageText";
    private static string button1 = "Close";
    #endregion

    #region PUBLIC_METHODS
    public static void Init()
    {
        if (popupControllerInstance == null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        popupController = new AndroidJavaClass(packName + "." + loggerClassName);
        AndroidJavaClass unityJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityJC.GetStatic<AndroidJavaObject>("currentActivity");
        popupController.SetStatic("mainActivity", activity);

        popupControllerInstance = popupController.CallStatic<AndroidJavaObject>("GetInstance");
#endif
        }
    }

    public static void ShowAlertDialog(string[] strings, System.Action<int> handler = null)
    {
        if (strings.Length < 3)
        {
            Debug.LogError("AlertView requires at least 3 strings");
            return;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            popupControllerInstance?.Call("ShowAlertView", new object[] { strings, new AlertViewCallBack(handler) });
        }
        else
        {
            Debug.LogWarning("AlertView not supported on this platform");
        }
    }
    #endregion
}