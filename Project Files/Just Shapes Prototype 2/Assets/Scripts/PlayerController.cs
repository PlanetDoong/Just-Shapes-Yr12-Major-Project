using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed;
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component on the player
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Store the vertical and horizontal axis this frame into separate variables to make line 24 shorter and
        // easier to read
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Change the player's velocity to a new 3D vector (I'm using 3D physics in this 2D game because I find it easier)
        // Multiply the x and y values by the speed variable and Time.deltaTime
        // I multiplied it by Time.deltaTime so the player's speed isn't affected by the frame rate the game is running at
        rb.velocity = new Vector3(horizontal * Time.deltaTime * speed, vertical * Time.deltaTime * speed, 0);
    }
}
