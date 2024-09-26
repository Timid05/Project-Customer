using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WhiteboardHandler : MonoBehaviour
{
    [SerializeField]
    Image imageOne;
    [SerializeField]
    Image imageTwo;
    [SerializeField]
    Button buttonRight;
    [SerializeField]
    Button buttonLeft;

    int pageNumber;

    private void Start()
    {
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
            buttonRight.gameObject.SetActive(true);
            buttonLeft.gameObject.SetActive(false);
            imageOne.enabled = true;
            imageTwo.enabled = false;
        }

        if (pageNumber == 2)
        {
            buttonRight.gameObject.SetActive(false);
            buttonLeft.gameObject.SetActive(true);
            imageOne.enabled = false;
            imageTwo.enabled = true;
        }
    }
}
