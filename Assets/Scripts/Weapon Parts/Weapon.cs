using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all data for a given weapon
/// </summary>[
[Serializable]
public class Weapon : ScriptableObject {
    
    public TriggerData TriggerData { get { return _triggerData; } set { _triggerData = value; } }
    public EventData EventData { get { return _eventData; } set { _eventData = value; } }
    public SeekerData SeekerData { get { return _seekerData; } set { _seekerData = value; } }

    public int CurrentAmmo { get; set; }

    [SerializeField]
    private TriggerData _triggerData;
    [SerializeField]
    private EventData _eventData;
    [SerializeField]
    private SeekerData _seekerData;

    public void ChangePart(PartBase part)
    {
        if(part is TriggerData)
        {
            TriggerData = part as TriggerData;

            CurrentAmmo = TriggerData.AmmoCapacity;
        }
        else if(part is EventData)
        {
            EventData = part as EventData;
        }
        else if(part is SeekerData)
        {
            SeekerData = part as SeekerData;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    public void AssignRandomParts()
    {
        ChangePart(PartLoader.TriggerData.Random());
        ChangePart(PartLoader.EventData.Random());
        ChangePart(PartLoader.SeekerData.Random());
    }

    [Serializable]
    public class Muzzle : IEquatable<Muzzle>
    {
        public Space Space { get { return _space; } }
        public float Angle { get { return _angle; } }        
        public float IntervalStart { get { return _intervalStart; } }
        public float IntervalEnd { get { return _intervalEnd; } }

        [SerializeField]
        private float _angle;
        [SerializeField]
        private Space _space = Space.Self;
        [SerializeField]
        private float _intervalStart = 0;
        [SerializeField]
        private float _intervalEnd = 0;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Muzzle)
            {
                return Equals(obj as Muzzle);
            }

            return false;
        }
        public bool Equals(Muzzle other)
        {
            if (other == null)
                return false;

            return
                other.Angle == Angle &&
                other.Space == Space &&
                other.IntervalStart == IntervalStart &&
                other.IntervalEnd == IntervalEnd;
        }
        public override int GetHashCode()
        {
            int i = 17;

            i += Angle.GetHashCode() * 13;
            i += Space.GetHashCode() * 13;
            i += IntervalStart.GetHashCode() * 13;
            i += IntervalEnd.GetHashCode() * 13;

            return i;
        }
        public override string ToString()
        {
            return string.Format("{0} ({1}-{2})\n{3}", _angle, IntervalStart, IntervalEnd, _space);
        }
    }
}
