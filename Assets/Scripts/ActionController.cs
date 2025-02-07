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
    private ActionType _currentAction;
    private Direction _currentDirection;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    public void DoAction(bool onBeat)
    {
        switch (_currentAction)
        {
            case ActionType.Idle:
                Debug.Log($"{ gameObject.name }: Idle");
                break;
            case ActionType.Move:
                Debug.Log($"{ gameObject.name }: Move");
                break;
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
    
    public void UpdateIntendedAction()
    {
        UpdateCurrentDirection();

        if (CanMove())
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
        if (!CanMove())
        {
            _currentDirection = Direction.None;
            return;
        }

        _currentDirection = Direction.Left;
    }

    private bool CanMove()
    {
        return true;
    }

    private void Attack()
    {
        Debug.Log($"{ gameObject.name }: Attack");
        
        var target = GetTarget();
        target._health.TakeDamage();
        target.SetAction(ActionType.Attacked);
    }
}
