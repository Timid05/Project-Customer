using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ChecklistText : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI textLine;
    
    private GameManager manager;

    private void Awake()  {
        if(textLine != null) {
            textLine = GetComponent<TextMeshProUGUI>();
        }
        textLine.fontStyle = FontStyles.Normal;
        textLine.enabled = false;
    }

    private void Start()
    {
        manager = GameManager.GetGameManager();  
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (manager.checklistOpen)
        {
            if (textLine.fontStyle == FontStyles.Normal)
            {
                textLine.fontStyle = FontStyles.Strikethrough;
            }
            else
            {
                textLine.fontStyle = FontStyles.Normal;
            }
        }  
    }

    private void Update()
    {
        if (manager.checklistOpen)
        {
            textLine.enabled = true;
        }
        else
        {
            textLine.fontStyle = FontStyles.Normal;
            textLine.enabled = false;
        }

    }
}
