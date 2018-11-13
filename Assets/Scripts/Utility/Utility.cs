using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Will return the max value of the current segment
    /// </summary>
    /// <param name="currentValue">Current value in total, not in relation to segment</param>
    /// <param name="valuePerSegment">How many values does a segment hold?</param>
    /// <returns>The max value of the current segment</returns>
    public static float GetMaxValueForSegment(float currentValue, float valuePerSegment)
    {
        int currentSegment = Mathf.CeilToInt(currentValue / valuePerSegment);
        return valuePerSegment * currentSegment;
    }
}
