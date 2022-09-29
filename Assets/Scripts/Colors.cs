using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colors
{
    // Start is called before the first frame update
    public static Color32 blue = new Color32(18, 18, 255, 255);
    public static Color32 red = new Color32(255, 35, 203, 255);
    public static Color32 yellow = new Color32(149, 222, 37, 255);
    public static Color32 green = new Color32(0, 255, 121, 255);

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
