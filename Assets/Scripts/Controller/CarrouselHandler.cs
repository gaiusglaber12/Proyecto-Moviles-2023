using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.UI;

public class CarrouselHandler : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [Header("Sling")]
    [SerializeField] private List<MeshRenderer> woodRenderers = null;
    [SerializeField] private List<MeshRenderer> slingRenderers = null;
    [SerializeField] private SlingController slingController = null;

    [Header("Skins")]
    [SerializeField] private List<SlingSkinConfigsSO> skinConfigsSOs = null;


    [Header("UI")]
    [SerializeField] private GameObject coinGO = null;
    [SerializeField] private GameObject gemGO = null;
    [SerializeField] private Button buyButton = null;
    [SerializeField] private Button selectedButton = null;
    [SerializeField] private TMP_Text priceTxt = null;
    [SerializeField] private GameObject loadingHolder = null;
    #endregion

    #region PRIVATE_FIELDS
    private int index = 1;
    private List<VirtualPurchaseDefinition> virtualPurchases = null;
    private List<PlayersInventoryItem> playerItems = null;
    private bool cachedDirection = false;
    private PlayersInventoryItem selectedItem = null;
    #endregion

    #region CONSTANTS
    private const string selectedSlingerKey = "selectedSlinger";
    #endregion

    #region UNITY_CALLS
    private IEnumerator Start()
    {
        slingController.OnSpawned = ConfigureSlinger;
        yield return new WaitForSeconds(5); //DEBUG
        virtualPurchases = EconomyService.Instance.Configuration.GetVirtualPurchases();//DEBUG

        GetInventory();
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetDirection(bool direction)//right = true left = false
    {
        cachedDirection = direction;
        slingController.SetDespawnAnim();
    }
    #endregion

    #region PRIVATE_METHODS
    private async Task GetInventory()
    {
        //TO DO: select selectedItem from playerprefs

        await PersistentView.Instance.UpdateBalance();//TEMP
        GetInventoryOptions options = new GetInventoryOptions
        {
            ItemsPerFetch = 5
        };
        var op = await EconomyService.Instance.PlayerInventory.GetInventoryAsync(options);

        playerItems = op.PlayersInventoryItems;

        buyButton.onClick.AddListener(() =>
        {
            AttempttoBuyItem();
        });

        selectedButton.onClick.AddListener(() =>
        {
            selectedItem = playerItems.Find((playerItem) => playerItem.PlayersInventoryItemId == virtualPurchases[index].Rewards[0].Item.GetReferencedConfigurationItem().Id);
            PlayerPrefs.SetString(selectedSlingerKey, selectedItem.InventoryItemId);
        });

    }

    private async void AttempttoBuyItem()
    {
        loadingHolder.SetActive(true);//
        buyButton.interactable = false;
        string purchaseID = virtualPurchases[index].Id;

        MakeVirtualPurchaseResult purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(purchaseID);

        await GetInventory();
        await PersistentView.Instance.UpdateBalance();

        SetSlingerPrice(virtualPurchases[index].Costs[0].Amount);
        SetSlingerCurrencyType(virtualPurchases[index].Costs[0].Item.GetReferencedConfigurationItem().Id);
        loadingHolder.SetActive(false);
    }

    private void ConfigureSlinger()
    {
        if (cachedDirection)
        {
            index++;
            if (index > skinConfigsSOs.Count - 1)
            {
                index = 0;
            }
        }
        else
        {
            index--;
            if (index < 0)
            {
                index = skinConfigsSOs.Count - 1;
            }
        }
        SetWoodMaterial(skinConfigsSOs[index].WoodMaterial);
        SetSlingerMaterial(skinConfigsSOs[index].SlingMaterial);
        if (virtualPurchases != null)
        {
            SetSlingerPrice(virtualPurchases[index].Costs[0].Amount);
            SetSlingerCurrencyType(virtualPurchases[index].Costs[0].Item.GetReferencedConfigurationItem().Id);
        }
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
        if (index == 0 || HasCurrentItem())
        {
            priceTxt.text = "owned";
            buyButton.interactable = false;
            if (playerItems.Find((playerItem) => playerItem.PlayersInventoryItemId == selectedItem.InventoryItemId) != null)
            {
                selectedButton.interactable = true;
            }
            else
            {
                selectedButton.interactable = false;
            }
        }
        else
        {
            if (price > PersistentView.Instance.GetActualCurrencyById(virtualPurchases[index].Costs[0].Item.GetReferencedConfigurationItem().Id))
            {
                priceTxt.color = Color.red;
                priceTxt.ForceMeshUpdate();
                buyButton.interactable = false;
            }
            else
            {
                priceTxt.color = Color.white;
                priceTxt.ForceMeshUpdate();
                buyButton.interactable = true;
            }
            priceTxt.text = price.ToString();
        }
    }

    private void SetSlingerCurrencyType(string selectedCurrencyId)
    {
        if (index == 0 || HasCurrentItem())
        {
            gemGO.SetActive(false);
            coinGO.SetActive(false);
        }
        else
        {
            if (selectedCurrencyId == "GM")
            {
                gemGO.SetActive(true);
                coinGO.SetActive(false);
            }
            else if (selectedCurrencyId == "CO")
            {
                gemGO.SetActive(false);
                coinGO.SetActive(true);
            }
        }
    }

    private bool HasCurrentItem()
    {
        return playerItems.Find((item) => item.InventoryItemId == virtualPurchases[index].Rewards[0].Item.GetReferencedConfigurationItem().Id) != null;
    }
    #endregion
}
