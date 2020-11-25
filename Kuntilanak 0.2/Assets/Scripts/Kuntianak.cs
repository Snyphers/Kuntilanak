using UnityEngine.AI;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    [SerializeField] Transform Destination;
    [SerializeField] NavMeshAgent NavMeshAgent;

    private void Start()
    {
        NavMeshAgent = this.GetComponent<NavMeshAgent>();

        if (NavMeshAgent == null)
        {
            Debug.Log("HMMM?" + gameObject.name);
        }
        else
        {
            //SetDestination();            
        }
    }

    public void Update()
    {
        if (Destination != null)
        {
            Vector3 TargetVector = Destination.transform.position;
            NavMeshAgent.SetDestination(TargetVector);
        }
    }
}