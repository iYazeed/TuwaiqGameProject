using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public float attackRange = 2f;
    public int damageAmount = 10;

    private bool canAttack = true;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && canAttack)
        {
            animator.SetTrigger("Attack"); // تشغيل الأنميشن
            canAttack = false;

            // ⏱️ تطبيق الضرر بعد تأخير مناسب (1.0 ثانية للتماشي مع الأنميشن)
            Invoke(nameof(ApplyDamage), 1.2f);

            // إعادة السماح بالهجوم بعد وقت مناسب
            Invoke(nameof(ResetAttack), 6f);
        }
    }

    void ApplyDamage()
    {
        player.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount);
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}