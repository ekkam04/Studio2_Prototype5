using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartLevel1()
    {
        SceneManager.LoadScene("EkkamScene");
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
