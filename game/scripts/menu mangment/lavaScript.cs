using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lavaScript : MonoBehaviour
{
    public gameMenuScript gameMenuScript;
    public void restartLevel(int levelIndex)
    {
        gameMenuScript.canOpenMenu = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(levelIndex);
    }

    public void returnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
