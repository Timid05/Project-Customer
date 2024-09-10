using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;
    Vector3 direction;

    [SerializeField]
    float speed;
    bool moving;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    void InputHandler()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction = transform.forward;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = transform.forward * -1;
            moving = true;
        } 
        else if(Input.GetKey(KeyCode.A))
        {
            moving = true;  
            direction = transform.right * -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moving = true;
            direction = transform.right;
        }
        else
        {
            moving = false;
        }
    }




    void Movement()
    {
        if (moving)
        {
            rb.velocity = new Vector3(direction.x * speed, rb.velocity.y, direction.z * speed);
        }
    }

    void Update()
    {
        if (rb != null)
        {
            InputHandler();
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Movement();
        }
    }
}
