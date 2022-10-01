using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    private ParticleSystem ps;
    private ParticleSystem.MainModule main;

    private bool zooming = false;
    private bool moving = false;

    [SerializeField] private float audioDelay;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
        main.simulationSpeed = 0;
        Invoke("StartZoomSound", 1.3f);
        Invoke("StartZoom", 1.5f);
        Invoke("ShakeScreen", 2f);
    }

    void StartZoomSound()
    {
        audioSource.Play();
    }

    void ShakeScreen()
    {
        GameManager.instance.cameraBehaviour.Shake(20f);
    }

    void StartZoom()
    {
        moving = true;
        zooming = true;
        main.simulationSpeed = 10f;
        Invoke("StopZoom", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            main.simulationSpeed = Mathf.Lerp(main.simulationSpeed, zooming ? 10f : 1f, 1.5f * Time.deltaTime);
        }
    }

    void StopZoom()
    {
        zooming = false;
    }
}
