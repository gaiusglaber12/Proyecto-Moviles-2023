using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeEntity : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public string id = string.Empty;
    #endregion

    #region PUBLIC_METHODS
    public abstract void Init(object data);
    public virtual CompositeEntity GetChild(string id) { return null; }
    #endregion
}
