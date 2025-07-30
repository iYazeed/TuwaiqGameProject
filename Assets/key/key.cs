using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class KeyPickup : MonoBehaviour
{
    public string playerTag = "Player";        // تأكد إنّ الـPlayer عليه Tag = "Player"
    public KeyCode pickupKey = KeyCode.E;      // الزر اللي يأخذ المفتاح

    [TextArea]
    public string promptText = "Press E to pick up the key";

    public AudioClip pickupSound;             // صوت التقاط المفتاح

    private bool canPickup = false;           // هل اللاعب قريب بما فيه الكفاية؟
    private Player playerInv;                 // مرجع لإنفنتوري اللاعب
    private AudioSource audioSource;

    void Start()
    {
        // خلي الكوليدر Trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // جهّز الـAudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            canPickup = true;
            playerInv = other.GetComponent<Player>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
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

            // شغّل صوت التقاط المفتاح
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound);

            // خبّي المفتاح من المشهد بعد قليل (حتى يسمع الصوت كامل)
            Destroy(gameObject, pickupSound != null ? pickupSound.length : 0f);
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
