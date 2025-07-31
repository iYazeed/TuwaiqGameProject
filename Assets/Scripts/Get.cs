using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class GardenDoor : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.E;

    [Header("Prompts")]
    public string promptOpenText = "Press E to open the door";

    [Header("Door Swing")]
    [Tooltip("Door opening angle on the Z axis only")]
    public float openAngleZ = 90f;   // زاوية الفتح حول Z
    public float openSpeed = 2f;

    [Header("Audio")]
    public AudioClip openSound;      // صوت فتح الباب

    private bool canInteract = false;
    private bool doorOpened = false;
    private Player playerInv;
    private Quaternion closedRot;
    private Quaternion openRot;
    private AudioSource audioSource;

    void Start()
    {
        // حفظ التدوير الأصلي
        closedRot = transform.localRotation;
        // إعداد تدوير الفتح حول المحور Z
        openRot = closedRot * Quaternion.Euler(0, 0, openAngleZ);

        // تأكد من جعل الكوليدر Trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // إعداد الـAudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!doorOpened && other.CompareTag(playerTag))
        {
            canInteract = true;
            playerInv = other.GetComponent<Player>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            canInteract = false;
            playerInv = null;
        }
    }

    void Update()
    {
        if (canInteract && !doorOpened && playerInv != null && Input.GetKeyDown(interactKey))
        {
            // افتح الباب فورًا بدون شروط
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        doorOpened = true;

        // شغل صوت الفتح
        if (openSound != null)
            audioSource.PlayOneShot(openSound);

        // حركة الفتح حول Z
        float t = 0f;
        while (t < 1f)
        {
            transform.localRotation = Quaternion.Slerp(closedRot, openRot, t);
            t += Time.deltaTime * openSpeed;
            yield return null;
        }
        transform.localRotation = openRot;
    }

    void OnGUI()
    {
        if (canInteract && !doorOpened && playerInv != null)
        {
            // رسالة الفتح دائماً
            var size = GUI.skin.label.CalcSize(new GUIContent(promptOpenText));
            float x = (Screen.width - size.x) * 0.5f;
            float y = (Screen.height - size.y) * 0.8f;
            GUI.Label(new Rect(x, y, size.x, size.y), promptOpenText);
        }
    }
}
