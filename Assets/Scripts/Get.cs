using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class GardenDoor : MonoBehaviour
{
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.E;

    [Header("Door Swing")]
    public float openAngleZ = 90f;
    public float openSpeed = 2f;

    [Header("Prompts")]
    [TextArea] public string promptNeedKey = "You need the garden key.";
    [TextArea] public string promptOpenDoor = "Press E to open the garden door.";

    bool canInteract = false;
    bool isOpen = false;
    Player playerInv;
    Quaternion closedRot;
    Quaternion openRot;

    void Start()
    {
        closedRot = transform.localRotation;
        openRot = closedRot * Quaternion.Euler(0, 0, openAngleZ);
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
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
        if (!canInteract || isOpen || playerInv == null) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (playerInv.hasGardenKey)
                StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        isOpen = true;
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
        if (!canInteract || playerInv == null || isOpen) return;

        string msg = playerInv.hasGardenKey ? promptOpenDoor : promptNeedKey;
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(msg));
        float x = (Screen.width - size.x) * .5f;
        float y = (Screen.height - size.y) * .8f;
        GUI.Label(new Rect(x, y, size.x, size.y), msg);
    }
}
