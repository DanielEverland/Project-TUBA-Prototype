using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubdividerElement : MonoBehaviour {

    [SerializeField]
    private IntReference _maxSubdivides;

    public int MaxLevel => _maxSubdivides.Value;
    public int CurrentLevel { get; private set; }

    public void Initialize(int subDivideLevel)
    {
        CurrentLevel = subDivideLevel;
    }
}
