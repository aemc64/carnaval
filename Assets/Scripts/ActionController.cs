using System;
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
    private Health _health;
    private IMovement _movement;
    
    private ActionType _currentAction;
    private ActionController _attackTarget;
    
    protected Direction CurrentDirection { get; private set; }
    
    protected virtual void Awake()
    {
        _health = GetComponent<Health>();
        _movement = GetComponent<IMovement>();
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
        
        var targetPosition = _attackTarget.transform.position;
        var isInNextTile = transform.position.IsTargetInNextTile(targetPosition, CurrentDirection);
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
        Debug.Log($"{ gameObject.name }: Attack");
        
        _attackTarget._health.TakeDamage();
        _attackTarget.SetAction(ActionType.Attacked);
    }

    private void Move()
    {
        Debug.Log($"{ gameObject.name }: Move");
        
        var movementDirection = GameUtils.Directions[CurrentDirection];
        transform.position += movementDirection * GameUtils.TileSize;
    }
}
