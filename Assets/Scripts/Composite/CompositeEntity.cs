using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeEntity : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public string id = string.Empty;
    [SerializeField] private Animator animator = null;
    public Action onToggle = null;
    #endregion

    #region PUBLIC_METHODS
    public abstract void Init(object data);
    public virtual void Toggle(bool toggle)
    {
        animator.SetBool("toggle", toggle);
    }
    public virtual CompositeEntity GetChild(string id) { return null; }
    #endregion
}
