using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    GameManager gameManager;
    Toggle toggle;

    void Start()
    {
        gameManager = GameManager.GetGameManager();
        toggle = GetComponent<Toggle>();

        if (gameManager == null)
        {
            Debug.LogError("Toggle could not find GameManager");
        }

        if (toggle == null)
        {
            Debug.LogError("Toggle could not find its component");
        }
    }

    public void HasCancer(string organName)
    {
        if (toggle.isOn)
        {
            if (!gameManager.selectedConditions.Contains(organName + "Cancer"))
            {
                gameManager.selectedConditions.Add(organName + " Cancer");
            }
        }
        else
        {
            gameManager.selectedConditions.Remove(organName + " Cancer");
        }
    }
}
