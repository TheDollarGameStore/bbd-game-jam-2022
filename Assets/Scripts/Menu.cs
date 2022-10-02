using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DecrementLevelUnlocked()
    {
        SoundManager.Instance.levelUnlocked--;
    }

    public void IncrementLevelUnlocked()
    {
        SoundManager.Instance.levelUnlocked++;
    }
}
