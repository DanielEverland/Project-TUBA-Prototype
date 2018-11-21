using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Type = System.Type;

public static class ActionLoader
{
    public static List<Type> AllTypes
    {
        get
        {
            if (_allTypes == null)
                LoadTypes();

            return _allTypes;
        }
    }
    private static List<Type> _allTypes;

    private static void LoadTypes()
    {
        _allTypes = typeof(AIStateMachineAction).Assembly.GetTypes()
            .Where(x => typeof(AIStateMachineAction).IsAssignableFrom(x) && !x.IsAbstract)
            .ToList();
    }
}