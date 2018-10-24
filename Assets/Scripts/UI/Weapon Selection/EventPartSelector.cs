using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventPartSelector : PartSelectorBase
{
    protected override IEnumerable<PartBase> AvailableParts { get { return PartLoader.EventData; } }
}