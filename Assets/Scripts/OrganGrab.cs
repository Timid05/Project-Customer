using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class OrganGrab : MonoBehaviour
{
    Camera cam;
    GameObject putBackPrompt;

    [SerializeField]
    float distanceToCamera;
    [SerializeField]
    float grabSpeed;
    bool grabbed;

    Vector3 startPos;
    Vector3 grabbedPos;

    private void Start()
    {
        cam = Camera.main;
        putBackPrompt = GameObject.FindGameObjectWithTag("OrganPrompt");
        startPos = transform.position;

        if (putBackPrompt != null)
        {
            putBackPrompt.SetActive(false);
        }
        else
        {
            Debug.Log("UI missing");
        }
    }


    private void OnMouseDown()
    {
        if (cam != null)
        {
            if (GameManager.GetGameManager().inspecting && !grabbed)
            {
                grabbed = true;
                grabbedPos = cam.transform.position + Vector3.down * distanceToCamera;
                grabSpeed = 0;
            }
        }
        else
        {
            Debug.Log(name + "could not find camera");
        }
    }


    void InspectingOrgan()
    {
        if (grabSpeed < 1)
        {
            grabSpeed = grabSpeed + Time.fixedDeltaTime;
        }

        putBackPrompt.SetActive(true);

        transform.localPosition = Vector3.Lerp(startPos, grabbedPos, grabSpeed);

        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.position = startPos;
            putBackPrompt.SetActive(false);
            grabbed = false;
        }
    }

    private void Update()
    {
        if (grabbed && GameManager.GetGameManager().inspecting)
        {
          
            InspectingOrgan();
        }
        else
        {
            transform.localPosition = startPos;
        }

        if (!GameManager.GetGameManager().inspecting)
        {
            grabbed = false;            
            putBackPrompt.SetActive(false);
        }
    }
}
