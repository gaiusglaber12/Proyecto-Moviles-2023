using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class CarrouselHandler : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("Sling")]
    [SerializeField] private List<MeshRenderer> woodRenderers = null;
    [SerializeField] private List<MeshRenderer> slingRenderers = null;
    [SerializeField] private Animator slingAnimator = null;

    [Header("Skins")]
    [SerializeField] private List<SlingSkinConfigsSO> skinConfigsSOs = null;

    [Header("UI")]
    [SerializeField] private Image imgHolder = null;
    [SerializeField] private Sprite gemSprite = null;
    [SerializeField] private Sprite coinSprite = null;
    [SerializeField] private TMP_Text priceTxt = null;
    #endregion

    #region PRIVATE_FIELDS
    private int index = 0;
    private List<VirtualPurchaseDefinition> virtualPurchases = null;
    #endregion

    #region UNITY_CALLS
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        virtualPurchases = EconomyService.Instance.Configuration.GetVirtualPurchases();
        ConfigureSlinger();
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetDirection(bool direction)//right = true left = false
    {
        if (direction)
        {
            index++;
            if (index >= virtualPurchases.Count)
            {
                index = 0;
            }
        }
        else
        {
            if (index < 0)
            {
                index = virtualPurchases.Count - 1;
            }
        }
        ConfigureSlinger();
    }
    #endregion

    #region PRIVATE_METHODS
    private void ConfigureSlinger()
    {
        SetWoodMaterial(skinConfigsSOs[index].WoodMaterial);
        SetSlingerMaterial(skinConfigsSOs[index].SlingMaterial);
        SetSlingerPrice(virtualPurchases[index].Costs[0].Amount);
        SetSlingerCurrencyType(virtualPurchases[index].Costs[0].Item.GetReferencedConfigurationItem().Id);
    }

    private void SetWoodMaterial(Material material)
    {
        for (int i = 0; i < woodRenderers.Count; i++)
        {
            woodRenderers[i].material = material;
        }
    }

    private void SetSlingerMaterial(Material material)
    {
        for (int i = 0; i < slingRenderers.Count; i++)
        {
            slingRenderers[i].material = material;
        }
    }

    private void SetSlingerPrice(int price)
    {
        priceTxt.text = price.ToString();
    }

    private void SetSlingerCurrencyType(string selectedCurrencyId)
    {
        if (selectedCurrencyId == "GM")
        {
            imgHolder.sprite = gemSprite;
        }
        else if (selectedCurrencyId == "CO")
        {
            imgHolder.sprite = coinSprite;
        }
    }

    #endregion
}
