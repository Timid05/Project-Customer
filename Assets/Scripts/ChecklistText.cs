using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChecklistText : MonoBehaviour
{
    TextMeshProUGUI textLine;

    void Start()
    {
        textLine = GetComponent<TextMeshProUGUI>();
        textLine.fontStyle = FontStyles.Strikethrough;
        textLine.enabled = false;
    }

    private void Update()
    {
        if (GameManager.GetGameManager().inspecting)
        {
            if (Input.GetMouseButton(0))
            {
                textLine.fontStyle = FontStyles.Strikethrough;
            }
            textLine.enabled = true;
        }
        else
        {
            textLine.fontStyle = FontStyles.Normal;
            textLine.enabled = false;
        }

    }
}
