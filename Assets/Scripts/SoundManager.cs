using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public int levelUnlocked;
    public float musicVolume;
    public AudioSource audioSourceLevel1, audioSourceLevel2, audioSourceLevel3, audioSourceLevel4, soundSource;
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
        audioSourceLevel1.volume = levelUnlocked >= 1 ? musicVolume : 0f;
        audioSourceLevel2.volume = levelUnlocked >= 2 ? musicVolume : 0f;
        audioSourceLevel3.volume = levelUnlocked >= 3 ? musicVolume : 0f;
        audioSourceLevel4.volume = levelUnlocked >= 4 ? musicVolume : 0f;
    }

    public void Update()
    {
        audioSourceLevel1.volume = Mathf.Lerp(audioSourceLevel1.volume, levelUnlocked >= 1 ? musicVolume : 0f, Time.deltaTime);
        audioSourceLevel2.volume = Mathf.Lerp(audioSourceLevel2.volume, levelUnlocked >= 2 ? musicVolume : 0f, Time.deltaTime);
        audioSourceLevel3.volume = Mathf.Lerp(audioSourceLevel3.volume, levelUnlocked >= 3 ? musicVolume : 0f, Time.deltaTime);
        audioSourceLevel4.volume = Mathf.Lerp(audioSourceLevel4.volume, levelUnlocked >= 4 ? musicVolume : 0f, Time.deltaTime);
    }

    public void LevelUp()
    {
        if (levelUnlocked < 4)
        levelUnlocked++;
    }

    public void LevelDown()
    {
        if (levelUnlocked > 1)
        {
            levelUnlocked--;
        }
    }

    public void PlayRandomized(AudioClip clip)
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(clip);
    }

    public void PlayPitched(AudioClip clip, float pitch)
    {
        soundSource.pitch = pitch;
        soundSource.PlayOneShot(clip);
    }
}
