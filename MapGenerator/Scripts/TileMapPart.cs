
using UnityEngine;

public class TileMapPart : MonoBehaviour
{
    public bool Top;
    public bool Right;
    public bool Down;
    public bool Left;

    [HideInInspector]
    public Int2? CurrentPosition;
}