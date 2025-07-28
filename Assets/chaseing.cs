using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Tooltip("Drag your Player Transform here, or leave blank to auto-find by the 'Player' tag.")]
    [SerializeField] private Transform player;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject found = GameObject.FindGameObjectWithTag("Player");
            if (found != null)
            {
                player = found.transform;
            }
            else
            {
                Debug.LogError($"[{nameof(EnemyChase)}] No Player found! Make sure your Player GameObject is tagged 'Player' or assign it in the Inspector.", this);
            }
        }
    }

    void Update()
    {
        if (agent != null && player != null)
        {
            agent.SetDestination(player.position);
        }
    }
}