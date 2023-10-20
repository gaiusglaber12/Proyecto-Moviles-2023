using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginTest : MonoBehaviour
{
#if UNITY_ANDROID
    #region PRIVATE_FIELDS
    private AndroidJavaClass unityClass;
    private AndroidJavaObject unityActivity;
    private AndroidJavaObject pluginInstance;
    #endregion

    #region UNITY_CALLS
    void Start()
    {
        InitializePlugin("com.example.unityplugin.PluginInstance");
    }
    #endregion

    #region PUBLIC_METHODS
    public void Toast()
    {
        if (pluginInstance != null)
        {
            pluginInstance.Call("Toast", "Hello world!");
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private void InitializePlugin(string pluginName)
    {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        pluginInstance = new AndroidJavaObject(pluginName);
        if (pluginInstance == null)
        {
            Debug.Log("Failed to initialize plugin");
        }
        else
        {
            pluginInstance.CallStatic("receiveUnityActivity", unityActivity);
        }

    }
    #endregion
#endif
}
