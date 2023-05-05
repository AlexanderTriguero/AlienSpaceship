using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigateRute : MonoBehaviour
{
    [SerializeField] Transform route;

    [SerializeField] int currentWayPointIndex = 0;

    NavMeshAgent agent;
    [SerializeField] float reachingDistance=2.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 currentPoint = route.GetChild(currentWayPointIndex).position;
        if (agent!=null && agent.enabled)
        {
            agent.SetDestination(currentPoint);
        }
        if (Vector3.Distance(transform.position, currentPoint) < reachingDistance)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= route.childCount)
            {
                currentWayPointIndex = 0;
            }
        }
    }

    public Transform GetRoute()
    {
        return route;
    }
    public void SetRoute(Transform newRoute)
    {
        route=newRoute;
    }
}
