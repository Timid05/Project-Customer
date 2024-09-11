using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CorpseCheck : MonoBehaviour
{
    GameObject player;
    GameObject prompt;

    Vector3 relativePos;

    [SerializeField]
    float checkDistance;
    [SerializeField]
    float cameraDistance;
    bool inRange;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        prompt = GameObject.FindGameObjectWithTag("CorpsePrompt");

        prompt.SetActive(false);
    }

    void InputHandler()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E)) 
        {
           GameManager.GetGameManager().EnterInspection(transform.localPosition, cameraDistance);
           inRange = false;
           prompt.SetActive(false);
        }
    }
    
    void Update()
    {
        relativePos = player.transform.localPosition - transform.localPosition;

        if (relativePos.magnitude < checkDistance && !GameManager.GetGameManager().inspecting)
        {
            prompt.SetActive(true);
            inRange = true;
        }
        else
        {
            prompt.SetActive(false);
            inRange = false;
        }

        InputHandler();
    }
}
