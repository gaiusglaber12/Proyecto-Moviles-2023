using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrouselHandler : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("Sling")]
    [SerializeField] private List<MeshRenderer> woodRendersrs = null;
    [SerializeField] private List<MeshRenderer> slingRenderers = null;
    [SerializeField] private Animator slingAnimator = null;

    [Header("Skins")]
    [SerializeField] private List<SlingSkinConfigsSO> skinConfigsSOs = null;

    [Header("UI")]
    [SerializeField] private Button leftBtn = null;
    [SerializeField] private Button rightBtn = null;

    #endregion

    #region PRIVATE_FIELDS

    #endregion

    #region UNITY_CALLS

    #endregion
}
