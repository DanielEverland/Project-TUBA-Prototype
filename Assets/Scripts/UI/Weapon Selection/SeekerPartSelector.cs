using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekerPartSelector : PartSelectorBase
{
    protected override IEnumerable<PartBase> AvailableParts => PartLoader.SeekerData;
}