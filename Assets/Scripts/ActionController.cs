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
    private const float MovementDuration = 0.25f;
    
    private Health _health;
    private IMovement _movement;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    private ActionType _currentAction;
    private ActionController _attackTarget;
    
    protected Direction CurrentDirection { get; private set; }
    public Vector3 ActualPosition { get; private set; }
    
    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<IMovement>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        ActualPosition = transform.position;
    }

    public void DoAction(bool onBeat)
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
        _attackTarget._health.TakeDamage();

        var isTargetDead = _attackTarget._health.CurrentHealth == 0;
        _attackTarget._animator.SetTrigger(isTargetDead ? "Dead" : "Hurt");
        _attackTarget.SetAction(ActionType.Attacked);

        if (isTargetDead)
        {
            _attackTarget.enabled = false;
            _attackTarget.gameObject.layer = LayerMask.NameToLayer("Wall");
        }
        
        _animator.SetTrigger("Attack");
        
    }

    private void Move()
    {
        var movementDirection = GameUtils.Directions[CurrentDirection];
        var targetPosition = ActualPosition + movementDirection * GameUtils.TileSize;
        ActualPosition = targetPosition;

        transform.DOMove(ActualPosition, MovementDuration).SetEase(Ease.Linear);
        _animator.SetTrigger("Move");
    }
}
