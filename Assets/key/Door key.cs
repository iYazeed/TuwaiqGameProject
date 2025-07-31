using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class DoorController : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.F;

    [Header("Prompts")]
    public string promptOpenText = "Press F to open the door";
    public string promptNoKeyText = "Find the key";

    [Header("Door Rotation")]
    [Tooltip("Door opening angle on the Y axis only")]
    public float openAngleY = 90f;
    public float openSpeed = 2f;

    [Header("Audio")]
    public AudioClip openSound;       // صوت فتح الباب

    private bool canInteract = false;
    private bool doorOpened = false;
    private Player playerInv;
    private Quaternion closedRot;
    private Quaternion openRot;
    private AudioSource audioSource;

    void Start()
    {
        // Store the original local rotation
        closedRot = transform.localRotation;
        // Prepare the open rotation (Y axis only)
        openRot = closedRot * Quaternion.Euler(0, openAngleY, 0);

        // Ensure the collider is set as Trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // Prepare AudioSource
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
            if (playerInv.hasKey)
                StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        doorOpened = true;

        // 1. تشغيل الصوت كبداية لحركة الباب
        if (openSound != null)
        {
            audioSource.clip = openSound;
            audioSource.loop = false;    // أو true لو تبي يكرر حتى تتوقف يدويًا
            audioSource.Play();
        }

        // 2. حركة فتح الباب
        float t = 0f;
        while (t < 1f)
        {
            transform.localRotation = Quaternion.Slerp(closedRot, openRot, t);
            t += Time.deltaTime * openSpeed;
            yield return null;
        }
        transform.localRotation = openRot;

        // 3. إيقاف الصوت مباشرة بعد انتهاء الحركة
        if (audioSource.isPlaying)
            audioSource.Stop();
    }


    void OnGUI()
    {
        if (canInteract && !doorOpened && playerInv != null)
        {
            // Show the appropriate prompt
            string txt = playerInv.hasKey ? promptOpenText : promptNoKeyText;
            var size = GUI.skin.label.CalcSize(new GUIContent(txt));
            float x = (Screen.width - size.x) * 0.5f;
            float y = (Screen.height - size.y) * 0.8f;
            GUI.Label(new Rect(x, y, size.x, size.y), txt);
        }
    }
}
