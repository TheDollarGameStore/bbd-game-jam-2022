using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{

    [HideInInspector] public int targetScene;

    [HideInInspector] public bool fadeIn;

    [SerializeField] private AudioClip transitionSound;

    private SpriteRenderer sr;

    private Image img;

    private float alpha;

    [SerializeField] private bool isExplosion;

    [SerializeField] private GameObject bg;

    private void Start()
    {
        bg.SetActive(false);
        //sr = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();

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

                Debug.Log("LOAD");
                SceneManager.LoadScene(targetScene);
                if (!isExplosion)
                {
                    bg.SetActive(true);
                }
                gameObject.SetActive(false);
            }
        }
        else
        {
            alpha -= (isExplosion ? 0.5f : 1f) * Time.deltaTime;

            if (alpha <= 0f)
            {
                Debug.Log("active false");
                gameObject.SetActive(false);
            }
        }

        //sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
    }
}
