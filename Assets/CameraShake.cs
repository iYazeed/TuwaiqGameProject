using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FootstepCameraShake : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;          // اربط هنا كاميرا اللاعب

    [Header("Footstep Settings")]
    public AudioClip outdoorFootstepSound;
    public AudioClip indoorFootstepSound;
    public float stepInterval = 0.5f;   // الوقت بين كل خطوة
    public float moveThreshold = 0.1f;   // الحد الأدنى للحركة لاعتبارها "مشي"

    [Header("Shake Settings")]
    public float shakeDuration = 0.1f;   // مدة اهتزاز الكاميرا لكل خطوة
    public float shakeMagnitude = 0.02f;  // قوة الاهتزاز

    private CharacterController cc;
    private AudioSource audioSource;
    private float stepTimer;
    private bool isIndoor;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        stepTimer = 0f;
    }

    void Update()
    {
        // قراءة مُدخلات الحركة
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (cc.isGrounded && input.magnitude > moveThreshold)
        {
            // ناقص من عداد الخطوات
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                // 1) تشغيل صوت الخطوة المناسب
                AudioClip clip = (isIndoor && indoorFootstepSound != null)
                                ? indoorFootstepSound
                                : outdoorFootstepSound;
                if (clip != null)
                    audioSource.PlayOneShot(clip);

                // 2) إطلاق اهتزاز الكاميرا
                StartCoroutine(Shake(shakeDuration, shakeMagnitude));

                // 3) إعادة ضبط عداد الخطوات
                stepTimer = stepInterval;
            }
        }
        else
        {
            // لو توقف المشي، أعد العدّاد ليستعد للمرحلة التالية
            stepTimer = 0f;
        }
    }

    // دخول/خروج اللاعب لمنطقة Indoor
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Indoor"))
            isIndoor = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Indoor"))
            isIndoor = false;
    }

    // الكوروتين الخاص بالاهتزاز
    private IEnumerator Shake(float duration, float magnitude)
    {
        if (playerCamera == null) yield break;

        Vector3 originalPos = playerCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            playerCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            yield return null;
        }

        playerCamera.transform.localPosition = originalPos;
    }
}
