using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigateToTransform: MonoBehaviour
{
    //sirve para hacer publico sin ques ea publico, es para que aparezca en unity para colocarlo
    public Transform transformGoTo;
    NavMeshAgent navMeshAgent;


    void Awake(){
        //cachear
        navMeshAgent   = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            if (transformGoTo)
            {
                navMeshAgent.SetDestination(transformGoTo.position);
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
            }
        }
        
    }
}
