using UnityEngine;
using UnityEngine.AI;

public class chaseing : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform Player;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }
   
    // Update is called once per frame
    void Update()
    {
        if (Player != null) { 
            agent.SetDestination(Player.position);
        }
    }
}
