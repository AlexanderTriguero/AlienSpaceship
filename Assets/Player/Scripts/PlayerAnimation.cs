using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    Animator animator;
    Vector3 oldPosition = Vector3.zero;
    float smoothFactor = 20f;
    // Start is called before the first frame update
    void Awake()
    {
        oldPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
    }

    Vector3 smoothedVelocity = Vector3.zero;
    void Update()
    {
        Vector3 currentWorldVelocity = (transform.position - oldPosition) / Time.deltaTime;
        Vector3 currentLocalVelocity = transform.InverseTransformDirection(currentWorldVelocity);

        smoothedVelocity += (currentLocalVelocity - smoothedVelocity).normalized * smoothFactor * Time.deltaTime;
        if (smoothedVelocity.magnitude > 0.5f)
        {
            animator.SetFloat("ForwardVelocity", smoothedVelocity.z);
            animator.SetFloat("HorizontalVelocity", smoothedVelocity.x);
        }
        else
        {
            animator.SetFloat("ForwardVelocity", 0f);
            animator.SetFloat("HorizontalVelocity", 0f);
        }
        oldPosition = transform.position;
    }
}
