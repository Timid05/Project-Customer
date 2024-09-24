using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ConditionManager : MonoBehaviour
{
    [SerializeField]
    public int numberOfConditionsToApply = 3;

    [SerializeField]
    private int numberOfRandomCancersToApply = 2; // Amount of random cancers to apply regardless of condition

    [SerializeField]
    private OrganCategory[] organCategories; // Array of OrganCategory for different types

    [SerializeField]
    private GameObject cancerPrefab; // Prefab to instantiate if organ has cancer

    private Dictionary<OrganType, List<GameObject>> organDictionary;

    // List to keep track of applied conditions for later use
    private List<AppliedConditionLog> appliedConditionsLog = new List<AppliedConditionLog>();

    private void Start()
    {
        // Initialize the dictionary
        organDictionary = new Dictionary<OrganType, List<GameObject>>();

        // Populate the dictionary based on the OrganCategory array
        foreach (var category in organCategories)
        {
            organDictionary[category.OrganType] = new List<GameObject>(category.OrganObjects);
        }

        ApplyRandomConditions();

        // Apply random cancers on random organs
        ApplyRandomCancers();
    }

    private void ApplyRandomConditions()
    {
        // Load all condition objects from Resources
        ConditionObject[] allConditions = Resources.LoadAll<ConditionObject>("conditions");
        Debug.Log($"Loaded {allConditions.Length} conditions from Resources.");

        // Ensure there are enough conditions
        if (allConditions.Length < numberOfConditionsToApply)
        {
            Debug.LogError("Not enough conditions to apply.");
            return;
        }

        // Shuffle conditions to randomize selection
        allConditions = Shuffle(allConditions);

        // Track applied conditions and organ types
        HashSet<OrganType> assignedOrganTypes = new HashSet<OrganType>();
        HashSet<string> appliedConditionNames = new HashSet<string>();

        int conditionsApplied = 0; // Track how many conditions have been applied

        // Iterate over shuffled conditions
        foreach (var condition in allConditions)
        {
            OrganType organType = condition.OrganType;

            // Check if this organ type has already been assigned a condition or if the condition name is already applied
            if (assignedOrganTypes.Contains(organType) || appliedConditionNames.Contains(condition.ConditionName))
            {
                continue; // Skip this condition if already assigned to the organ type or condition name
            }

            List<GameObject> organsOfType = organDictionary.ContainsKey(organType) ? organDictionary[organType] : null;

            if (organsOfType != null && organsOfType.Count > 0)
            {
                // Apply condition to all GameObjects in this organ category
                foreach (var organ in organsOfType)
                {
                    ApplyConditionToOrgan(condition, organ);

                    // Log the applied condition along with the organ category
                    appliedConditionsLog.Add(new AppliedConditionLog(organ.name, organType, condition, condition.HasCancer));
                }

                // Mark this organ type as assigned and condition name as applied
                assignedOrganTypes.Add(organType);
                appliedConditionNames.Add(condition.ConditionName);
                conditionsApplied++;

                // Stop if we have applied the required number of conditions
                if (conditionsApplied >= numberOfConditionsToApply)
                {
                    break; // Exit the loop once we have enough conditions
                }
            }
            else
            {
                Debug.LogWarning($"No organs available for condition type {organType}");
            }
        }

        // Log all applied conditions
        Debug.Log("Applied " +numberOfConditionsToApply + " Conditions");
        foreach (var log in appliedConditionsLog)
        {
            Debug.Log($"Organ: {log.OrganName}, Category: {log.OrganType}, Condition: {log.Condition.ConditionName}");
        }
    }

    private void ApplyConditionToOrgan(ConditionObject condition, GameObject organ)
    {
        // Apply color and material to all child GameObjects with MeshRenderer
        ApplyColorAndMaterialToChildren(organ, condition);

        // Check if the condition has cancer, if true, instantiate the cancer prefab
        if (condition.HasCancer && cancerPrefab != null)
        {
            // Get a random position on the surface of the mesh
            Vector3 surfacePosition = GetRandomSurfacePosition(organ, out Vector3 normal);

            // Instantiate the prefab at the surface position and set it as a child of the organ
            GameObject cancerObject = Instantiate(cancerPrefab, surfacePosition, Quaternion.LookRotation(normal), organ.transform);

            // Optionally, adjust cancerObject's orientation to better align with the surface normal
            cancerObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        }

        // Log application
        Debug.Log($"Applied condition: {condition.ConditionName} to organ: {organ.name}");
    }

    private void ApplyRandomCancers()
{
    // Create a flat list of all organs
    List<GameObject> allOrgans = organDictionary.Values.SelectMany(organList => organList).ToList();

    // Shuffle the list to randomize the selection of organs
    allOrgans = Shuffle(allOrgans.ToArray()).ToList();

    // Apply cancer to the first 'numberOfRandomCancersToApply' organs
    for (int i = 0; i < Mathf.Min(numberOfRandomCancersToApply, allOrgans.Count); i++)
    {
        GameObject organ = allOrgans[i];

        // Get the existing log entry for this organ
        var existingLog = appliedConditionsLog.FirstOrDefault(log => log.OrganName == organ.name);

        // Get a random position on the surface of the mesh
        Vector3 surfacePosition = GetRandomSurfacePosition(organ, out Vector3 normal);

        // Instantiate the cancerPrefab at the random position and set it as a child of the organ
        GameObject cancerObject = Instantiate(cancerPrefab, surfacePosition, Quaternion.LookRotation(normal), organ.transform);

        // Optionally, adjust cancerObject's orientation to better align with the surface normal
        cancerObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);

        // If there is an existing log, update its HasCancer property; otherwise, create a new log entry
        if (existingLog != null)
        {
            existingLog.HasCancer = true; // Mark cancer as applied
            Debug.Log($"{organ} + {existingLog.HasCancer}");
        }
        else
        {
            AppliedConditionLog log = new AppliedConditionLog(organ.name, GetOrganType(organ), null, true);
            appliedConditionsLog.Add(log); // Create and add new log entry
            
        }

        Debug.Log($"Random cancer applied to organ: {organ.name}");
    }
}

// Helper method to get the organ type from the GameObject
    private OrganType GetOrganType(GameObject organ)
{
    // Implement logic to determine the organ type based on the GameObject
    return OrganType.Lung; // Replace this with actual logic.
}
    private Vector3 GetRandomSurfacePosition(GameObject organ, out Vector3 normal)
    {
        // Search for the MeshFilter in the child objects
        MeshFilter meshFilter = organ.GetComponentInChildren<MeshFilter>();

        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;

            // Pick a random vertex
            int randomIndex = Random.Range(0, vertices.Length);

            // Get the world position of the vertex
            Vector3 worldPosition = meshFilter.transform.TransformPoint(vertices[randomIndex]);

            // Get the normal direction at this vertex
            normal = meshFilter.transform.TransformDirection(normals[randomIndex]);

            // Return the world position (vertex on surface)
            return worldPosition;
        }
        else
        {
            Debug.LogWarning("No MeshFilter found on any children of the organ for random surface selection.");
            normal = Vector3.up; // Default normal
            return organ.transform.position; // Fallback to organ's position
        }
    }

    private void ApplyColorAndMaterialToChildren(GameObject organ, ConditionObject condition)
    {
        // Retrieve the MeshRenderer components from the children
        MeshRenderer[] renderers = organ.GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            // Set material if available
            if (condition.Material != null)
            {
                // Apply the specified material to all meshes
                Material[] materials = new Material[renderer.materials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = condition.Material;
                }
                renderer.materials = materials;
            }
            else
            {
                // Set color only if no material is assigned
                Color color = condition.SpriteColor;
                color.a = 1f; // Set alpha to 100%

                // Apply color to all materials of the MeshRenderer
                foreach (var material in renderer.materials)
                {
                    material.color = color;
                }
            }
        }
    }

    private T[] Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[rnd];
            array[rnd] = temp;
        }
        return array;
    }

    // Method to retrieve the applied conditions log
    public List<AppliedConditionLog> GetAppliedConditionsLog()
    {
        return appliedConditionsLog;
    }
}



[System.Serializable]
public class AppliedConditionLog
{
    public string OrganName;
    public OrganType OrganType; // Log the organ category
    public ConditionObject Condition;
    public bool HasCancer;       // Log if cancer was applied by condition

    public AppliedConditionLog(string organName, OrganType organType, ConditionObject condition, bool hasCancer)
    {
        OrganName = organName;
        OrganType = organType;
        Condition = condition;
        HasCancer = hasCancer;
    }
}


[System.Serializable]
public class OrganCategory
{
    public OrganType OrganType;
    public GameObject[] OrganObjects;
}