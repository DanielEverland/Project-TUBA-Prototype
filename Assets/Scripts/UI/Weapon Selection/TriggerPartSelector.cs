using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerPartSelector : PartSelectorBase
{
    protected override void OnSelectionChanged(int index)
    {
        Owner.ChangePart(PartLoader.TriggerData.ElementAt(index));
    }
}
