using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ekkam {
    public class Lighter : MonoBehaviour
    {

        [SerializeField] private GameObject flame;
        [SerializeField] private Transform hinge;
        [SerializeField] private Light flameLight;

        private float initialLightIntensity;

        private float hingeRotationZ = 0f;
        private bool lidOpen = false;
        public float fuel = 100f;
        public Slider fuelSlider;
        public float fuelBurnRate = 1f;

        UIManager uiManager;

        void Start()
        {
            uiManager = GameObject.FindObjectOfType<UIManager>();
            fuelSlider = uiManager.lighterUI.GetComponentInChildren<Slider>();
            fuelSlider.value = fuel;
            initialLightIntensity = flameLight.intensity;
            flame.SetActive(false);
        }

        void Update()
        {
            if (lidOpen)
            {
                hingeRotationZ = Mathf.Lerp(hingeRotationZ, 90f, Time.deltaTime * 5f);
                fuel -= fuelBurnRate * Time.deltaTime;
                fuelSlider.value = fuel;
                // scale the light intensity based on the fuel
                flameLight.intensity = Mathf.Lerp(flameLight.intensity, initialLightIntensity * (fuel / 100f), Time.deltaTime * 5f);
                if (fuel <= 0f)
                {
                    ToggleLighter();
                }
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
            if (!lidOpen && fuel > 0f)
            {
                lidOpen = true;
                flame.SetActive(true);
            }
            else
            {
                lidOpen = false;
                flame.SetActive(false);
            }
        }

        public void Refuel()
        {
            fuel = 100f;
            fuelSlider.value = fuel;
        }
    }
}
