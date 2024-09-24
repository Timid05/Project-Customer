using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;


public class DropdownHandler : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown dropdown;

    GameManager gameManager;
    string oldCaption;
    
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        gameManager = GameManager.GetGameManager();
       
        if (dropdown == null)
        {
            Debug.LogError("could not find dropdown component");
        }

        if (gameManager == null)
        {
            Debug.LogError("dropdown could not find GameManager");
        }
        
    }

    void Update()
    {
       if (dropdown.captionText.text != oldCaption || oldCaption == null)
        {
            gameManager.selectedConditions.Remove(oldCaption);
            gameManager.selectedConditions.Add(dropdown.captionText.text);
            oldCaption = dropdown.captionText.text;  
        }
    }
}
