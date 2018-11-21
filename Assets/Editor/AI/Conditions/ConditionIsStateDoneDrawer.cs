using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ConditionIsStateDone))]
public class ConditionIsStateDoneDrawer : ReorderableListElementPropertyDrawer
{
    protected override List<string> ElementNames => new List<string>()
    {
        "_targetState",
    };
}
