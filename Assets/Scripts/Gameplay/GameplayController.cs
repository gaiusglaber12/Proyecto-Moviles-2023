using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private LevelConfigSO debugConfig = null;
    [SerializeField] private LevelConfigSO[] levels = null;
    [SerializeField] private SlingController slingController = null;
    #endregion

    #region PRIVATE_FIELDS

    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        if (debugConfig != null)
        {
            slingController.InitGameplay(debugConfig.BallPrefab);
        }
    }
    #endregion
}
