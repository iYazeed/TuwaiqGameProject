using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
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

    bool canInteract = false;
    bool doorOpened = false;
    Player playerInv;
    Quaternion closedRot;
    Quaternion openRot;

    void Start()
    {
        // Store the original local rotation
        closedRot = transform.localRotation;
        // Prepare the open rotation (Y axis only)
        openRot = closedRot * Quaternion.Euler(0, openAngleY, 0);

        // Ensure the collider is set as Trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
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
            // Show the appropriate prompt
            string txt = playerInv.hasKey ? promptOpenText : promptNoKeyText;
            var size = GUI.skin.label.CalcSize(new GUIContent(txt));
            float x = (Screen.width - size.x) * 0.5f;
            float y = (Screen.height - size.y) * 0.8f;
            GUI.Label(new Rect(x, y, size.x, size.y), txt);
        }
    }
}
