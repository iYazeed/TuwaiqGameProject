using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Gas : MonoBehaviour
{
    public string playerTa = "Player";        // تأكد إنّ الـPlayer عليه Tag = "Player"
    public KeyCode pickupKe = KeyCode.F;     // الزر اللي يأخذ المفتاح
    [TextArea] public string promptTex = "اضغط F لأخذ المفتاح";

    private bool canPicku = false;           // هل اللاعب قريب بما فيه الكفاية؟
    private Player playerIn;        // مرجع لإنفنتوري اللاعب

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
        if (other.CompareTag(playerTa))
        {
            Debug.Log("Player entered pickup area");
            canPicku = true;
            playerIn = other.GetComponent<Player>();
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTa))
        {
            // اللاعب خرج من مجال الالتقاط
            canPicku = false;
            playerIn = null;
        }
    }

    void Update()
    {
        if (canPicku && playerIn != null && Input.GetKeyDown(pickupKe))
        {
            // اعطيه المفتاح
            playerIn.hasGas = true;
            // خبّي المفتاح من المشهد
            gameObject.SetActive(false);
        }
    }

    void OnGUI()
    {
        if (canPicku && playerIn != null)
        {
            // نص في منتصف الشاشة
            var size = GUI.skin.label.CalcSize(new GUIContent(promptTex));
            float x = (Screen.width - size.x) / 2;
            float y = (Screen.height - size.y) * 0.8f;
            GUI.Label(new Rect(x, y, size.x, size.y), promptTex);
        }
    }
}
