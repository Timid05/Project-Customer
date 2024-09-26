using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WhiteboardHandler : MonoBehaviour
{
    GameManager manager;

    [SerializeField]
    Image imageOne;
    [SerializeField]
    Image imageTwo;
    [SerializeField]
    Button buttonRight;
    [SerializeField]
    Button buttonLeft;
    [SerializeField]
    TextMeshProUGUI patientText;

    int pageNumber;

    private void Start()
    {
        manager = GameManager.GetGameManager();

        if (imageOne != null && imageTwo != null)
        {
            imageTwo.enabled = false;
            buttonLeft.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Please assign images to whiteboard");
        }
    }

    public void ChangePage(int page)
    {
        pageNumber = page;
    }

    void Update()
    {
        if (pageNumber == 1)
        {
            patientText.enabled = true;
            buttonRight.gameObject.SetActive(true);
            buttonLeft.gameObject.SetActive(false);
            imageOne.enabled = true;
            imageTwo.enabled = false;
        }

        if (pageNumber == 2)
        {
            patientText.enabled = false;
            buttonRight.gameObject.SetActive(false);
            buttonLeft.gameObject.SetActive(true);
            imageOne.enabled = false;
            imageTwo.enabled = true;
        }

        if (manager.difficulty == "Easy")
        {
            patientText.text = "John (N/A) as they called him (John Doe) was the local homeless guy, unfortunately he was known to have issues with using and abusing.";
        }

        if (manager.difficulty == "Medium")
        {
            patientText.text = "Steven (28) was once a rising star in his field, thriving under pressure until the weight of it all pushed him too far. He turned to something he thought would help him keep up with the demands of his life, but it quickly spiraled out of control.";
        }

        if (manager.difficulty == "Hard")
        {
            patientText.text = "Mario’s (21) life seemed perfect from the outside—he was a bright university student with a full social calendar. But underneath, he was battling her own demons. His health began to deteriorate rapidly as she fell deeper into her personal struggle, pushing away friends and family. By the time he sought help, it was too late, and his body had paid the price.";
        }

        if (manager.difficulty == "Expert")
        {
            patientText.text = "Joe’s (34) story is one of prolonged suffering and deep-rooted dependency. What started as an attempt to manage chronic pain turned into something far more destructive. Over the years, his health spiraled, and by the end, multiple organs were failing. His case is complex, with severe damage across several areas, making it difficult to pinpoint the exact timeline of his decline. ";
        }
    }
}
