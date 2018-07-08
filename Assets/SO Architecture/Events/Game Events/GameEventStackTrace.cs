﻿using System;
using UnityEngine;

#if UNITY_EDITOR
public class GameEventStackTrace : IEquatable<GameEventStackTrace>
{
    private GameEventStackTrace() { }
    private GameEventStackTrace(string trace)
    {
        _id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        _stackTrace = trace;

        if (Application.isPlaying)
        {
            _frameCount = Time.frameCount;
        }
    }

    private readonly int _id;
    private readonly int _frameCount;
    private readonly string _stackTrace;

    public static GameEventStackTrace Create()
    {
        return new GameEventStackTrace(Environment.StackTrace);
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj is GameEventStackTrace)
        {
            return Equals(obj as GameEventStackTrace);
        }

        return false;
    }
    public bool Equals(GameEventStackTrace other)
    {
        return other._id == this._id;
    }
    public override int GetHashCode()
    {
        return _id;
    }
    public override string ToString()
    {
        if (_frameCount > 0)
        {
            return string.Format("{0} {1}", _frameCount, _stackTrace);
        }
        else
        {
            return _stackTrace;
        }
    }

    public static implicit operator string(GameEventStackTrace trace)
    {
        return trace.ToString();
    }
}
#endif