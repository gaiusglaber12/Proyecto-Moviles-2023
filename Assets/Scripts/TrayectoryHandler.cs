using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class TrayectoryHandler : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private LineRenderer lineRenderer = null;
    [SerializeField] private int ammountOfRenders = 0;
    [SerializeField] private SlingController slingController = null;
    #endregion

    #region PRIVATE_FIELDS
    private Vector3 initialPoint = Vector3.zero;
    private Vector3 heightValue = Vector3.zero;
    private Vector3 depthValue = Vector3.zero;
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        SetLineVertex();
        SetLineColor();
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetMatrix(Vector3 initialPoint, Vector3 heightValue, Vector3 depthValue)
    {
        this.initialPoint = initialPoint;
        this.heightValue = heightValue;
        this.depthValue = depthValue;
        Debug.Log("InitialValue: " + initialPoint + "HeightValue: " + heightValue + "DepthValue: " + depthValue);
    }

    public void ToggleTrayectory(bool toggle)
    {
        lineRenderer.positionCount = toggle ? ammountOfRenders : 0;
    }
    #endregion

    #region PRIVATE_METHODS
    private Vector3 EvaluatePosition(float t)
    {
        Vector3 ac = Vector3.Lerp(initialPoint, heightValue, t);
        Vector3 cb = Vector3.Lerp(heightValue, depthValue, t);
        return Vector3.Lerp(ac, cb, t);
    }

    private Color EvaluateColor(float t)
    {
        if (t < 0.5f)
        {
            Color gy = Color.Lerp(Color.green, Color.yellow, t * 2);
            return gy;
        }
        else
        {
            Color yr = Color.Lerp(Color.yellow, Color.red, (t - 0.5f) * 2);
            return yr;
        }
    }

    private void SetLineVertex()
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, EvaluatePosition(i / (float)lineRenderer.positionCount));
        }
    }

    private void SetLineColor()
    {
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = EvaluateColor(slingController.CurrDepth * 0.9f / slingController.MaxDepth);
    }
    #endregion
}
