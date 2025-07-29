using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
        if (player == null)
            Debug.LogError("InventoryDisplay: Player component not found on this GameObject!");
    }

    void OnGUI()
    {
        if (player == null) return;

        // Starting position for the labels
        float x = 10f;
        float y = Screen.height - 150f;
        float width = 250f;
        float height = 25f;

        // Style for the labels
        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            normal = { textColor = Color.white }
        };

        // Header
        GUI.Label(new Rect(x, y, width, height), "Inventory:", style);
        y += height;

        // Always show all items, with check if player has it
        GUI.Label(new Rect(x, y, width, height), (player.hasKey ? "✔ " : "✖ ") + "House Key", style);
        y += height;

        GUI.Label(new Rect(x, y, width, height), (player.hasGas ? "✔ " : "✖ ") + "Gas Can", style);
        y += height;

        GUI.Label(new Rect(x, y, width, height), (player.hasCarKey ? "✔ " : "✖ ") + "Car Key", style);
        y += height;

        GUI.Label(new Rect(x, y, width, height), (player.hasGardenKey ? "✔ " : "✖ ") + "Gate Key", style);
        y += height;
    }
}