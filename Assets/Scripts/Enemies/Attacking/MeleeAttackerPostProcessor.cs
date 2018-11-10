using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeAttackerPostProcessor : MonoBehaviour {

    public abstract float ProcessAttackForce(float value);
}
