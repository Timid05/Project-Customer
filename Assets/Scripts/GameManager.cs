using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager GetGameManager()
    {
        return mainManager;
    }

    public List<string> correctConditions;
    public List<string> selectedConditions;

    public string difficulty;

    static GameManager mainManager = null;

    Camera cam;
    GameObject player;
    [SerializeField]
    GameObject UI;
    GameObject checklistUI;
    [SerializeField]
    GameObject checklistPrompt;
    [SerializeField]
    GameObject whiteboardUI;
    public GameObject endScreen;

    ConditionManager conditionManager;

    [SerializeField]
    GameObject difficultySelector;
    [SerializeField]
    GameObject corpse;
    [SerializeField]
    GameObject pc;

    public bool inspectingCorpse = false;
    public bool inspectingWhiteboard = false;
    public bool organGrabbed = false;
    public bool checklistOpen = false;
    public bool gameStarted = false;

    public int score;

    private void Awake()
    {
        if (mainManager == null)
        {
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
        checklistUI = GameObject.FindGameObjectWithTag("Checklist");
        conditionManager = GetComponent<ConditionManager>();
        endScreen = GameObject.FindGameObjectWithTag("EndScreen");

        correctConditions = new List<string>();
        selectedConditions = new List<string>();

        if (endScreen != null)
        {
            endScreen.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameManager could not find endscreen objecct");
        }

        if (whiteboardUI == null)
        {
            Debug.LogWarning("Please assign whiteboard to the gamemanager");
        }


        if (conditionManager == null)
        {
            Debug.LogError("GameManager object is missing ConditionManager script");
        }
        else
        {
            conditionManager.enabled = false;
            corpse.SetActive(false);
            if (corpse == null)
            {
                Debug.Log("deactivated, corpse is now null");
            }
        }

        if (player != null)
        {

        }
        else
        {
            Debug.LogError("GameManager could not find the player");
        }

        if (checklistUI != null)
        {
            checklistUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameManager did not find Checklist UI");
        }

        if (checklistPrompt != null)
        {
            //checklistPrompt.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameManager could not find UI element with tag 'ListPrompt'");
        }


        whiteboardUI.SetActive(false);
        UI.SetActive(false);
    }

    public void EnterInspection(Vector3 corpsePos, float camDistance)
    {
        if (cam != null && player != null)
        {
            inspectingCorpse = true;
            checklistPrompt.SetActive(true);
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
            checklistPrompt.SetActive(false);
            inspectingCorpse = false;
            organGrabbed = false;
            player.GetComponent<Renderer>().enabled = true;
            cam.transform.position = player.transform.position + Vector3.up * 0.8f;
            Cursor.lockState = CursorLockMode.Locked;
            checklistUI.SetActive(false);
            checklistOpen = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (checklistUI.activeSelf)
            {
                checklistOpen = false;
                checklistUI.SetActive(false);
            }
            else
            {
                checklistOpen = true;
                checklistUI.SetActive(true);
            }
        }
    }


    public void Scoring()
    {
        score = 0;
        foreach (string condition in selectedConditions)
        {
            if (correctConditions.Contains(condition))
            {
                score++;
            }
        }

        Debug.Log("current score is " + score + " out of " + correctConditions.Count);

        foreach (string condition in correctConditions)
        {
            Debug.Log("correct condition: " + condition);
        }
    }

    public void Restart()
    {
        checklistUI.SetActive(false);
        checklistPrompt.SetActive(false);

    }


    float pcLerp = 0;
    [SerializeField]
    float pcLerpSpeed;
    private void Update()
    {

        if (inspectingWhiteboard)
        {
            whiteboardUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            whiteboardUI.SetActive(false);
        }

        if (inspectingCorpse)
        {
            InspectionInput();
        }

        if (gameStarted)
        {
            UI.SetActive(true);
            corpse.SetActive(true);
            difficultySelector.SetActive(false);
            if (pcLerp <= 1)
            {
                pcLerp += pcLerpSpeed / 50 * Time.fixedDeltaTime;
                cam.transform.position = Vector3.Lerp(cam.transform.position, player.transform.position + Vector3.up * 0.8f, pcLerp);
            }
        }
        else
        {
            checklistPrompt.SetActive(false);
            cam.transform.position = pc.transform.position + new Vector3(0, 0.7f, -1);
        }
    }
}
