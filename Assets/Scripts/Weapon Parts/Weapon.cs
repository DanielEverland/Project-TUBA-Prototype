using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains all data for a given weapon
/// </summary>
public class Weapon : IEquatable<Weapon> {

    public Weapon()
    {
        _guid = new Guid();
    }

    private Guid _guid;
    
    public TriggerData TriggerData { get; set; }
    public EventData EventData { get; set; }
    public SeekerData SeekerData { get; set; }

    public bool Equals(Weapon other)
    {
        return other._guid == this._guid;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if(obj is Weapon)
        {
            return Equals(obj as Weapon);
        }

        return false;
    }
    public override int GetHashCode()
    {
        return _guid.GetHashCode();
    }
    public override string ToString()
    {
        return base.ToString();
    }
}
