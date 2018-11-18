using UnityEngine;

public abstract class AIMover : AIComponent
{
    [SerializeField]
    private GameObjectReference _player;
    [SerializeField]
    private CharacterController2D _characterController;
    [SerializeField]
    private FloatReference _movementSpeed = new FloatReference(10);
    [SerializeField, HideInInspector]
    private MovementPostProcessor _postProcessor;
    
    protected virtual Color PlayerLineColor => new Color(1, 0, 0, 0.1f);

    protected Vector3 PlayerPosition { get; set; }
    protected Vector3 PlayerDirection { get; set; }
    protected float DistanceToPlayer => Vector3.Distance(transform.position, PlayerPosition);
    protected GameObject Player => _player.Value;
    protected CharacterController2D CharacterController => _characterController;
    protected float MovementSpeed => GetMovementSpeed();

    protected virtual void Update()
    {
        DrawDebug();
    }
    public override void Think()
    {
        PlayerPosition = Player.transform.position;
        PlayerDirection = (Player.transform.position - transform.position).normalized;
    }
    private float GetMovementSpeed()
    {
        if (_postProcessor != null)
            return _postProcessor.ProcessMovementSpeed(_movementSpeed.Value);

        return _movementSpeed.Value;
    }
    protected virtual void OnValidate()
    {
        _postProcessor = GetComponent<MovementPostProcessor>();

        if (_characterController == null)
            _characterController = GetComponent<CharacterController2D>();
    }
    protected virtual void MoveTo(Vector2 position)
    {
        MoveTo(position, MovementSpeed * Time.deltaTime);
    }
    protected virtual void MoveTo(Vector2 position, float speed)
    {
        Vector2 delta = position - (Vector2)transform.position;

        if(delta.magnitude < speed)
        {
            Move(delta.normalized, delta.magnitude);
        }
        else
        {
            Move(delta.normalized, speed);
        }
    }
    protected virtual void DrawDebug() => Debug.DrawLine(transform.position, Player.transform.position, PlayerLineColor);
    protected virtual void Move(Vector2 direction) => Move(direction.normalized, MovementSpeed * Time.deltaTime);
    protected virtual void Move(Vector2 direction, float speed) => CharacterController.Move(direction * speed);
}
