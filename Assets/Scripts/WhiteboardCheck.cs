using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteboardCheck : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]
    GameObject player;
    MeshRenderer playerRenderer;
    Camera cam;

    Vector3 relativePos;

    [SerializeField]
    float range;
    [SerializeField]
    float offset;

    void Start()
    {
        gameManager = GameManager.GetGameManager();
        cam = Camera.main;

        if (player == null)
        {
            Debug.LogWarning("please assign player to whiteboard");
        }

        playerRenderer = player.GetComponent<MeshRenderer>();
    }


    void Update()
    {
        relativePos = transform.position - player.transform.position;

        if (relativePos.magnitude < range)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!gameManager.inspectingWhiteboard)
                {
                    playerRenderer.enabled = false; 
                    cam.transform.position = transform.position - new Vector3(0, 0, offset);
                    cam.transform.rotation = Quaternion.Euler(0, 0, 0);
                    gameManager.inspectingWhiteboard = true;
                }
                else
                {
                    playerRenderer.enabled = true;
                    cam.transform.position = player.transform.position + Vector3.up * 0.8f;
                    cam.transform.rotation = player.transform.rotation;
                    gameManager.inspectingWhiteboard = false;

                }
            }
        }
    }
}
