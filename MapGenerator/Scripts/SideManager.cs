using System;
using System.Collections.Generic;
using System.Diagnostics;
using Random = UnityEngine.Random;

public class SideManager
{
    /*          90
     *           ^
     *           |
     *           |
     * 180 <------------> 0 
     *           |
     *           |
     *           v
     *          270 
     */
    public const string Top = "TOP";
    public const string Right = "RIGHT";
    public const string Down = "DOWN";
    public const string Left = "LEFT";

    private static readonly Dictionary<string, int> SideAngles = new Dictionary<string, int>(4)
    {
        {Right, 0},
        {Top, 90},
        {Left, 180},
        {Down, 270}
    };

    public static float GetRotation(string side, TileMapPart mapPart)
    {
        var sideAngle = SideAngles[side];

        var newObjectAngels = GetAngles(mapPart);

        var random = Random.Range(0, newObjectAngels.Count - 1);

        var randomAngle = newObjectAngels[random];

        return -(180 - (randomAngle - sideAngle));
    }

    private static List<float> GetAngles(TileMapPart newObjectBorder)
    {
        List<float> angles = new List<float>();

        if (newObjectBorder.Top)
        {
            angles.Add(SideAngles[Top]);
        }

        if (newObjectBorder.Right)
        {
            angles.Add(SideAngles[Right]);
        }

        if (newObjectBorder.Down)
        {
            angles.Add(SideAngles[Down]);
        }

        if (newObjectBorder.Left)
        {
            angles.Add(SideAngles[Left]);
        }

        return angles;
    }
}