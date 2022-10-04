using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Classic()
    {
        Transitioner.instance.TransitionToScene(1);
        PlayerPrefs.SetString("GameMode", "Classic");
    }

    public void Endless()
    {
        Transitioner.instance.TransitionToScene(1);
        PlayerPrefs.SetString("GameMode", "Endless");
    }
}
