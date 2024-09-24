using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    float mouseX;
    float mouseY;
    float xRotation;
    float yRotation;

    public float mouseSensitivity = 3;

    GameObject player;
    GameManager manager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        manager = GameManager.GetGameManager();
    }

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100 * Time.fixedDeltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100 * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        if (!manager.inspecting && manager.gameStarted)
        {
            xRotation += mouseX;
            yRotation -= mouseY;

            yRotation = Mathf.Clamp(yRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            if (player != null)
            {
                player.transform.localRotation = Quaternion.Euler(0f, xRotation, 0f);
            } 
            else
            {
                Debug.Log("No GameObject with tag 'Player' has been found by the camera");
            }
        } 
    }
}
