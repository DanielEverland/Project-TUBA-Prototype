using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpChaseMovementPostProcessor : MonoBehaviour {

    public abstract float ProcessPauseBetweenJumps(float value);
}
