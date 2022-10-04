using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{

    [HideInInspector] public int targetScene;

    [HideInInspector] public bool fadeIn;

    [SerializeField] private AudioClip transitionSound;

    private SpriteRenderer sr;

    private float alpha;

    [SerializeField] private bool isExplosion;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (fadeIn)
        {
            alpha = 0f;
            SoundManager.Instance.PlayRandomized(transitionSound);
        }
        else
        {
            alpha = 1f;
        }

        transform.localScale = Vector3.one * 200f;
    }
    // Start is called before the first frame update
    void Update()
    {
        if (fadeIn)
        {
            alpha += 1f * Time.deltaTime;

            if (alpha >= 1f)
            {
                SceneManager.LoadScene(targetScene);
            }
        }
        else
        {
            alpha -= (isExplosion ? 0.5f : 1f) * Time.deltaTime;

            if (alpha <= 0f)
            {
                Destroy(gameObject);
            }
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }
}
