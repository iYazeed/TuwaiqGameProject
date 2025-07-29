using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PowerPanel : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerTag = "Player";      // Tag للاعب
    public KeyCode interactKey = KeyCode.E;     // زر التفاعل

    [Header("Power Targets")]
    public Light[] lightsToEnable;   // مصابيح تنفع لما تشغل الكهرباء
    public GameObject[] devicesToPower;   // أجهزة أو جيم أوبجكت تنشط عند توفر الكهرباء

    [Header("Prompts")]
    [TextArea] public string promptOn = "اضغط E لتشغيل الكهرباء";
    [TextArea] public string promptOff = "الكهرباء شغالة";

    bool canInteract = false;
    bool isPowered = false;

    void Start()
    {
        // اضبط الـCollider كـTrigger
        var col = GetComponent<BoxCollider>();
        col.isTrigger = true;

        // اطفئ كل الأضواء والأجهزة بالبداية
        foreach (var l in lightsToEnable) if (l != null) l.enabled = false;
        foreach (var d in devicesToPower) if (d != null) d.SetActive(false);
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
            // شغّل الكهرباء
            isPowered = true;
            foreach (var l in lightsToEnable) if (l != null) l.enabled = true;
            foreach (var d in devicesToPower) if (d != null) d.SetActive(true);
        }
    }

    void OnGUI()
    {
        if (!canInteract) return;

        // عرض الرسالة المناسبة
        string msg = isPowered ? promptOff : promptOn;
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(msg));
        float x = (Screen.width - size.x) * 0.5f;
        float y = (Screen.height - size.y) * 0.8f;
        GUI.Label(new Rect(x, y, size.x, size.y), msg);
    }
}
