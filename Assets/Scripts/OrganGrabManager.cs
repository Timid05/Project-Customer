using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;

public class OrganGrabManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam; // Camera reference
    [SerializeField]
    private List<GameObject> organs = new List<GameObject>(); // List of organ objects
    [SerializeField]
    private GameObject putBackPrompt;
    [SerializeField]
    private GameObject operationTable;

    [SerializeField]
    private float distanceToCamera = 3f; // Distance to move organ towards camera
    [SerializeField]
    private float grabSpeed = 5f;
    [SerializeField]
    private AudioPlayer audioPlayer;
    [SerializeField]
    private float rotateSpeed = 100f;
    private int tableSpacesUsed;
    private int maxTableSpaces = 8;

    private float grabLerp = 0;
    private bool grabbed = false;
    private GameObject currentOrgan;
    private Vector3 startPos;
    private Quaternion startRotation; // Store the initial rotation
    private Vector3 grabbedPos;

    private GameManager gameManager; // Reference to GameManager
    private Dictionary<GameObject, Vector3> organStartPositions; //For storing the initial positions of the organs inside the body
    private Dictionary<int, Vector3> tablePositions; //For storing the positions on the table that organs can be put on 
    private Dictionary<int, bool> occupiedSpaces; //Keeps track of which tablespaces are occupied by organs

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main; // Assign main camera if not set
        }

        if (putBackPrompt != null)
        {
            putBackPrompt.SetActive(false); // Deactivate the prompt at the start
        }

        organStartPositions = new Dictionary<GameObject, Vector3>();
        tablePositions = new Dictionary<int, Vector3>();
        occupiedSpaces = new Dictionary<int, bool>();

        //Store the initial positions of each organ
        foreach (GameObject organ in organs)
        {
            organStartPositions.Add(organ, organ.transform.position);
        }

        // Find and assign GameManager
        gameManager = GameManager.GetGameManager();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found.");
        }

        if (operationTable == null)
        {
            Debug.LogError("Operation table not assigned to OrganGrabManager");
        }

        //fill out the tablePositions dictionary based on the position of the operation table
        int key = 0;
        for (int i = 0; i < maxTableSpaces; i++)
        {

            float z = 0.5f - 0.3f * i;
            float x;
            for (int j = 0; j < 2; j++)
            {
                key++;
                x = 0.1f - 0.3f * j;
                tablePositions.Add(key, operationTable.transform.position + new Vector3(x, 1.5f, z));
            }
        }

        //give the occupiedSpaces dictionary the same amount of keys as tablePositions and mark all of them false
        foreach (int index in tablePositions.Keys)
        {
            occupiedSpaces.Add(index, false);
        }
    }

    private void Update()
    {
        if (!gameManager.inspecting && currentOrgan != null)
        {
            currentOrgan.transform.position = startPos;
            currentOrgan.transform.rotation = startRotation;
            putBackPrompt.SetActive(false);
            grabbed = false;
        }

        if (gameManager != null && gameManager.inspecting && !gameManager.organGrabbed && Input.GetMouseButtonDown(0) && !gameManager.checklistOpen)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform raycast
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;

                Debug.Log("Raycast hit object: " + hitObject.name);

                // Check if the hit object or any of its parents is an organ in the list
                foreach (GameObject organ in organs)
                {
                    if (hitObject == organ || hitObject.transform.IsChildOf(organ.transform))
                    {
                        Debug.Log("Organ detected: " + organ.name);

                        currentOrgan = organ; // Set the current organ

                        if (currentOrgan.transform.position != organStartPositions[currentOrgan])
                        {
                            foreach(int key in tablePositions.Keys) {
                                //round the tableposition and organposition values so the if statement will function correctly
                                float roundedSpaceX = (float)Math.Round(tablePositions[key].x, 2);
                                float roundedSpaceZ = (float)Math.Round(tablePositions[key].z, 2);
                                float roundedOrganX = (float)Math.Round(organ.transform.position.x, 2);
                                float roundedOrganZ = (float)Math.Round(organ.transform.position.z, 2);

                                //mark the place the clicked organ was at as unoccupied
                                if (roundedOrganX == roundedSpaceX && roundedOrganZ == roundedSpaceZ)
                                {
                                    occupiedSpaces[key] = false;
                                }
                            }
                            tableSpacesUsed--;
                        }

                        startPos = organStartPositions[organ];
                        startRotation = organ.transform.rotation; // Store the initial rotation
                        grabbedPos = cam.transform.position + cam.transform.forward * distanceToCamera;

                        grabbed = true;
                        grabLerp = 0;

                        putBackPrompt.SetActive(true); // Show prompt to put the organ back
                        gameManager.organGrabbed = true; // Set organGrabbed to true
                        break;
                    }
                }
            }
            else
            {
                // Debug: Raycast didn't hit anything
                Debug.Log("Raycast did not hit any object.");
            }
        }

        // If organ is grabbed, move and rotate it
        if (grabbed)
        {
            InspectingOrgan();
        }
    }

    private void InspectingOrgan()
    {
        // Move organ towards camera
        if (grabLerp < 1)
        {
            audioPlayer.OrganNoise();
            grabLerp += grabSpeed * Time.deltaTime;
            currentOrgan.transform.position = Vector3.Lerp(currentOrgan.transform.position, grabbedPos, grabLerp);
            
        }

        if (!gameManager.checklistOpen)
        {
            // Rotate organ based on mouse movement
            if (Input.GetMouseButton(0))
            {
                float xRotation = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
                currentOrgan.transform.Rotate(Vector3.up, -xRotation);
            }

            // Return organ if "E" is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentOrgan.transform.position = organStartPositions[currentOrgan];
                currentOrgan.transform.rotation = startRotation; // Return to original rotation
                grabbed = false;
                currentOrgan = null;
                putBackPrompt.SetActive(false);
                gameManager.organGrabbed = false; // Reset organGrabbed
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                TableHandler();
            }
        }
    }

    private void TableHandler()
    {
        if (tableSpacesUsed != maxTableSpaces)
        {
            
            for (int i = 1; i < maxTableSpaces + 1; i++)  //loop through all the spaces to look for unoccupied ones
            {
                Vector3 currentSpace = tablePositions[i];
                if (occupiedSpaces[i] == false)
                {
                    tableSpacesUsed++;
                    currentOrgan.transform.position = currentSpace; //If the current space is not occupied, assign it to the current organ
                    currentOrgan.transform.rotation = startRotation;
                    occupiedSpaces[i] = true; //mark the current space as occupied
                    grabbed = false;
                    currentOrgan = null;
                    putBackPrompt.SetActive(false);
                    gameManager.organGrabbed = false;
                    break;
                }
            }
        }
        else
        {
            Debug.Log("maximum table spaces occupied");
        }
    }
}
