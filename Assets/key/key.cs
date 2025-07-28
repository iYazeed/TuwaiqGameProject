using UnityEngine;

[RequireComponent(typeof(Collider))]
public class KeyPickup : MonoBehaviour
{
    public string playerTag = "Player";        // تأكد إنّ الـPlayer عليه Tag = "Player"
    public KeyCode pickupKey = KeyCode.E;     // الزر اللي يأخذ المفتاح
    [TextArea] public string promptText = "اضغط E لأخذ المفتاح";

    private bool canPickup = false;           // هل اللاعب قريب بما فيه الكفاية؟
    private Player playerInv;        // مرجع لإنفنتوري اللاعب

    void Start()
    {
        // خلي الكوليدر Trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
        // تأكد إنّ الماتيريال أو الميش الحالي ظاهر للـTrigger (Layer & Mask)
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit Trigger by: " + other.name);
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player entered pickup area");
            canPickup = true;
            playerInv = other.GetComponent<Player>();
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // اللاعب خرج من مجال الالتقاط
            canPickup = false;
            playerInv = null;
        }
    }

    void Update()
    {
        if (canPickup && playerInv != null && Input.GetKeyDown(pickupKey))
        {
            // اعطيه المفتاح
            playerInv.hasKey = true;
            // خبّي المفتاح من المشهد
            gameObject.SetActive(false);
        }
    }

    void OnGUI()
    {
        if (canPickup && playerInv != null)
        {
            // نص في منتصف الشاشة
            var size = GUI.skin.label.CalcSize(new GUIContent(promptText));
            float x = (Screen.width - size.x) / 2;
            float y = (Screen.height - size.y) * 0.8f;
            GUI.Label(new Rect(x, y, size.x, size.y), promptText);
        }
    }
}
