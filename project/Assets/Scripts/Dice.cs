using UnityEngine;
using System.Collections;

public static class Dice
{
    public static int Roll(params int[] values)
    {
        int result = 0;

        for (int i = 0; i < values.Length; i++)
        {
            result += Random.Range(0, values[i]) + 1; // 1-to-values[i] range
        }

        return result;
    }
}