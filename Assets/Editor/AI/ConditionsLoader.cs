using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Type = System.Type;

public static class ConditionsLoader
{
    public static List<Type> AllTypes
    {
        get
        {
            if(_allTypes == null)
                LoadTypes();

            return _allTypes;
        }
    }
    private static List<Type> _allTypes;

    private static void LoadTypes()
    {
        _allTypes = typeof(AICondition).Assembly.GetTypes()
            .Where(x => typeof(AICondition).IsAssignableFrom(x) && !x.IsAbstract)
            .ToList();
    }
}