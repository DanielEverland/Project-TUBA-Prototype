using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritualModifier : MonoBehaviour {

    [SerializeField]
    private BoolReference _disableColliders = new BoolReference(true);
    [SerializeField]
    private BoolReference _makeTransparent = new BoolReference(true);
    [SerializeField]
    private List<Renderer> _renderers;
    [SerializeField]
    private List<Collider2D> _colliders;
    
    public SpiritualState CurrentState { get; set; }

    protected virtual float SpiritualAlpha => 0.5f;

    protected bool ShouldDisableColliders => _disableColliders.Value;
    protected bool ShouldMakeTransparent => _makeTransparent.Value;
    protected List<Renderer> Renderers => _renderers;
    protected List<Collider2D> Colliders => _colliders;

	public virtual void Switch()
    {
        CurrentState = CurrentState.Next();

        PollColliders();
        PollRenderers();
    }
    protected virtual void PollColliders()
    {
        foreach (Collider2D collider in Colliders)
        {
            collider.enabled = CurrentState == SpiritualState.Corpereal;
        }
    }
    protected virtual void PollRenderers()
    {
        foreach (Renderer renderer in Renderers)
        {
            foreach (Material material in renderer.materials)
            {
                Color materialColor = material.color;
                materialColor.a = GetAlpha();
                material.color = materialColor;
            }
        }
    }
    protected virtual float GetAlpha()
    {
        return CurrentState == SpiritualState.Corpereal ? 1 : SpiritualAlpha;
    }    
}
public enum SpiritualState
{
    Corpereal = 0,
    Spiritual = 1,
}