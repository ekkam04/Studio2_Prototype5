using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : MonoBehaviour
{

    [SerializeField] private GameObject flame;
    [SerializeField] private Transform hinge;

    private float hingeRotationZ = 0f;
    private bool lidOpen = false;

    void Start()
    {
        flame.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleLighter();

        }

        if (lidOpen)
        {
            hingeRotationZ = Mathf.Lerp(hingeRotationZ, 90f, Time.deltaTime * 5f);
        }
        else
        {
            hingeRotationZ = Mathf.Lerp(hingeRotationZ, 0f, Time.deltaTime * 5f);
        }

        hinge.localRotation = Quaternion.Euler(0f, 0f, hingeRotationZ);
    }

    void ToggleLighter()
    {
        flame.SetActive(!flame.activeSelf);
        lidOpen = !lidOpen;
    }
}
