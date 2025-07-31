using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Getkey : MonoBehaviour
{
    public string playerTa = "Player";       // تأكد إنّ التاق مضبوط
    public KeyCode pickupKe = KeyCode.F;      // زر الالتقاط
    [TextArea] public string promptTex = "اضغط F لأخذ المفتاح";
    public AudioClip pickupSound;               // صوت التقاط المفتاح

    private bool canPicku = false;              // هل اللاعب قريب؟
    private Player playerIn;                    // مرجع سكربت Player
    private Collider myCollider;

    void Start()
    {
        // تحضير الكوليدر كـ Trigger
        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;
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
            // 1. منحه المفتاح
            playerIn.hasGardenKey = true;

            // 2. شغل الصوت فورًا في موقع المفتاح
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // 3. اختفِ المفتاح فورًا
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
