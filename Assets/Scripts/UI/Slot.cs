using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public GameObject currentItem;

    public void OnDrop(PointerEventData eventData) {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        dropped.transform.SetParent(transform);
        dropped.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        currentItem = dropped;
    }
}