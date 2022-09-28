using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors
{
    // Start is called before the first frame update
    public static Color32 blue = new Color32(77, 166, 255, 255);
    public static Color32 red = new Color32(235, 86, 75, 255);
    public static Color32 green = new Color32(143, 222, 93, 255);
    public static Color32 yellow = new Color32(255, 255, 120, 255);

    public static Color32 GetColorFromEnum(LuminColor color)
    {
        switch (color)
        {
            case LuminColor.BLUE:
                return Colors.blue;
            case LuminColor.RED:
                return Colors.red;
            case LuminColor.YELLOW:
                return Colors.yellow;
            case LuminColor.GREEN:
                return Colors.green;
            default:
                return Color.black;
        }
    }
}
