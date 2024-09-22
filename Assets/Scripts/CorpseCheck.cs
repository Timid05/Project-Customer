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

        if (prompt != null)
        {
            prompt.SetActive(false);
        }
        else
        {
            Debug.Log("UI missing");
        }
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
        if (player != null)
        {
            relativePos = player.transform.localPosition - transform.localPosition;
        }
        else
        {
            Debug.Log("Corpse did not find GameObject with tag 'Player'");
        }

        if (relativePos.magnitude < checkDistance && !GameManager.GetGameManager().inspecting)
        {
            if (prompt != null)
            {
                prompt.SetActive(true);
                inRange = true;
            }
            else
            {
                Debug.Log("Prompt is null most likely not assigned, please check the assignment and make sure the gameobject is active");
            }
        }
        else
        {
            if (prompt != null)
            {
                prompt.SetActive(false);
                inRange = false;
            }
            else
            {
                Debug.Log("Prompt is null most likely not assigned, please check the assignment make sure the gameobject is active");
            }

        }

        InputHandler();
    }
}
