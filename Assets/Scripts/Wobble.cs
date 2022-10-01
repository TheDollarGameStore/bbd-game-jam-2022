using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    private float x;
    private float scale;
    private Vector2 defaultScale = Vector2.zero;
    private float currentAmplitude;

    [SerializeField]
    private float amplitude;

    [SerializeField]
    private float settleSpeed;

    [SerializeField]
    private float wobbleFrequency;

    private void Update()
    {
        if (defaultScale != Vector2.zero)
        {
            if (currentAmplitude > 0)
            {
                scale = currentAmplitude * Mathf.Sin(x * wobbleFrequency);
                x += 7f * Time.deltaTime;
                currentAmplitude -= (settleSpeed * 70f) * Time.deltaTime;
            }
            else
            {
                scale = 0;
                currentAmplitude = 0;
            }

            transform.localScale = defaultScale + new Vector2(scale, scale);
        }
    }

    public void DoTheWobble()
    {
        defaultScale = transform.localScale;
        currentAmplitude = amplitude;
    }
}
