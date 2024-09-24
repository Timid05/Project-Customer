using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngineInternal;

public class GameManager : MonoBehaviour
{
    public static GameManager GetGameManager()
    {
        return mainManager;
    }

    static GameManager mainManager = null;

    Camera cam;
    GameObject player;
    GameObject checklistUI;
    GameObject checklistPrompt;
    
    ConditionManager conditionManager;

    [SerializeField]
    GameObject difficultySelector;
    [SerializeField]
    GameObject corpse;
    [SerializeField]
    GameObject pc;

    public bool inspecting;
    public bool organGrabbed;
    public bool checklistOpen;
    public bool gameStarted;

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
        checklistUI = GameObject.FindGameObjectWithTag("Checklist");
        checklistPrompt = GameObject.FindGameObjectWithTag("ListPrompt");
        conditionManager = GetComponent<ConditionManager>();
        corpse = GameObject.Find("CorpseTest");

        if (conditionManager == null)
        {
            Debug.LogError("GameManager object is missing ConditionManager script");
        }
        else
        {
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
            checklistPrompt.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameManager could not find UI element with tag 'ListPrompt'");
        }
    }

    public void EnterInspection(Vector3 corpsePos, float camDistance)
    {
        if (cam != null && player != null)
        {
            inspecting = true;
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
            inspecting = false;
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


    float pcLerp = 0;
    [SerializeField]
    float pcLerpSpeed;
    private void Update()
    {
        if (inspecting)
        {
            InspectionInput();
        }

        if (gameStarted)
        {
            
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
            cam.transform.position = pc.transform.position + new Vector3(0, 0.7f, -1);
        }
    }
}
