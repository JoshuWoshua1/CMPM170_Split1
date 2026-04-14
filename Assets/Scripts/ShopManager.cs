using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    public int playerGold = 100;
    public TMP_Text goldText;
    public Transform playerInventoryPanel;
    public Transform shopInventoryPanel;

    private void Awake() {
        Instance = this;
        UpdateGoldDisplay();
    }

    public void BuyItem(ShopItemHandler item) {
        ShopSlot slot = item.GetComponentInParent<ShopSlot>();
        Item itemData = item.GetComponent<Item>();
        if (slot == null || itemData == null) return;

        if (playerGold < itemData.buyPrice) {
            Debug.Log("Not enough gold!");
            return;
        }

        playerGold -= itemData.buyPrice;
        UpdateGoldDisplay();

        // move to first empty player slot
        ShopSlot emptySlot = FindEmptySlot(playerInventoryPanel);
        if (emptySlot != null) {
            // slot.currentItem = null;
            ShopItemHandler boughtItem = Instantiate(item);
            // item.transform.SetParent(emptySlot.transform);
            boughtItem.transform.SetParent(emptySlot.transform);
            boughtItem.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f,-50f, 1f);
            emptySlot.currentItem = boughtItem.gameObject;
            boughtItem.isShopItem = false;
            boughtItem.transform.Find("Chest").transform.Find("Generate Button").gameObject.SetActive(true);
        }
    }

    public void SellItem(ShopItemHandler item) {
        ShopSlot slot = item.GetComponentInParent<ShopSlot>();
        Item itemData = item.GetComponent<Item>();
        if (slot == null || itemData == null) return;

        playerGold += itemData.GetSellPrice();
        UpdateGoldDisplay();

        // move to empty shop slot instead of destroying
        // ShopSlot emptySlot = FindEmptySlot(shopInventoryPanel);
        // // if (emptySlot != null) {
        // //     slot.currentItem = null;
        // //     item.transform.SetParent(emptySlot.transform);
        // //     item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        // //     emptySlot.currentItem = item.gameObject;
        // //     item.isShopItem = true;
        //     // no empty shop slot, just destroy
            slot.currentItem = null;
            Destroy(item.gameObject);
    }

    private ShopSlot FindEmptySlot(Transform panel) {
        foreach (Transform child in panel) {
            ShopSlot slot = child.GetComponent<ShopSlot>();
            if (slot != null && slot.currentItem == null) return slot;
        }
        return null;
    }

    private void UpdateGoldDisplay() {
        if (goldText) goldText.text = playerGold.ToString();
    }
}