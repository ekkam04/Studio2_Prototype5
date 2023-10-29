using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ekkam {
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
            if (lidOpen)
            {
                hingeRotationZ = Mathf.Lerp(hingeRotationZ, 90f, Time.deltaTime * 5f);
            }
            else
            {
                hingeRotationZ = Mathf.Lerp(hingeRotationZ, 0f, Time.deltaTime * 5f);
            }

            hinge.localRotation = Quaternion.Euler(0f, 0f, hingeRotationZ);

            // Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        public void ToggleLighter()
        {
            flame.SetActive(!flame.activeSelf);
            lidOpen = !lidOpen;
        }
    }
}
