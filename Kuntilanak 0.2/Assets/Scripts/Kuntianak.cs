using UnityEngine.AI;
using UnityEngine;

public class Kuntianak : MonoBehaviour
{
    [SerializeField] Transform Destination;
    [SerializeField] NavMeshAgent NavMeshAgent;

    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

        GameObject Character = GameObject.FindWithTag("Player");
        Destination = Character.transform;

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