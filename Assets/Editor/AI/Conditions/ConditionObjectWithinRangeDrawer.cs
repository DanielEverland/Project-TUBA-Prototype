using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionObjectWithinRange))]
public class ConditionObjectWithinRangeDrawer : ReorderableListElementPropertyDrawer
{
    protected override List<string> ElementNames => new List<string>()
    {
        "_targetObject",
        "_distance",
    };
}
