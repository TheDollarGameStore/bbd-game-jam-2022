using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public LuminColor color;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.color = Colors.GetColorFromEnum(color);

        Invoke("DestroySelf", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x != 0f)
        {
            transform.localScale -= Vector3.one * (10f * Time.deltaTime);

            if (transform.localScale.x <= 0f)
            {
                transform.localScale = Vector3.zero;
            }
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
