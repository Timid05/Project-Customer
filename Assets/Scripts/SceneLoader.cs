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
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Loadmainscene(){
        SceneManager.LoadScene("MainScene");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void End()
    {
        if (manager.endScreen != null)
        {
            manager.Scoring();
            manager.endScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("EndScreen is null");
        }
    }

    public void StartRun(string difficulty)
    {
        if (difficulty == "Easy")
        {
            manager.difficulty = "Easy";
            conditionManager.numberOfConditionsToApply = 2;
        }
        if (difficulty == "Medium")
        {
            manager.difficulty = "Medium";
            conditionManager.numberOfConditionsToApply = 3;
        }
        if (difficulty == "Hard")
        {
            manager.difficulty = "Hard";
            conditionManager.numberOfConditionsToApply = 5;
        }
        if (difficulty == "Expert")
        {
            manager.difficulty = "Expert";
            conditionManager.numberOfConditionsToApply = 6;
        }

        conditionManager.enabled = true;
        manager.gameStarted = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
