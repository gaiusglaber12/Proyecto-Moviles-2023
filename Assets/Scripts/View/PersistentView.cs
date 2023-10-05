using System.Collections.Generic;

using UnityEngine;

using Unity.Services.Economy.Model;

public class PersistentView : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private List<CurrencyView> currencyViews = null;
    #endregion

    #region PUBLIC_METHODS
    public void Init(List<PlayerBalance> playerBalance)
    {
        for (int i = 0; i < currencyViews.Count; i++)
        {
            for (int j = 0; j < playerBalance.Count; j++)
            {
                if (currencyViews[i].CurrencyId == playerBalance[j].CurrencyId)
                {
                    currencyViews[i].Init(playerBalance[j].Balance);
                }
            }
        }
        Debug.Log("PersistentView initialized");
    }
    #endregion
}
