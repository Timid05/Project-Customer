using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI resultText;
    GameManager gameManager;

    float correctness;

    void Start()
    {
        gameManager = GameManager.GetGameManager();

        if (gameManager == null)
        {
            Debug.LogError("Endscreen could not find GameManager");
        }

        if (scoreText == null)
        {
            Debug.LogWarning("Assign scoretext to the endscreen");
        } 

        if (resultText == null)
        {
            Debug.LogWarning("Assign resulttext to the endscreen");
        }
    }

    
    void Update()
    {
        correctness = (float)gameManager.score / (float)gameManager.correctConditions.Count * 100;

        scoreText.text = "Your score is " + gameManager.score + " out of " + gameManager.correctConditions.Count;

        if (correctness > 70)
        {
            resultText.text = "Good job Doc, the police will be waiting on it.";
        }

        if (correctness <= 70 && correctness >= 30)
        {
            resultText.text = "Come on, Doc. Don't get rusty.";
        }

        if (correctness < 30)
        {
            resultText.text = "For the sake of the public, it's been deemed best that you return to a med school.";
        } 
    }
}
