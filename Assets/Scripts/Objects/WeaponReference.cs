[System.Serializable]
public sealed class WeaponReference : BaseReference<Weapon, WeaponVariable>
{
    public WeaponReference() : base() { }
    public WeaponReference(Weapon value) : base(value) { }
}