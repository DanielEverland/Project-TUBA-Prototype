using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger.asset", menuName = MENU_ROOT + "Trigger", order = MENU_ORDER)]
public class TriggerData : PartBase {

    public float Cooldown { get { return _cooldown; } }
    public bool UseCharge { get { return _useCharge; } }
    public float Power { get { return _power; } }
    public int AmmoCapacity { get { return _ammoCapacity; } }
    public float ReloadTime { get { return _reloadTime; } }
    public int SeekersToFire { get { return _seekersToFire; } }
    public float ChargeTime
    {
        get
        {
            if (!UseCharge)
                return 0;

            return _chargeTime;
        }
    }

    [SerializeField]
    private bool _useCharge = false;
    [SerializeField]
    private float _chargeTime = 0;
    [SerializeField]
    private float _cooldown = 0.3f;
    [SerializeField]
    private float _power = 10;
    [SerializeField]
    private int _ammoCapacity = 10;
    [SerializeField]
    private float _reloadTime = 1;
    [SerializeField]
    private List<Muzzle> _muzzles;
    [SerializeField]
    private bool _randomMuzzle = false;
    [SerializeField]
    private int _seekersToFire = 1;

    private void Reset()
    {
        if(_muzzles == null)
            _muzzles = new List<Muzzle>();
        
        if(_muzzles.Count == 0)
        {
            _muzzles.Add(new Muzzle());
        }
    }

    [System.Serializable]
    public class Muzzle : System.IEquatable<Muzzle>
    {
        [SerializeField]
        private Vector3 _direction = Vector3.right;
        [SerializeField]
        private Space _space = Space.Self;
        [SerializeField]
        private float _fov = 0;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if(obj is Muzzle)
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
                other._direction == this._direction &&
                other._space == this._space &&
                other._fov == this._fov;
        }
        public override int GetHashCode()
        {
            int i = 17;

            i += _direction.GetHashCode() * 13;
            i += _space.GetHashCode() * 13;
            i += _fov.GetHashCode() * 13;

            return i;
        }
        public override string ToString()
        {
            return string.Format("({0})\n{1} - ({2})", _direction, _space, _fov);
        }
    }
}
