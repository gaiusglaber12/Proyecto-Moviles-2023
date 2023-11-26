using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarrouselHandler : SceneController
{
    #region EXPOSED_FIELDS
    [Header("Sling")]
    [SerializeField] private SlingController slingController = null;

    [Header("Skins")]
    [SerializeField] private List<SlingSkinConfigsSO> skinConfigsSOs = null;

    [Header("UI")]
    [SerializeField] private TMPro.TMP_Text totalSlingersTxt = null;
    [SerializeField] private GameObject coinGO = null;
    [SerializeField] private GameObject gemGO = null;
    [SerializeField] private Button buyButton = null;
    [SerializeField] private Button selectedButton = null;
    [SerializeField] private TMP_Text priceTxt = null;
    [SerializeField] private GameObject loadingHolder = null;

    [SerializeField] private Button leftButton = null;
    [SerializeField] private Button rightButton = null;
    [SerializeField] private Button goBackBtn = null;
    #endregion

    #region PRIVATE_FIELDS
    private int index = 0;
    private List<VirtualPurchaseDefinition> virtualPurchases = null;
    private List<PlayersInventoryItem> playerItems = null;
    private bool cachedDirection = false;
    private PlayersInventoryItem selectedItem = null;
    #endregion

    #region CONSTANTS
    private const string selectedSlingerKey = "selectedSlinger";
    private const string defaultSlinger = "SLINGER_0";
    #endregion

    #region UNITY_CALLS
    private async void Start()
    {
        await GetInventory();
        virtualPurchases = EconomyService.Instance.Configuration.GetVirtualPurchases();

        virtualPurchases.RemoveAll((virtualPurchase) => !virtualPurchase.Id.Contains("SLINGER"));

        PersistentView.Instance.ToggleView(true);
        slingController.OnSpawned = ConfigureSlinger;
        slingController.OnSpawnedFinished = ActivateButtons;
        leftButton.interactable = true;
        rightButton.interactable = true;
        totalSlingersTxt.text = "1/" + virtualPurchases.Count;
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetDirection(bool direction)//right = true left = false
    {
        leftButton.interactable = false;
        rightButton.interactable = false;
        cachedDirection = direction;
        slingController.SetDespawnAnim();
    }

    public void ChangeScene(string sceneName)
    {
        goBackBtn.interactable = false;
        StartCoroutine(ChangeScene("SkinsSelector", sceneName));
    }
    #endregion

    #region PRIVATE_METHODS
    private void ActivateButtons()
    {
        leftButton.interactable = true;
        rightButton.interactable = true;
    }

    private async Task GetInventory()
    {
        await PersistentView.Instance.UpdateBalance();//TEMP
        GetInventoryOptions options = new GetInventoryOptions
        {
            ItemsPerFetch = 5
        };
        var op = await EconomyService.Instance.PlayerInventory.GetInventoryAsync();

        playerItems = op.PlayersInventoryItems;

        string selectedSlingerId = PlayerPrefs.GetString(selectedSlingerKey, string.Empty);
        string auxId = string.IsNullOrEmpty(selectedSlingerId) ? defaultSlinger : selectedSlingerId;

        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].InventoryItemId == auxId)
            {
                selectedItem = playerItems[i];
            }
        }

        buyButton.onClick.AddListener(() =>
        {
            AttempttoBuyItem();
        });

        selectedButton.onClick.AddListener(() =>
        {
            for (int i = 0; i < playerItems.Count; i++)
            {
                string virtualPurchaseId = virtualPurchases[index].Rewards[0].Item.GetReferencedConfigurationItem().Id;
                if (playerItems[i].InventoryItemId == virtualPurchaseId)
                {
                    selectedItem = playerItems[i];
                }
            }
            PlayerPrefs.SetString(selectedSlingerKey, selectedItem.InventoryItemId);
            selectedButton.interactable = false;
        });

        leftButton.interactable = true;
        rightButton.interactable = true;
        Debug.LogWarning(" INITIALIZED ");
    }

    private async void AttempttoBuyItem()
    {
        loadingHolder.SetActive(true);//
        buyButton.interactable = false;
        string purchaseID = virtualPurchases[index].Id;

        MakeVirtualPurchaseResult purchaseResult = await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(purchaseID);

        await GetInventory();
        await PersistentView.Instance.UpdateBalance();

        SetSlingerPrice(virtualPurchases[index].Costs.Count == 0 ? 0 : virtualPurchases[index].Costs[0].Amount);
        SetSlingerCurrencyType(virtualPurchases[index].Costs.Count == 0 ? "owned" : virtualPurchases[index].Costs[0].Item.GetReferencedConfigurationItem().Id);

        string firstPurchase = PlayerPrefs.GetString("firstPurchase", string.Empty);
        if (firstPurchase != string.Empty)
        {
            PlayerPrefs.SetString("firstPurchase", "true1");
            PlayGamesPlatform.Instance.ReportProgress("CgkI-NmMitEJEAIQAQ", 100,
                (state) =>
                {
                    if (state)
                    {
                        Debug.Log("achievement unlocked succefully");
                    }
                    else
                    {
                        Debug.Log("achievement dont unlocked succefully");
                    }
                });
        }
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
        slingController.SetSkin(skinConfigsSOs[index].WoodMaterial, skinConfigsSOs[index].SlingMaterial);
        if (virtualPurchases != null)
        {
            SetSlingerPrice(virtualPurchases[index].Costs.Count == 0 ? 0 : virtualPurchases[index].Costs[0].Amount);
            SetSlingerCurrencyType(virtualPurchases[index].Costs.Count == 0 ? "owned" : virtualPurchases[index].Costs[0].Item.GetReferencedConfigurationItem().Id);
        }
        if (virtualPurchases != null)
        {
            totalSlingersTxt.text = (index + 1) + "/" + virtualPurchases.Count;
        }
    }

    private void SetSlingerPrice(int price)
    {
        if (HasCurrentItem())
        {
            priceTxt.text = "owned";
            priceTxt.color = Color.white;
            priceTxt.ForceMeshUpdate();
            buyButton.interactable = true;
            buyButton.interactable = false;
            if (selectedItem.InventoryItemId != virtualPurchases[index].Rewards[0].Item.GetReferencedConfigurationItem().Id)
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
            if (virtualPurchases[index].Costs.Count > 0)
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
    }

    private void SetSlingerCurrencyType(string selectedCurrencyId)
    {
        if (HasCurrentItem())
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
