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
    
    private ActionType _currentAction;
    private ActionController _attackTarget;
    
    protected Direction CurrentDirection { get; private set; }
    public Vector3 ActualPosition { get; private set; }
    
    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<IMovement>();
        
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

    private void Attack()
    {
        //Debug.Log($"{ gameObject.name }: Attacking { _attackTarget.gameObject.name }");
        
        _attackTarget._health.TakeDamage();
        _attackTarget.SetAction(ActionType.Attacked);
    }

    private void Move()
    {
        var movementDirection = GameUtils.Directions[CurrentDirection];
        var targetPosition = ActualPosition + movementDirection * GameUtils.TileSize;
        ActualPosition = targetPosition;
        
        //Debug.Log($"{ gameObject.name }: Move to { ActualPosition }");
        
        transform.DOMove(ActualPosition, MovementDuration).SetEase(Ease.Linear);
    }
}
