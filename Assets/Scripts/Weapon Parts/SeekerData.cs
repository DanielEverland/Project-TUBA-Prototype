using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seeker.asset", menuName = MENU_ROOT + "Seeker", order = MENU_ORDER)]
public class SeekerData : PartBase
{
    public Color Color { get { return _color; } }

    [SerializeField]
    private Color _color;
}