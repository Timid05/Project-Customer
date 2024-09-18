using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.U2D;

public class ConditionManager : MonoBehaviour
{
    [SerializeField]
    private int numberOfConditionsToApply = 3;

    [SerializeField]
    private OrganCategory[] organCategories; // Array of OrganCategory for different types

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

        // Shuffle and select the top N conditions
        allConditions = Shuffle(allConditions);
        ConditionObject[] selectedConditions = allConditions.Take(numberOfConditionsToApply).ToArray();
        Debug.Log($"Selected {selectedConditions.Length} conditions.");

        // Apply each selected condition to all GameObjects in the corresponding organ category
        foreach (var condition in selectedConditions)
        {
            OrganType organType = condition.OrganType;
            List<GameObject> organsOfType = organDictionary.ContainsKey(organType) ? organDictionary[organType] : null;

            if (organsOfType != null && organsOfType.Count > 0)
            {
                // Apply condition to all GameObjects in this organ category
                foreach (var organ in organsOfType)
                {
                    ApplyConditionToOrgan(condition, organ);

                    // Log the applied condition along with the organ category
                    appliedConditionsLog.Add(new AppliedConditionLog(organ.name, organType, condition));
                    Debug.Log(appliedConditionsLog);
                }

                // Remove the category from the dictionary to avoid reapplying conditions
                organDictionary.Remove(organType);
            }
            else
            {
                Debug.LogWarning($"No organs available for condition type {organType}");
            }
        }

        // Log all applied conditions
        Debug.Log("Applied Conditions:");
        foreach (var log in appliedConditionsLog)
        {
            Debug.Log($"Organ: {log.OrganName}, Category: {log.OrganType}, Condition: {log.Condition.ConditionName}");
        }
    }

    private void ApplyConditionToOrgan(ConditionObject condition, GameObject organ)
    {
        // Apply color to the SpriteShapeRenderer of the GameObject
        Renderer objectRenderer = organ.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            // Set alpha to 100% and apply the color
            Color color = condition.SpriteColor;
            color.a = 1f; // Set alpha to 100%
            objectRenderer.material.color = color;

            Debug.Log($"Applied condition: {condition.ConditionName} to organ: {organ.name}");
        }
        else
        {
            Debug.LogWarning($"SpriteShapeRenderer not found on organ: {organ.name}");
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

// Class to store log data for applied conditions, including the organ category (OrganType)
[System.Serializable]
public class AppliedConditionLog
{
    public string OrganName;
    public OrganType OrganType; // Log the organ category
    public ConditionObject Condition;

    public AppliedConditionLog(string organName, OrganType organType, ConditionObject condition)
    {
        OrganName = organName;
        OrganType = organType;
        Condition = condition;
    }
}

[System.Serializable]
public class OrganCategory
{
    public OrganType OrganType;
    public GameObject[] OrganObjects;
}
