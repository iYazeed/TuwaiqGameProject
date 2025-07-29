using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform player;             // اللاعب
    public Animator animator;            // Animator العدو
    public float attackRange = 2f;       // مدى الهجوم
    public int damageAmount = 10;        // مقدار الضرر

    private bool canAttack = true;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && canAttack)
        {
            // 🔥 تفعيل أنميشن الهجوم
            animator.SetTrigger("Attack");

            // ⚔️ تطبيق الضرر (يمكنك استبداله بـ Animation Event لو حبيت توقيت أدق)
            player.GetComponent<PlayerHealth>().TakeDamage(damageAmount);

            // ⏱️ منع الهجوم لمدة قصيرة لتجنب التكرار السريع
            canAttack = false;
            Invoke(nameof(ResetAttack), 3f); // عدّل الوقت حسب نوع الهجوم
        }
    }

    void ResetAttack()
    {
        canAttack = true;
    }
}