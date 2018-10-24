using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventPartSelector : PartSelectorBase
{
    protected override void OnSelectionChanged(int index)
    {
        Owner.ChangePart(PartLoader.EventData.ElementAt(index));
    }
}