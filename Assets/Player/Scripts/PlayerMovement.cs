using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //static PlayerMovement theOnlyInstanceOfPlayerMovementInThisSceneBecauseISaySo;
    public static PlayerMovement instance;

    [SerializeField] float speed = 4f;
    [SerializeField] LayerMask layermaskAimingDetection;

    [Header("First Person Rotation")]
    [SerializeField] bool rotateWithMouse = false;
    [SerializeField] float mouseSensitivityX = 0.005f;



    CharacterController characterController;
    PlayerWithLife playerWithLife;


    float oldMousePositionX;

    private void Awake()
    {
        instance = this;

        characterController = GetComponent<CharacterController>();

        oldMousePositionX = Input.mousePosition.x;
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerWithLife = GetComponent<PlayerWithLife>();

    }

    float speedY = 0f;
    private float gravity = -9.8f;

    private void Update()
    {
        if (playerWithLife.IsAlive()) { 
            if (!UIPausa.pausado)
            {
                UpdateMovement();
                //UpdateAnimation();
                UpdateOrientation();
            }

        }
    }


    Vector3 movementFromInput;
    Vector3 movementFromCamera;
    private void UpdateMovement()
    {
        movementFromInput = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) { movementFromInput += Vector3.left; }
        if (Input.GetKey(KeyCode.W)) { movementFromInput += Vector3.forward; }
        if (Input.GetKey(KeyCode.S)) { movementFromInput += Vector3.back; }
        if (Input.GetKey(KeyCode.D)) { movementFromInput += Vector3.right; }

        movementFromCamera = Camera.main.transform.TransformDirection(movementFromInput);
        movementFromCamera = Vector3.ProjectOnPlane(movementFromCamera, Vector3.up);
        movementFromCamera.Normalize();

        speedY += gravity * Time.deltaTime;
        movementFromCamera.y = speedY;
        characterController.Move(movementFromCamera * speed * Time.deltaTime);

        if (characterController.isGrounded) { speedY = 0f; }
    }

    //private void UpdateAnimation()
    //{
    //    Vector3 localMovement = transform.InverseTransformDirection(movementFromCamera);
    //    animator.SetFloat("ForwardVelocity", localMovement.z);
    //    animator.SetFloat("HorizontalVelocity", localMovement.x);
    //}

    [SerializeField] bool orientateToCamera;

    private void UpdateOrientation()
    {
        if (rotateWithMouse)
        { UpdateOrientationWithMouse(); }
        else if (orientateToCamera)
        { UpdateOrientateToCamera(); }
        else
        { UpdateOrientateToMouse(); }
    }

    private void UpdateOrientateToMouse()
    {
        Vector3 desiredForward = Vector3.zero;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermaskAimingDetection))
        {
            desiredForward = hit.point - transform.position;
            PerformOrientation(desiredForward);
        }
    }

    private void UpdateOrientateToCamera()
    {
        Vector3 desiredForward = Vector3.zero;

        if (movementFromInput.sqrMagnitude > (0.01f * 0.01f))
        {
            desiredForward = Camera.main.transform.forward;
            PerformOrientation(desiredForward);
        }
    }

    private void UpdateOrientationWithMouse()
    {
        float mouseDelta = Input.GetAxis("Mouse X");
        if (Time.deltaTime !=0)
        {
            float mouseSpeed = (mouseDelta / Screen.width) / Time.deltaTime;    // <---- THIS

            Quaternion rotationToApply = Quaternion.AngleAxis(mouseSpeed * mouseSensitivityX, Vector3.up);
            transform.rotation = rotationToApply * transform.rotation;

            oldMousePositionX = Input.mousePosition.x;
        }
    }

    void PerformOrientation(Vector3 desiredForward)
    {
        desiredForward = Vector3.ProjectOnPlane(desiredForward, Vector3.up);
        desiredForward.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(desiredForward, Vector3.up);
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.04f);
    }
}
