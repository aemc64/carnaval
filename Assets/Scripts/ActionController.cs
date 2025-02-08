using System;
using DG.Tweening;
using UnityEngine;

public enum ActionType
{
    Idle,
    Move,
    Attack,
    Attacked
}

public class ActionController : MonoBehaviour
{
    [SerializeField] private Color _damageColor = Color.red;
    
    private const float MovementDuration = 0.25f;
    private const float HurtDuration = 0.25f;
    
    private Health _health;
    private IMovement _movement;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    private ActionType _currentAction;
    private ActionController _attackTarget;
    private RhythmController _rhythmController;
    private Color _originalColor;
    
    protected Direction CurrentDirection { get; private set; }
    public Vector3 ActualPosition { get; private set; }
    
    public bool ReflectAttack { get; set; }
    
    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<IMovement>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        
        ActualPosition = transform.position;
    }

    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeat += OnBeat;
    }

    private void OnBeat()
    {
        _currentAction = ActionType.Idle;
    }

    public void DoAction()
    {
        switch (_currentAction)
        {
            case ActionType.Idle:
                Debug.Log($"{ gameObject.name }: Idle");
                break;
            case ActionType.Move:
            {
                Move();
                break;
            }
            case ActionType.Attack:
            {
                Attack();
                break;
            }
            case ActionType.Attacked:
                Debug.Log($"{ gameObject.name }: Attacked");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetAction(ActionType action)
    {
        _currentAction = action;
    }
    
    public void UpdateIntendedAction(bool onBeat)
    {
        if (_currentAction == ActionType.Attacked)
        {
            return;
        }
        
        UpdateCurrentDirection();

        if (CanMove(onBeat))
        {
            ChangeSpriteDirection();
            
            if (CanAttack())
            {
                SetAction(ActionType.Attack);
                return;
            }
            
            SetAction(ActionType.Move);
            return;
        }
        
        SetAction(ActionType.Idle);
    }

    protected virtual ActionController GetTarget()
    {
        return GameManager.Instance.Player;
    }

    private bool CanAttack()
    {
        _attackTarget = GetTarget();
        if (_attackTarget == null)
        {
            return false;
        }
        
        var targetPosition = _attackTarget.ActualPosition;
        var isInNextTile = ActualPosition.IsTargetInNextTile(targetPosition, CurrentDirection);
        return isInNextTile;
    }

    private void UpdateCurrentDirection()
    {
        if (_movement == null)
        {
            CurrentDirection = Direction.Left;
            return;
        }
        
        CurrentDirection = _movement.GetDirection();
    }

    protected virtual bool CanMove(bool onBeat)
    {
        return CurrentDirection != Direction.None;
    }

    private void ChangeSpriteDirection()
    {
        _spriteRenderer.flipX = CurrentDirection switch
        {
            Direction.Left => true,
            Direction.Right => false,
            _ => _spriteRenderer.flipX
        };
    }

    private void Attack()
    {
        if (!ReflectAttack)
        {
            ReceiveDamage(_attackTarget);
        }
        else
        {
            ReceiveDamage(this);
            return;
        }
        
        _animator.SetTrigger("Attack");
    }

    private void ReceiveDamage(ActionController target)
    {
        target._health.TakeDamage();

        var isTargetDead = target._health.CurrentHealth == 0;
        target._animator.SetTrigger(isTargetDead ? "Dead" : "Hurt");
        target.SetAction(ActionType.Attacked);
        
        target._spriteRenderer.color = _damageColor;
        target._spriteRenderer.DOColor(_originalColor, HurtDuration);

        if (!isTargetDead)
        {
            return;
        }
        
        target.enabled = false;
        target.gameObject.layer = LayerMask.NameToLayer("Wall");
    }

    private void Move()
    {
        var movementDirection = GameUtils.Directions[CurrentDirection];
        var targetPosition = ActualPosition + movementDirection * GameUtils.TileSize;
        ActualPosition = targetPosition;

        transform.DOMove(ActualPosition, MovementDuration).SetEase(Ease.Linear);
        _animator.SetTrigger("Move");
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeat -= OnBeat;
        }
    }
}
