using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyPicku : MonoBehaviour
{
    public string playerTa = "Player";             // تأكد التاق مضبوط
    public KeyCode pickupKe = KeyCode.E;            // زر الالتقاط
    [TextArea] public string promptTex = "Press E to pick up the key";
    public AudioClip pickupSound;                     // صوت التقاط المفتاح

    private bool canPicku = false;                    // هل اللاعب قريب؟
    private Player playerIn;                          // مرجع سكربت Player

    void Start()
    {
        // اضبط الكوليدر كـ Trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTa))
        {
            canPicku = true;
            playerIn = other.GetComponent<Player>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTa))
        {
            canPicku = false;
            playerIn = null;
        }
    }

    void Update()
    {
        if (canPicku && playerIn != null && Input.GetKeyDown(pickupKe))
        {
            // 1. منح المفتاح
            playerIn.hasCarKey = true;

            // 2. شغّل الصوت فوراً في موقع المفتاح
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // 3. احذف الكائن مباشرة (المجسم يختفي)
            Destroy(gameObject);
        }
    }

    void OnGUI()
    {
        if (canPicku && playerIn != null)
        {
            var size = GUI.skin.label.CalcSize(new GUIContent(promptTex));
            float x = (Screen.width - size.x) * 0.5f;
            float y = (Screen.height - size.y) * 0.8f;
            GUI.Label(new Rect(x, y, size.x, size.y), promptTex);
        }
    }
}
