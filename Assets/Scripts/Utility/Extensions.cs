using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        return new List<T>(enumerable).Random();
    }
    public static T Random<T>(this IList<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}