using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSlot : MonoBehaviour, IDropHandler
{
    public GameObject currentItem; // The item currently held in this shop slot
    public int itemPrice;
    public TMP_Text priceText;
    public bool isShopSlot = true; // In shop menu, true = shop side and false = player side
    
    public void OnDrop(PointerEventData eventData) {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        ItemDragHandler drag = dropped.GetComponent<ItemDragHandler>();
        if (drag == null) return;

        ShopSlot originalSlot = drag.originalParent.GetComponent<ShopSlot>();

        if (currentItem != null) {
            // swap - move existing item to original slot
            currentItem.transform.SetParent(drag.originalParent);
            currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            if (originalSlot != null) originalSlot.currentItem = currentItem;
        } else {
            if (originalSlot != null) originalSlot.currentItem = null;
        }

        dropped.transform.SetParent(transform);
        dropped.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        currentItem = dropped;
    }

    private void Awake() {
        if(!priceText) {
            priceText = transform.Find("PriceText").GetComponent<TMP_Text>();
        }
    }

    public void UpdatePriceDisplay() {
        if(priceText && currentItem) {
            priceText.text = itemPrice.ToString();
        }
    }

    public void SetItem(GameObject item, int price) {
        currentItem = item;
        itemPrice = price;
        UpdatePriceDisplay();
    }

}
