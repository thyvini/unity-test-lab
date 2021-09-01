using UnityEngine;
using System;

public class Pivot
{
    public Pivot(GameObject type, Tuple<int, int> position, int growthRate)
    {
        Type = type;
        Position = position;
        GrowthRate = growthRate;
    }

    public GameObject Type { get; }
    public Tuple<int, int> Position { get; }
    public int GrowthRate { get; }
}
