using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternBehaviourExecutor : MonoBehaviour
{
    private PatternBehaviour _behaviour;

    public void Initialize(PatternBehaviour behaviour, PatternObject obj)
    {
        _behaviour = behaviour;
        _behaviour.Pattern = obj;
    }
    private void Update()
    {
        _behaviour.Update();
    }
}
