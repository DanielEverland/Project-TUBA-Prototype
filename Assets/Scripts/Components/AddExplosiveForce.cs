using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddExplosiveForce : MonoBehaviour {

    [SerializeField]
    private FloatReference _radius = new FloatReference(10);
    [SerializeField]
    private FloatReference _force = new FloatReference(50);
    [SerializeField]
    private ForceMode2D _forceMode = ForceMode2D.Impulse;
    [SerializeField]
    private LayerMask _ignoreLayer;

    protected LayerMask IgnoreLayer => _ignoreLayer;
    protected ForceMode2D ForceMode => _forceMode;
    protected float Radius => _radius.Value;
    protected float Force => _force.Value;

    public void AddForce()
    {
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, Radius, Vector2.zero))
        {
            if (hit.rigidbody == null || IgnoreLayer.value == (IgnoreLayer.value | 1 << hit.collider.gameObject.layer))
                continue;

            Vector2 delta = hit.transform.position - transform.position;            
            CharacterController2D charController = hit.rigidbody.gameObject.GetComponent<CharacterController2D>();

            if(charController != null)
            {
                charController.AddForce(delta.normalized * Force, ForceMode);
            }
            else
            {
                hit.rigidbody.AddForce(delta.normalized * Force, ForceMode);
            }
        }
    }
}
