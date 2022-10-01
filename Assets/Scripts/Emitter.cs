using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    [HideInInspector] public LuminColor upColor = LuminColor.NONE;
    [HideInInspector] public LuminColor downColor = LuminColor.NONE;
    [HideInInspector] public LuminColor rightColor = LuminColor.NONE;
    [HideInInspector] public LuminColor leftColor = LuminColor.NONE;

    [SerializeField] private GameObject blueLuminPrefab;
    [SerializeField] private GameObject redLuminPrefab;
    [SerializeField] private GameObject yellowLuminPrefab;
    [SerializeField] private GameObject greenLuminPrefab;

    [SerializeField] private GameObject emitterDiodePrefab;

    [SerializeField] private AudioClip placeSound;

    private bool shrinking = false;

    private Wobble wobbler;

    // Start is called before the first frame update
    void Start()
    {
        wobbler = GetComponent<Wobble>();
        PickColors();
    }

    // Update is called once per frame
    void PickColors()
    {
        
        upColor = PickColor();
        downColor = PickColor();
        rightColor = PickColor();
        leftColor = PickColor();

        //Ensure that not all diodes are empty
        if (upColor == LuminColor.NONE && downColor == LuminColor.NONE && leftColor == LuminColor.NONE && rightColor == LuminColor.NONE)
        {
            PickColors();
        }
        else
        {
            AddDiode(upColor, 0f);
            AddDiode(rightColor, 270f);
            AddDiode(downColor, 180f);
            AddDiode(leftColor, 90f);
        }
    }

    LuminColor PickColor()
    {
        if (Random.Range(0, 2) == 0)
        {
            return (LuminColor)Random.Range(0, 4);
        }

        return LuminColor.NONE;
    }

    void AddDiode(LuminColor color, float angle)
    {
        if (color == LuminColor.NONE)
        {
            return;
        }
        GameObject newDiode = Instantiate(emitterDiodePrefab, transform.position, Quaternion.identity, transform);
        newDiode.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        newDiode.GetComponent<SpriteRenderer>().color = Colors.GetColorFromEnum(color);
    }

    public void Placed(int y, int x)
    {
        LaunchLumins(y, x);
        Invoke("Wobble", 0.01f);
        SoundManager.Instance.PlayRandomized(placeSound);
        Invoke("Shrink", 1f);
    }

    private void Wobble()
    {
        wobbler.DoTheWobble();
    }

    private void Update()
    {
        if (shrinking)
        {
            transform.localScale -= Vector3.one * (10f * Time.deltaTime);

            if (transform.localScale.x <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void LaunchLumins(int y, int x)
    {
        if (upColor != LuminColor.NONE)
        {
            PlaceLumin(y + 1, x, upColor);
        }

        if (downColor != LuminColor.NONE)
        {
            PlaceLumin(y - 1, x, downColor);
        }

        if (leftColor != LuminColor.NONE)
        {
            PlaceLumin(y, x - 1, leftColor);
        }

        if (rightColor != LuminColor.NONE)
        {
            PlaceLumin(y, x + 1, rightColor);
        }
    }

    private void PlaceLumin(int y, int x, LuminColor color)
    {
        Lumin newLumin = null;

        switch (color)
        {
            case LuminColor.RED:
                newLumin = Instantiate(redLuminPrefab, transform.position, Quaternion.identity).GetComponent<Lumin>();
                break;
            case LuminColor.YELLOW:
                newLumin = Instantiate(yellowLuminPrefab, transform.position, Quaternion.identity).GetComponent<Lumin>();
                break;
            case LuminColor.GREEN:
                newLumin = Instantiate(greenLuminPrefab, transform.position, Quaternion.identity).GetComponent<Lumin>();
                break;
            case LuminColor.BLUE:
                newLumin = Instantiate(blueLuminPrefab, transform.position, Quaternion.identity).GetComponent<Lumin>();
                break;
        }

        GameManager.instance.tiles[y, x].lumin = newLumin;
    }

    void Shrink()
    {
        Destroy(wobbler);
        shrinking = true;
    }
}
