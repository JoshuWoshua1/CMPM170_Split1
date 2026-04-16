using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    public int quantity = 1;
    private TMP_Text quantityText;

    public int buyPrice = 10;
    public float sellPriceMultiplier = 0.5f;

    private void Awake() {
        quantityText = GetComponentInChildren<TMP_Text>();
        UpdateQuantityDisplay();
    }

    public int GetSellPrice() {
        return Mathf.RoundToInt(buyPrice * sellPriceMultiplier);
    }

    public void UpdateQuantityDisplay() {
        if(quantityText != null) {
            quantityText.text = quantity > 1 ? quantity.ToString() : "";
        }
    }

    public void AddToStack(int amount = 1) {
        quantity += amount;
        UpdateQuantityDisplay();
    }
    public int RemoveFromStack(int amount = 1) {
        int removed = Mathf.Min(amount, quantity);
        quantity -= removed;
        UpdateQuantityDisplay();
        return removed;
    }
}
