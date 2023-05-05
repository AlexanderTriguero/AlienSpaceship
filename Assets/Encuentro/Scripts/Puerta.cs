using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    Animator animator;
    MeshCollider meshCollider;
    BoxCollider boxCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    public void CloseDoor()
    {
        animator.SetBool("doorOpened",false);

    }

    public void OpenDoor()
    {
        animator.SetBool("doorOpened", true);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            OpenDoor();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            CloseDoor();
        }
    }

}
