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

    //Input handler that changes the direction of the player depending on the key pressed
    void InputHandler()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction = transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = transform.forward * -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = transform.right * -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = transform.right;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            direction = transform.right + transform.forward;
            direction = Vector3.Normalize(direction);
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            direction = -transform.right + transform.forward;
            direction = Vector3.Normalize(direction);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            direction = -transform.right + -transform.forward;
            direction = Vector3.Normalize(direction);
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            direction = transform.right + -transform.forward;
            direction = Vector3.Normalize(direction);
        }

        if (!GameManager.GetGameManager().inspecting)
        {
            if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                moving = false;
            }
            else
            {
                moving = true;
            }
        }
        else
        {
            moving = false;
            direction = Vector3.zero;
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
