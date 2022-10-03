using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        Transitioner.instance.TransitionToScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
