using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class Gas : MonoBehaviour
{
    public string playerTa = "Player";              // تأكد إنّ الـPlayer عليه Tag = "Player"
    public KeyCode pickupKe = KeyCode.F;             // زر الالتقاط
    [TextArea] public string promptTex = "Press F to pick up the gas can";
    public AudioClip pickupSound;                      // صوت جمع الغاز

    private bool canPicku = false;                     // هل اللاعب قريب؟
    private Player playerIn;                           // مرجع سكربت Player
    private AudioSource audioSource;
    private MeshRenderer[] renderers;                  // كل الـMeshRenderers تحت هذا الكائن
    private Collider myCollider;

    void Start()
    {
        // 1. تحضير الكوليدر كـ Trigger
        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;

        // 2. تحضير الـAudioSource
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // 3. جمع كل MeshRenderers لإخفائها لاحقاً
        renderers = GetComponentsInChildren<MeshRenderer>();
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
            // منح الغاز
            playerIn.hasGas = true;

            // تشغيل الصوت فورًا
            if (pickupSound != null)
                audioSource.PlayOneShot(pickupSound);

            // إخفاء الموديل (MeshRenderers) والـCollider
            foreach (var r in renderers)
                r.enabled = false;
            myCollider.enabled = false;

            // تدمير الكائن بعد انتهاء الصوت
            float delay = pickupSound != null ? pickupSound.length : 0f;
            Destroy(gameObject, delay);
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
