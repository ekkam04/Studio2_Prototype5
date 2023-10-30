using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Ekkam {
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject interactUI;
        [SerializeField] GameObject usePromptUI;
        [SerializeField] GameObject pauseUI;
        [SerializeField] public GameObject lighterUI;

        GeneratorFixing generatorFixing;

        void Start()
        {
            generatorFixing = FindObjectOfType<GeneratorFixing>();
            interactUI.SetActive(false);
            usePromptUI.SetActive(false);
            lighterUI.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (generatorFixing != null && generatorFixing.generatorFixingUI.activeSelf)
                {
                    generatorFixing.EndFixing();
                }
                else
                {
                    if (pauseUI.activeSelf)
                    {
                        ResumeGame();
                    }
                    else
                    {
                        PauseGame();
                    }
                }
            }
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseUI.SetActive(true);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseUI.SetActive(false);
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main Menu");
        }

        public void QuitGame()
        {
            Application.Quit();
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
