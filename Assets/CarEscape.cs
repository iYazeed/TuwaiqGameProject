using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class CarEscape : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public Camera playerCamera;
    public GameObject enemy;
    public Camera carCamera;
    public GameObject carObject;

    [Header("Player Requirements")]
    public bool requireCarKey = true;
    public bool requireGas    = true;

    [Header("UI")]
    public CanvasGroup blackScreen;
    public GameObject powerHintUI; // ← اسحب هنا UI_PowerHint

    [Header("Timing & Movement")]
    public float triggerDistance = 3f;
    public float carSpeed        = 5f;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip musicClip;

    [Header("End Settings")]
    public string menuSceneName;

    private bool triggered = false;
    private Player playerScript;
    private AudioSource _internalAudio;

    void Start()
    {
        if (carCamera   != null) carCamera.gameObject.SetActive(false);
        if (blackScreen != null) blackScreen.alpha = 0;
        if (player      != null) playerScript = player.GetComponent<Player>();

        _internalAudio = GetComponent<AudioSource>();
        _internalAudio.playOnAwake = false;
        _internalAudio.loop      = false;
    }

    void Update()
    {
        if (triggered || playerScript == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        bool hasKey = playerScript.hasCarKey;
        bool hasGas = playerScript.hasGas;

        if (distance < triggerDistance &&
            (!requireCarKey || hasKey) &&
            (!requireGas    || hasGas))
        {
            StartCoroutine(EscapeSequence());
        }
    }

    IEnumerator EscapeSequence()
    {
        triggered = true;

        // أخفي رسالة الــ Power Hint فور البداية
        if (powerHintUI != null)
            powerHintUI.SetActive(false);

        // 1. Fade to black
        if (blackScreen != null)
        {
            float fadeInTime = 1.5f, t = 0f;
            while (t < fadeInTime)
            {
                t += Time.deltaTime;
                blackScreen.alpha = Mathf.Lerp(0, 1, t / fadeInTime);
                yield return null;
            }
        }

        // 2. Hide player/enemy and switch cameras
        if (player      != null) player.SetActive(false);
        if (playerCamera!= null) playerCamera.gameObject.SetActive(false);
        if (enemy       != null) enemy.SetActive(false);
        if (carCamera   != null) carCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        // 3. Fade out & drive
        float fadeOutTime = 1.5f, timer = 0f;
        while (timer < fadeOutTime)
        {
            if (blackScreen != null)
                blackScreen.alpha = Mathf.Lerp(1, 0, timer / fadeOutTime);

            if (carObject != null)
                carObject.transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        // 4. Play music
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }

        // 5. Continue driving
        float moveTime = 20f, t1 = 0f;
        while (t1 < moveTime)
        {
            if (carObject != null)
                carObject.transform.Translate(Vector3.forward * carSpeed * Time.deltaTime);

            t1 += Time.deltaTime;
            yield return null;
        }

        // 6. Fade to black again
        if (blackScreen != null)
        {
            float fadeIn2 = 1.5f, t2 = 0f;
            while (t2 < fadeIn2)
            {
                t2 += Time.deltaTime;
                blackScreen.alpha = Mathf.Lerp(0, 1, t2 / fadeIn2);
                yield return null;
            }
        }

        // 7. Unlock cursor & load menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
        if (!string.IsNullOrEmpty(menuSceneName))
            SceneManager.LoadScene(menuSceneName);
        else
            Debug.LogError("CarEscape: menuSceneName is empty! Set it in the Inspector.");
    }
}
