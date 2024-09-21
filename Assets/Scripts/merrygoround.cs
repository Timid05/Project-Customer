using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class merrygoround : MonoBehaviour
{
    [SerializeField]
    private float anglepersecond;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, anglepersecond * Time.deltaTime, 0);
    }
}
