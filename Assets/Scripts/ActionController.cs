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
    private Direction _currentDirection;
    
    private void Awake()
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
            if (CanAttack(onBeat))
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

    private bool CanAttack(bool onBeat)
    {
        var target = GetTarget();
        if (target == null)
        {
            return false;
        }
        
        var targetPosition = target.transform.position;
        var isInNextTile = transform.position.IsTargetInNextTile(targetPosition, _currentDirection);
        return isInNextTile;
    }

    private void UpdateCurrentDirection()
    {
        if (_movement == null)
        {
            _currentDirection = Direction.None;
            return;
        }
        
        _currentDirection = _movement.GetDirection();
    }

    protected virtual bool CanMove(bool onBeat)
    {
        return _currentDirection != Direction.None;
    }

    private void Attack()
    {
        Debug.Log($"{ gameObject.name }: Attack");
        
        var target = GetTarget();
        target._health.TakeDamage();
        target.SetAction(ActionType.Attacked);
    }

    private void Move()
    {
        Debug.Log($"{ gameObject.name }: Move");
        
        var movementDirection = GameUtils.Directions[_currentDirection];
        transform.position += movementDirection * GameUtils.TileSize;
    }
}
