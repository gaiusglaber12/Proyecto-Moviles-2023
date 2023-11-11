using TMPro;
using UnityEngine;

public class CurrencyView : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private string currencyId = string.Empty;
    [SerializeField] private TMP_Text currencyText = null;
    [SerializeField] private float lerperSpeed = 0.5f;
    #endregion

    #region PRIVATE_FIELDS
    private long actualCurrency = 0;
    private long lerpToCurrency = 0;
    private float lerpingCurrency = 0.0f;
    #endregion

    #region PROPERTIES
    public string CurrencyId { get => currencyId; }
    public long CantCurrency { get => lerpToCurrency; }
    #endregion

    #region UNITY_CALLS
    private void Update()
    {
        if (lerpToCurrency != actualCurrency)
        {
            lerpingCurrency = Mathf.Lerp(lerpingCurrency, lerpToCurrency, lerperSpeed);
            actualCurrency = Mathf.RoundToInt(lerpingCurrency);
            currencyText.text = actualCurrency.ToString();
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void Init(long userCurrency)
    {
        lerpToCurrency = userCurrency;
    }
    #endregion
}
