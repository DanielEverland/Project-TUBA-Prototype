using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthPostProcessor : MonoBehaviour
{
    public abstract float ProcessMaxHealth(float maxHealth);
}