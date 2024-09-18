using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GetGameManager()
    {
        return mainManager;
    }

    static GameManager mainManager = null;

    Camera cam;
    GameObject player;

    public bool inspecting;
    public bool organGrabbed;

    private void Awake()
    {
        if (mainManager == null)
        {
            DontDestroyOnLoad(gameObject);
            mainManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
    }

    public void EnterInspection(Vector3 corpsePos, float camDistance)
    {
        if (cam != null && player != null)
        {
            inspecting = true;
            cam.transform.position = corpsePos + Vector3.up * camDistance;
            cam.transform.LookAt(corpsePos);
            player.GetComponent<Renderer>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }

        if (cam == null) 
        {
            Debug.Log("GameManager did not find a camera");
        }
        if (player == null)
        {
            Debug.Log("GameManager did not find a GameObject with the tag 'Player'");
        }
    }

    public void InspectionInput()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            inspecting = false;
            player.GetComponent<Renderer>().enabled = true;
            cam.transform.position = player.transform.position + Vector3.up * 0.8f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        if (inspecting) { 
          InspectionInput();
        }
    }
}
