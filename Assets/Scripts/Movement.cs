// This script moves a game object left, right, forwards, backwards...
// using input from keyboard/gamepad (set in the Input Manager)
// 'Update' Method is used for the Input (keyboard/Gamepad)
// 'Fixed' Method is used for physics movement
// The Input is 'Normalized' to prevent faster diagonal movement
// 'Time.fixedDeltaTime' is used to keep the physics framrate consistant on all devices
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Movement : MonoBehaviour
{
    // Add the variables
    [SerializeField]
    private float speed; // Speed variable
    private Rigidbody rb; // Set the variable 'rb' as Rigibody
    private Vector3 movement; // Set the variable 'movement' as a Vector3 (x,y,z)
    
    // 'Start' Method run once at start for initialisation purposes
    void Start()
    {
        // find the Rigidbody of this game object and add it to the variable 'rb'
        rb = this.GetComponent<Rigidbody>();
    }
    
    // 'Update' Method is called once per frame
    void Update()
    {
        // In Update we get the Input for left, right, up and down and put it in the variable 'movement'...
        // We only get the input of x and z, y is left at 0 as it's not required
        // 'Normalized' diagonals to prevent faster movement when two inputs are used together
        movement = new Vector3(Input.GetAxis("Horizontal"), 0 ,Input.GetAxis("Vertical")).normalized;
    }
    
    // 'FixedUpdate' Method is used for Physics movements
    void FixedUpdate()
    {
        moveCharacter(movement); // We call the function 'moveCharacter' in FixedUpdate for Physics movement
    }
    
    // 'moveCharacter' Function for moving the game object
    void moveCharacter(Vector3 direction)
    {
        // We multiply the 'speed' variable to the Rigidbody's velocity...
        // and also multiply 'Time.fixedDeltaTime' to keep the movement consistant on all devices
        rb.velocity = direction * speed * Time.fixedDeltaTime;
    }
 
}