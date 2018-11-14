using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIComponent
{
    void Think();
}
public abstract class AIComponent : MonoBehaviour, IAIComponent {

    public abstract void Think();
}
