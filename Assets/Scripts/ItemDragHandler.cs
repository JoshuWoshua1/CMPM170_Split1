using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; //Save OG parent
        transform.SetParent(transform.root); //Above other canvas'
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; //Semi-transparent during drag
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //Follow the mouse
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // if OnDrop on ShopSlot didn't fire, return home
        if (transform.parent.GetComponent<ShopSlot>() == null)
        {
            transform.SetParent(originalParent);
            // GetComponent<RectTransform>().anchoredPosition =  new Vector3(50f,-50f, 1f);
        }
          GetComponent<RectTransform>().anchoredPosition =  new Vector3(50f,-50f, 1f);
    }
}
