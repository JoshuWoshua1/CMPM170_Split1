using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShopItemHandler : MonoBehaviour, IPointerClickHandler
{
    public bool isShopItem;

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (isShopItem) {
                ShopManager.Instance.BuyItem(this);
            } else {
                ShopManager.Instance.SellItem(this);
            }
        }
    }
}