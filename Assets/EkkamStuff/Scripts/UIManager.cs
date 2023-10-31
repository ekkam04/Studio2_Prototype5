using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace Ekkam {
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject interactUI;
        [SerializeField] GameObject usePromptUI;
        [SerializeField] GameObject pauseUI;
        [SerializeField] GameObject livesUI;
        [SerializeField] GameObject winUI;
        [SerializeField] GameObject loseUI;
        [SerializeField] public GameObject lighterUI;

        [SerializeField] RectTransform[] blackPanels;

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

        public void UpdateLives(int lives)
        {
            livesUI.GetComponentInChildren<TextMeshProUGUI>().text = "Lives: " + lives.ToString();
        }

        async public void ShowBlackPanels()
        {
            // move the black panels to the right of the screen
            for (int i = 0; i < blackPanels.Length; i++)
            {
                blackPanels[i].anchoredPosition = new Vector2(1930, blackPanels[i].anchoredPosition.y);
                blackPanels[i].gameObject.SetActive(true);
            }
            // move the black panels to the left of the screen
            for (int i = 0; i < blackPanels.Length; i++)
            {
                LeanTween.moveX(blackPanels[i], 0, 1f).setEaseOutCubic();
                await Task.Delay(50);
            }
        }

        async public void HideBlackPanels()
        {
            // move the black panels to the right of the screen
            for (int i = 0; i < blackPanels.Length; i++)
            {
                LeanTween.moveX(blackPanels[i], 1930, 1f).setEaseInCubic();
                await Task.Delay(50);
            }
            await Task.Delay(1000);
            for (int i = 0; i < blackPanels.Length; i++)
            {
                blackPanels[i].gameObject.SetActive(false);
            }
        }

        public void ShowWinUI()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            winUI.SetActive(true);
        }

        public void ShowLoseUI()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            loseUI.SetActive(true);
        }
    }
}
