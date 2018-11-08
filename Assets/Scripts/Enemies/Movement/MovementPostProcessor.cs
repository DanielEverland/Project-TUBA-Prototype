using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementPostProcessor : MonoBehaviour {

    public abstract float ProcessMovementSpeed(float movementSpeed);
}
