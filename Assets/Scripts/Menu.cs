using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        Transitioner.Instance.TransitionToScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
