using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Type = System.Type;

public static class PatternLoader
{
    public static List<Type> Patterns
    {
        get
        {
            if (_patterns == null)
                LoadPatterns();

            return _patterns;
        }
    }
    private static List<Type> _patterns;

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

    private static void LoadPatterns()
    {
        _patterns = new List<Type>(typeof(PatternComponent).Assembly.GetTypes()
            .Where(x => typeof(PatternComponent).IsAssignableFrom(x)
                && !x.IsAbstract
                && x != typeof(PatternComponent)));
    }
    private static void LoadDrawers()
    {
        _drawers = new Dictionary<Type, PropertyDrawer>();

        IEnumerable<PropertyDrawer> allDrawers = typeof(PatternLoader).Assembly.GetTypes()
            .Where(x => typeof(PropertyDrawer).IsAssignableFrom(x))
            .Select(x => System.Activator.CreateInstance(x) as PropertyDrawer);
        
        foreach (PropertyDrawer drawer in allDrawers)
        {
            object[] attributes = drawer.GetType().GetCustomAttributes(typeof(CustomPropertyDrawer), false);

            if(attributes.Length != 0)
            {
                CustomPropertyDrawer drawerAttribute = attributes[0] as CustomPropertyDrawer;
                Type targetType = (Type)typeof(CustomPropertyDrawer).GetField("m_Type", FieldFlags).GetValue(drawerAttribute);
                
                if(typeof(PatternComponent).IsAssignableFrom(targetType) && targetType.IsAbstract == false)
                {
                    _drawers.Add(targetType, drawer);
                }
            }
        }        
    }
}
