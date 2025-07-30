using UnityEngine;

public class PowerMessageController : MonoBehaviour
{
    public GameObject uiMessage;           // UI النص اللي راح يظهر
    public PowerPanel powerManager;      // سكربت الكهرباء اللي فيه isPowerOn

    void Start()
    {
        // إظهار الرسالة عند بداية اللعبة
        if (uiMessage != null)
            uiMessage.SetActive(true);
    }

    void Update()
    {
        // إخفاء الرسالة إذا اشتغل الكهرباء
        if (powerManager != null && powerManager.isPowered && uiMessage.activeSelf)
        {
            uiMessage.SetActive(false);
        }
    }
}
