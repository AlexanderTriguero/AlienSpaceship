using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstPersonCamera : MonoBehaviour
{

    [SerializeField][Range(-180f,180f)] float mouseSensitivityY = -30f;
    [SerializeField] float maxLookAngle = 60f;
    [SerializeField] PlayerWithLife playerWithLife;

    void Awake()
    {
        //oldMousePositionY = Input.mousePosition.y;
    }

    void Update()

    {
        if (playerWithLife.IsAlive())
        {
            if (!UIPausa.pausado)
            {
                float mouseDelta = Input.GetAxis("Mouse Y");

                if (mouseDelta != 0f)

                {

                    float mouseSpeed = (mouseDelta / Screen.height) / Time.deltaTime;  // <---- THIS



                    Vector3 forwardOnPlane = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

                    float angle = Vector3.SignedAngle(forwardOnPlane, transform.forward, transform.right);

                    float angleToApply = mouseSpeed * mouseSensitivityY;



                    if ((angle + angleToApply) > maxLookAngle)

                    { angleToApply += (maxLookAngle - (angle + angleToApply)); }

                    else if ((angle + angleToApply) < -maxLookAngle)

                    { angleToApply += (-maxLookAngle - (angle + angleToApply)); }



                    Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, transform.right);

                    transform.rotation = rotationToApply * transform.rotation;

                }
            }
        }
    }
}
