using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{

    [HideInInspector] public int targetScene;

    [HideInInspector] public bool fadeIn;

    private SpriteRenderer sr;

    private float alpha;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (fadeIn)
        {
            alpha = 0f;
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
            alpha -= 1f * Time.deltaTime;

            if (alpha <= 0f)
            {
                Destroy(gameObject);
            }
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }
}
