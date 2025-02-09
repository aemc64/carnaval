using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    
    public int CurrentHealth => _health;

    public event Action OnDamageTaken;
    public event Action OnDeath;

    public void TakeDamage()
    {
        _health = Mathf.Max(_health - 1, 0);
        OnDamageTaken?.Invoke();

        if (_health == 0)
        {
            OnDeath?.Invoke();
        }
    }
}
