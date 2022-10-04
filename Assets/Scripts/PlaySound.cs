using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private bool randomized;

    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        if (randomized)
        {
            SoundManager.Instance.PlayRandomized(audioClip);
        }
        else
        {
            SoundManager.Instance.PlayPitched(audioClip, 1f);
        }
    }
}