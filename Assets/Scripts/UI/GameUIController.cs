using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TempPlayer player;
    [SerializeField] private Image equippedToolImage;
    [SerializeField] private Image toolDurabilityFillImage;

    private ToolBase cachedTool;

    void Update()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<TempPlayer>();
        }

        ToolBase currentTool = player != null ? player.equippedTool : null;

        if (currentTool != cachedTool)
        {
            UpdateToolIcon(currentTool);
            cachedTool = currentTool;
        }

        UpdateToolDurability(currentTool);
    }

    private void UpdateToolIcon(ToolBase currentTool)
    {
        if (equippedToolImage == null)
        {
            return;
        }

        if (currentTool == null || currentTool.toolSprite == null || currentTool.toolSprite.sprite == null)
        {
            equippedToolImage.sprite = null;
            equippedToolImage.enabled = false;
            return;
        }

        equippedToolImage.sprite = currentTool.toolSprite.sprite;
        equippedToolImage.enabled = true;
    }

    private void UpdateToolDurability(ToolBase currentTool)
    {
        if (toolDurabilityFillImage == null)
        {
            return;
        }

        if (currentTool == null)
        {
            toolDurabilityFillImage.fillAmount = 0f;
            return;
        }

        int maxDurability = Mathf.Max(1, currentTool.maxToolDurability);
        float durabilityPercent = Mathf.Clamp01((float)currentTool.toolDurability / maxDurability);
        toolDurabilityFillImage.fillAmount = durabilityPercent;
    }
}
