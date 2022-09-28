using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LuminColor
{
    BLUE,
    GREEN,
    YELLOW,
    RED,
    NONE
}

public class Lumin : MonoBehaviour
{
    public LuminColor color;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.color = Colors.GetColorFromEnum(color);
    }
}
