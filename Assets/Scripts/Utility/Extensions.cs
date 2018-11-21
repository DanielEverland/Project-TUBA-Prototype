using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.Random;

public static class Extensions
{
    public static float RoundToNearest(this float value, float nearest)
    {
        return Mathf.Round(value / nearest) * nearest;
    }
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException(System.String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])System.Enum.GetValues(src.GetType());
        int j = System.Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }
    /// <summary>
    /// Like clamp, but will wrap around instead of truncating
    /// </summary>
    public static float Wrap(this float value, float min, float max)
    {
        if (value > max)
            return min + (value - max) - 1;
        else if (value < min)
            return max - (min - value) + 1;

        return value;
    }
    /// <summary>
    /// Like clamp, but will wrap around instead of truncating
    /// </summary>
    public static int Wrap(this int value, int min, int max)
    {
        if (value > max)
            return min + (value - max) - 1;
        else if (value < min)
            return max - (min - value) + 1;

        return value;
    }
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        return new List<T>(enumerable).Random();
    }
    public static T Random<T>(this IList<T> list)
    {
        return list[Range(0, list.Count)];
    }
}