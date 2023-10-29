using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Ekkam {
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject interactUI;
        [SerializeField] GameObject usePromptUI;
        [SerializeField] public GameObject lighterUI;

        void Start()
        {
            interactUI.SetActive(false);
            usePromptUI.SetActive(false);
            lighterUI.SetActive(false);
        }

        void Update()
        {
            
        }

        public void ShowInteractPrompt(string interactText)
        {
            interactUI.GetComponentInChildren<TextMeshProUGUI>().text = interactText;
            interactUI.SetActive(true);
        }

        public void HideInteractPrompt()
        {
            interactUI.SetActive(false);
        }

        public void ShowUsePrompt(string useText)
        {
            usePromptUI.GetComponentInChildren<TextMeshProUGUI>().text = useText;
            usePromptUI.SetActive(true);
        }

        public void HideUsePrompt()
        {
            usePromptUI.SetActive(false);
        }

        public void ShowLighterUI()
        {
            lighterUI.SetActive(true);
        }

        public void HideLighterUI()
        {
            lighterUI.SetActive(false);
        }
    }
}
