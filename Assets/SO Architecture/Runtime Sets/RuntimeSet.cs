﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSet<T> : ScriptableObject, IEnumerable<T>
{
    [SerializeField]
    private List<T> _items = new List<T>();

    public void Add(T obj)
    {
        if (!_items.Contains(obj))
            _items.Add(obj);
    }
    public void Remove(T obj)
    {
        if (_items.Contains(obj))
            _items.Remove(obj);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();        
    }
    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }
}
