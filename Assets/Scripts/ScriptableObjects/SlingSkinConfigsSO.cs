using System;

using UnityEngine;

[CreateAssetMenu(fileName = "SlingSkinConfig", menuName = "Configs/SlingSkinConfig")]
public class SlingSkinConfigsSO : ScriptableObject
{
    #region EXPOSED_FIELDS
    [SerializeField] private string remoteId = string.Empty;
    [SerializeField] private Material woodMaterial = null;
    [SerializeField] private Material slingMaterial = null;
    [NonSerialized] public int cost = 0;
    [NonSerialized] public string vcType = string.Empty;
    #endregion

    #region PROPERTIES
    public Material WoodMaterial { get => woodMaterial; }
    public Material SlingMaterial { get => slingMaterial; }
    public string RemoteId { get => remoteId; }
    #endregion
}
