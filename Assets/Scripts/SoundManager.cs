using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public int levelUnlocked = 1;
    public AudioClip level1, level2, level3, level4;
    public AudioSource effectSource, musicSource;
    public static SoundManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (!musicSource.isPlaying)
        {
            if (levelUnlocked >= 1)
            {
                musicSource.PlayOneShot(level1);
            }
            if (levelUnlocked >= 2)
            {
                musicSource.PlayOneShot(level2);
            }
            if (levelUnlocked >= 3)
            {
                musicSource.PlayOneShot(level3);
            }
            if (levelUnlocked >= 4)
            {
                musicSource.PlayOneShot(level4);
            }
        }
    }
}
