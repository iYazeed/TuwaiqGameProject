using UnityEngine;
using System.Collections;

public class CarEscape : MonoBehaviour
{
    public GameObject player;
    public Camera playerCamera;
    public GameObject enemy;
    public Camera carCamera;
    public GameObject carObject;
    public float carSpeed = 5f;
    public float triggerDistance = 3f;
    public CanvasGroup blackScreen;

    private bool triggered = false;
    private Player playerScript;

    void Start()
    {
        if (carCamera != null)
            carCamera.gameObject.SetActive(false);

        if (blackScreen != null)
            blackScreen.alpha = 0;

        if (player != null)
            playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        if (triggered || player == null || playerScript == null)
            return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        // ✅ التحقق من المسافة ومن الأغراض المطلوبة
        if (distance < triggerDistance && playerScript.hasCarKey && playerScript.hasGas)
        {
            StartCoroutine(EscapeSequence());
        }
    }

    IEnumerator EscapeSequence()
    {
        triggered = true;

        // 1. شاشة سوداء تدريجي من كاميرا اللاعب
        if (blackScreen != null)
        {
            float fadeInTime = 1.5f;
            float t = 0;
            while (t < fadeInTime)
            {
                t += Time.deltaTime;
                blackScreen.alpha = Mathf.Lerp(0, 1, t / fadeInTime);
                yield return null;
            }
        }

        // 2. بعد السواد الكامل: إخفاء اللاعب وتبديل الكاميرات
        if (player != null)
            player.SetActive(false);

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);

        if (carCamera != null)
            carCamera.gameObject.SetActive(true);

        if (enemy != null)
            enemy.SetActive(false);

        yield return new WaitForSeconds(1f); // ثبات على السواد

        // 3. شاشة سوداء تبدأ تختفي تدريجي + السيارة تبدأ تمشي
        float fadeOutTime = 1.5f;
        float timer = 0f;

        while (timer < fadeOutTime)
        {
            if (blackScreen != null)
                blackScreen.alpha = Mathf.Lerp(1, 0, timer / fadeOutTime);

            if (carObject != null)
                carObject.transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        // 4. استمرار حركة السيارة بعد اختفاء الشاشة السوداء
        float moveTime = 8f;
        float t2 = 0f;

        while (t2 < moveTime)
        {
            if (carObject != null)
                carObject.transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);

            t2 += Time.deltaTime;
            yield return null;
        }

        Debug.Log("🚗 النهاية السينمائية تمت بنجاح");
    }
}
