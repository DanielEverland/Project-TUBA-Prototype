using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PropertyDrawerLoader
{
    public static Dictionary<Type, PropertyDrawer> Drawers
    {
        get
        {
            if (_drawers == null)
                LoadDrawers();

            return _drawers;
        }
    }
    private static Dictionary<Type, PropertyDrawer> _drawers;

    private static readonly BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.NonPublic;

    private static void LoadDrawers()
    {
        _drawers = new Dictionary<Type, PropertyDrawer>();

        IEnumerable<PropertyDrawer> allDrawers = typeof(PatternLoader).Assembly.GetTypes()
            .Where(x => typeof(PropertyDrawer).IsAssignableFrom(x))
            .Select(x => Activator.CreateInstance(x) as PropertyDrawer);

        foreach (PropertyDrawer drawer in allDrawers)
        {
            object[] attributes = drawer.GetType().GetCustomAttributes(typeof(CustomPropertyDrawer), false);

            if (attributes.Length != 0)
            {
                CustomPropertyDrawer drawerAttribute = attributes[0] as CustomPropertyDrawer;
                Type targetType = (Type)typeof(CustomPropertyDrawer).GetField("m_Type", FieldFlags).GetValue(drawerAttribute);

                if (targetType.IsAbstract == false)
                {
                    _drawers.Add(targetType, drawer);
                }
            }
        }
    }
}
