using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekerPartSelector : PartSelectorBase
{
    protected override void OnSelectionChanged(int index)
    {
        Owner.ChangePart(PartLoader.SeekerData.ElementAt(index));
    }
}