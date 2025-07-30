using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PowerPanel : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.E;

    [Header("Power Targets")]
    public Light[] lightsToEnable;
    public GameObject[] devicesToPower;

    [Header("UI Hint Message")]
    public GameObject uiMessage; // 🔔 الرسالة اللي تطلع تحت الشاشة

    [Header("Prompts")]
    [TextArea] public string promptOn = "اضغط E لتشغيل الكهرباء";
    [TextArea] public string promptOff = "الكهرباء شغالة";

    public bool canInteract = false;
    public bool isPowered = false;

    void Start()
    {
        // ضبط الكوليدر
        var col = GetComponent<BoxCollider>();
        col.isTrigger = true;

        // إطفاء الأشياء
        foreach (var l in lightsToEnable) if (l != null) l.enabled = false;
        foreach (var d in devicesToPower) if (d != null) d.SetActive(false);

        // تفعيل الرسالة في البداية
        if (uiMessage != null)
            uiMessage.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            canInteract = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
            canInteract = false;
    }

    void Update()
    {
        if (canInteract && !isPowered && Input.GetKeyDown(interactKey))
        {
            // شغل الكهرباء
            isPowered = true;
            foreach (var l in lightsToEnable) if (l != null) l.enabled = true;
            foreach (var d in devicesToPower) if (d != null) d.SetActive(true);

            // إخفاء الرسالة بعد التشغيل
            if (uiMessage != null)
                uiMessage.SetActive(false);
        }
    }

    void OnGUI()
    {
        if (!canInteract) return;

        string msg = isPowered ? promptOff : promptOn;
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(msg));
        float x = (Screen.width - size.x) * 0.5f;
        float y = (Screen.height - size.y) * 0.8f;
        GUI.Label(new Rect(x, y, size.x, size.y), msg);
    }
}
