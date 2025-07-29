using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;         // ãæŞÚ ÇááÇÚÈ
    public float speed = 5f;         // ÓÑÚÉ ÇáãØÇÑÏÉ
    public float stoppingDistance = 2f; // ÇáãÓÇİÉ Çááí íÊæŞİ İíåÇ ÇáÚÏæ

    void Update()
    {
        // ÇÍÓÈ ÇáãÓÇİÉ Èíä ÇáÚÏæ æÇááÇÚÈ
        float distance = Vector3.Distance(transform.position, player.position);

        // ÅĞÇ ßÇä ÇáÚÏæ ÈÚíÏ ÈãÇ İíå ÇáßİÇíÉ¡ íÊÍÑß äÍæå
        if (distance > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // ÇÎÊíÇÑí: æÌå ÇáÚÏæ äÍæ ÇááÇÚÈ
            transform.LookAt(player);
        }
    }
}
