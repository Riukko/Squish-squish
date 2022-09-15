using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void PlayEasyBtn()
    {
        PlayerPrefs.SetInt("difficulty", 0);
        SceneManager.LoadScene("Game");
    }
    public void PlayHardBtn()
    {
        PlayerPrefs.SetInt("difficulty", 1);
        SceneManager.LoadScene("Game");
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    public void MenuBtn()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ExitGame()
    {
        PlayerPrefs.DeleteAll();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
              Application.Quit();
        #endif
    }
}
