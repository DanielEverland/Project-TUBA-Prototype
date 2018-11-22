using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static UnityEngine.Random;

public static class Extensions
{
    /// <summary>
    /// Creates a deep copy of <paramref name="obj"/>
    /// </summary>
    public static T DeepCopy<T>(this T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
    /// <summary>
    /// Instantiates all entries in a list
    /// </summary>
    public static void Instantiate<T>(this List<T> list) where T : Object
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = Object.Instantiate(list[i]);
        }
    }
    /// <summary>
    /// Rounds the value to the nearest <paramref name="nearest"/>
    /// </summary>
    /// <param name="value">The value to round</param>
    public static Vector4 RoundToNearest(this Vector4 vector, float nearest)
    {
        return new Vector4()
        {
            x = vector.x.RoundToNearest(nearest),
            y = vector.y.RoundToNearest(nearest),
            z = vector.z.RoundToNearest(nearest),
            w = vector.w.RoundToNearest(nearest),
        };
    }
    /// <summary>
    /// Rounds the value to the nearest <paramref name="nearest"/>
    /// </summary>
    /// <param name="value">The value to round</param>
    public static Vector3 RoundToNearest(this Vector3 vector, float nearest)
    {
        return new Vector3()
        {
            x = vector.x.RoundToNearest(nearest),
            y = vector.y.RoundToNearest(nearest),
            z = vector.z.RoundToNearest(nearest),
        };
    }
    /// <summary>
    /// Rounds the value to the nearest <paramref name="nearest"/>
    /// </summary>
    /// <param name="value">The value to round</param>
    public static Vector2 RoundToNearest(this Vector2 vector, float nearest)
    {
        return new Vector2()
        {
            x = vector.x.RoundToNearest(nearest),
            y = vector.y.RoundToNearest(nearest),
        };
    }
    /// <summary>
    /// Rounds the value to the nearest <paramref name="nearest"/>
    /// </summary>
    /// <param name="value">The value to round</param>
    public static float RoundToNearest(this float value, float nearest)
    {
        return Mathf.Round(value / nearest) * nearest;
    }
    /// <summary>
    /// Returns the next value from the enum
    /// </summary>
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
    /// <summary>
    /// Returns a random item from the enumerable
    /// </summary>
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        return new List<T>(enumerable).Random();
    }
    /// <summary>
    /// Returns a random item from the list
    /// </summary>
    public static T Random<T>(this IList<T> list)
    {
        return list[Range(0, list.Count)];
    }
}