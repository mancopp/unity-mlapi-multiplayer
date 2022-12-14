using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : NetworkBehaviour
{
    /// <summary>
    /// Move the player charactercontroller based on horizontal and vertical axis input
    /// </summary>
    [SerializeField]
    float yVelocity = 0f;
    [Range(5f,25f)]
    public float gravity = 15f;
    //the speed of the player movement
    [Range(5f,15f)]
    public float movementSpeed = 10f;
    //jump speed
    [Range(5f,15f)]
    public float jumpSpeed = 5f;

    //now the camera so we can move it up and down
    Transform cameraTransform;
    float pitch = 0f;
    [Range(1f,90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;

    CharacterController cc;

    private void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        if(IsLocalPlayer)
        {
            cc = GetComponent<CharacterController>();   
        }
        else
        {
            cameraTransform.GetComponent<Camera>().enabled = false;
            cameraTransform.GetComponent<AudioListener>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsLocalPlayer && cc is not null)
        {
            Look();
            Move();
        }
    }

    void Look()
    {
        float xInput = Input.GetAxis("Mouse X") * 50f;
        float yInput = Input.GetAxis("Mouse Y") * 50f;
        transform.Rotate(0, xInput, 0);

        pitch -= yInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        Quaternion rot = Quaternion.Euler(pitch, 0, 0);
        cameraTransform.localRotation = rot;
    }

    void Move()
    {
        //update speed based onn the input
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        //transofrm it based off the player transform and scale it by movement speed
        Vector3 move = transform.TransformVector(input) * movementSpeed;
        //is it on the ground
        if (cc.isGrounded)
        {
            yVelocity = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
            }
        }
        //now add the gravity to the yvelocity
        yVelocity -= gravity * Time.deltaTime;
        move.y = yVelocity;
        //and finally move
        cc.Move(move * Time.deltaTime);
    }
    
}
