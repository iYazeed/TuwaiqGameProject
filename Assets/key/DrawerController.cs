using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class DrawerControllerZ : MonoBehaviour
{
    [Header("السماح بالتفاعل")]
    public KeyCode interactKey = KeyCode.E;
    [TextArea] public string promptOpen = "اضغط E لفتح الدرج";
    [TextArea] public string promptClose = "اضغط E لإغلاق الدرج";

    [Header("إعدادات الحركة على Z")]
    public Transform drawer;      // الفائدة: لو الميش داخل Pivot آخر
    public float openZ = 0.2f;   // المسافة على Z لفتح الدرج
    public float speed = 2f;     // سرعة الحركة

    bool canInteract = false;
    bool isOpen = false;
    Player playerInv;

    Vector3 closedPos;
    Vector3 openPos;

    void Start()
    {
        if (drawer == null) drawer = transform;
        closedPos = drawer.localPosition;
        openPos = closedPos + new Vector3(0, 0, openZ);

        var col = GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            playerInv = other.GetComponent<Player>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            playerInv = null;
        }
    }

    void Update()
    {
        if (!canInteract || playerInv == null) return;

        if (Input.GetKeyDown(interactKey))
        {
            // إذا مفتوح اقفله، وإلا افتحه
            StartCoroutine(MoveDrawer(isOpen ? closedPos : openPos));
            isOpen = !isOpen;
        }
    }

    IEnumerator MoveDrawer(Vector3 target)
    {
        Vector3 start = drawer.localPosition;
        float t = 0f;
        while (t < 1f)
        {
            drawer.localPosition = Vector3.Lerp(start, target, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        drawer.localPosition = target;
    }

    void OnGUI()
    {
        if (!canInteract) return;

        string txt = isOpen ? promptClose : promptOpen;
        var sz = GUI.skin.label.CalcSize(new GUIContent(txt));
        GUI.Label(new Rect((Screen.width - sz.x) / 2, Screen.height * 0.8f, sz.x, sz.y), txt);
    }
}
