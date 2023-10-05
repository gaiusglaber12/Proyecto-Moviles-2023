using System.Collections.Generic;

using UnityEngine;

public class TrayectoryHandler : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private GameObject trayectoryRenderGo = null;

    [SerializeField] private Transform trayectoryHolder = null;
    [SerializeField] private int ammountOfRenders = 0;
    #endregion

    #region PRIVATE_FIELDS
    private GameObject go = null;

    private Vector3 initialPoint = Vector3.zero;
    private Vector3 heightValue = Vector3.zero;
    private Vector3 depthValue = Vector3.zero;

    private List<GameObject> trayectoryGos = null;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        SetTrayectoryGos();
    }
    #endregion

    #region PUBLIC_METHODS
    public void Init(GameObject go)
    {
        this.go = go;
        trayectoryGos = new List<GameObject>();
        for (int i = 0; i < ammountOfRenders; i++)
        {
            GameObject actualGo = Instantiate(trayectoryRenderGo);
            actualGo.transform.parent = trayectoryHolder;
            trayectoryGos.Add(actualGo);
        }
    }

    public void DrawTrayectory(Vector3 initialPoint, Vector3 heightValue, Vector3 depthValue)
    {
        this.initialPoint = initialPoint;
        this.heightValue = heightValue;
        this.depthValue = depthValue;
        Debug.Log("InitialValue: " + initialPoint + "HeightValue: " + heightValue + "DepthValue: " + depthValue);
    }

    public void ToggleTrayectory(bool toggle)
    {
        if (trayectoryGos == null)
        {
            return;
        }
        for (int i = 0; i < ammountOfRenders; i++)
        {
            trayectoryGos[i].SetActive(toggle);
        }
    }
    #endregion

    #region PRIVATE_METHODS
    private Vector3 EvaluatePosition(float t)
    {
        Vector3 ac = Vector3.Lerp(initialPoint, heightValue, t);
        Vector3 cb = Vector3.Lerp(heightValue, depthValue, t);
        return Vector3.Lerp(ac, cb, t);
    }

    private void SetTrayectoryGos()
    {
        if (trayectoryGos == null)
        {
            return;
        }

        for (int i = 0; i < trayectoryGos.Count; i++)
        {
            trayectoryGos[i].transform.position = EvaluatePosition(i / (float)trayectoryGos.Count);
        }
    }
    #endregion
}
