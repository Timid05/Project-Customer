using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConditionsText : MonoBehaviour
{
    TextMeshProUGUI conditionsText;
    GameManager manager;

    void Start()
    {
        conditionsText = GetComponent<TextMeshProUGUI>();
        manager = GameManager.GetGameManager(); 
    }

    
    void Update()
    {
        conditionsText.text = "Correct conditions: " + string.Join(", ", manager.correctConditions);
    }
}
