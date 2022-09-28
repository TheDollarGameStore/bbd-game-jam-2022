using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    private LuminColor upColor;
    private LuminColor downColor;
    private LuminColor rightColor;
    private LuminColor leftColor;

    [SerializeField] private GameObject emitterDiodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        PickColors();
    }

    // Update is called once per frame
    void PickColors()
    {
        upColor = (LuminColor)Random.Range(0, 5);
        downColor = (LuminColor)Random.Range(0, 5);
        rightColor = (LuminColor)Random.Range(0, 5);
        leftColor = (LuminColor)Random.Range(0, 5);

        AddDiode(upColor, 0f);
        AddDiode(rightColor, 90f);
        AddDiode(downColor, 180f);
        AddDiode(leftColor, 270f);
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
}
