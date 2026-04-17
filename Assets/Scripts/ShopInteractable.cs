using UnityEngine;

public class ShopInteractable : MonoBehaviour
{
    public MenuController menuController;
    public GameObject interactionPromptPrefab;
    public Transform interactionPromptParent;
    public KeyCode interactKey = KeyCode.E;
    public Vector3 promptOffset = new Vector3(0f, 1.75f, 0f);

    private bool isPlayerInRange = false;
    private TempPlayer playerInRange;
    private GameObject interactionPromptInstance;
    private RectTransform interactionPromptRectTransform;
    private Canvas interactionPromptCanvas;
    private Camera cachedMainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (menuController == null)
        {
            menuController = FindFirstObjectByType<MenuController>();
        }

        cachedMainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerInRange || playerInRange == null)
        {
            return;
        }

        UpdatePromptPosition();

        if (Input.GetKeyDown(interactKey) && menuController != null)
        {
            menuController.ToggleMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TempPlayer player = other.GetComponentInParent<TempPlayer>();
        if (player == null)
        {
            return;
        }

        isPlayerInRange = true;
        playerInRange = player;
        ShowPrompt();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TempPlayer player = other.GetComponentInParent<TempPlayer>();
        if (player == null || player != playerInRange)
        {
            return;
        }

        isPlayerInRange = false;
        playerInRange = null;
        HidePrompt();
    }

    private void OnDisable()
    {
        isPlayerInRange = false;
        playerInRange = null;

        HidePrompt();
    }

    private void ShowPrompt()
    {
        if (interactionPromptPrefab == null || interactionPromptInstance != null)
        {
            return;
        }

        if (interactionPromptParent != null)
        {
            interactionPromptInstance = Instantiate(interactionPromptPrefab, interactionPromptParent);
        }
        else
        {
            interactionPromptInstance = Instantiate(interactionPromptPrefab);
        }

        interactionPromptRectTransform = interactionPromptInstance.GetComponent<RectTransform>();
        interactionPromptCanvas = interactionPromptInstance.GetComponentInParent<Canvas>();
        UpdatePromptPosition();
    }

    private void HidePrompt()
    {
        if (interactionPromptInstance != null)
        {
            Destroy(interactionPromptInstance);
            interactionPromptInstance = null;
        }

        interactionPromptRectTransform = null;
        interactionPromptCanvas = null;
    }

    private void UpdatePromptPosition()
    {
        if (interactionPromptInstance == null || playerInRange == null)
        {
            return;
        }

        if (cachedMainCamera == null)
        {
            cachedMainCamera = Camera.main;
            if (cachedMainCamera == null)
            {
                return;
            }
        }

        Vector3 targetPosition = playerInRange.transform.position + promptOffset;

        if (interactionPromptCanvas != null && interactionPromptCanvas.renderMode != RenderMode.WorldSpace && interactionPromptRectTransform != null)
        {
            interactionPromptRectTransform.position = cachedMainCamera.WorldToScreenPoint(targetPosition);
            return;
        }

        interactionPromptInstance.transform.position = targetPosition;
    }
}
