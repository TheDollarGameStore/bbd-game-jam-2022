using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject pauseMenu;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void LoadMenu()
    {
        Transitioner.instance.TransitionToScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void RestartGame()
    {
        GameManager.instance.RestartLevel();
    }
}