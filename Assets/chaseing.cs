using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public Transform player;         // ãæÞÚ ÇááÇÚÈ
    public float speed = 5f;         // ÓÑÚÉ ÇáãØÇÑÏÉ
    public float stoppingDistance = 2f; // ÇáãÓÇÝÉ Çááí íÊæÞÝ ÝíåÇ ÇáÚÏæ
    public NavMeshAgent agent;
    void Update()
    {
        // ÇÍÓÈ ÇáãÓÇÝÉ Èíä ÇáÚÏæ æÇááÇÚÈ
        float distance = Vector3.Distance(transform.position, player.position);

        // ÅÐÇ ßÇä ÇáÚÏæ ÈÚíÏ ÈãÇ Ýíå ÇáßÝÇíÉ¡ íÊÍÑß äÍæå
        if (distance > stoppingDistance)
        {
            agent.SetDestination(player.position);
           

        }
    }
}
