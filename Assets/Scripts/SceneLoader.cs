using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameManager manager;
    ConditionManager conditionManager;

    private void Start()
    {
        manager = GameManager.GetGameManager();
        conditionManager = manager.GetComponent<ConditionManager>();
        conditionManager.enabled = false;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartRun(string difficulty)
    {
        if (difficulty == "Easy")
        {
            conditionManager.numberOfConditionsToApply = 2;
        }
        if (difficulty == "Medium")
        {
            conditionManager.numberOfConditionsToApply = 3;
        }
        if (difficulty == "Hard")
        {
            conditionManager.numberOfConditionsToApply = 5;
        }
        if (difficulty == "Expert")
        {
            conditionManager.numberOfConditionsToApply = 12;
        }

        conditionManager.enabled = true;
        manager.gameStarted = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
