using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
public class NavigateToPosition : MonoBehaviour
{ 
    [SerializeField] public Vector3 position;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null && agent.enabled)
        {
            agent.SetDestination(position);
        }
    }
}
