using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Type = System.Type;

public static class PatternLoader
{
    public static List<Type> Components
    {
        get
        {
            if (_components == null)
                _components = new List<Type>(LoadOfType<PatternComponent>());

            return _components;
        }
    }
    private static List<Type> _components;

    public static List<Type> Behaviours
    {
        get
        {
            if (_behaviours == null)
                _behaviours = new List<Type>(LoadOfType<PatternBehaviour>());

            return _behaviours;
        }
    }
    private static List<Type> _behaviours;
    
    private static IEnumerable<Type> LoadOfType<T>()
    {
        return typeof(T).Assembly.GetTypes()
            .Where(x => typeof(T).IsAssignableFrom(x)
                && !x.IsAbstract
                && x != typeof(T));
    }
}
