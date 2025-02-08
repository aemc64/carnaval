using System.Collections.Generic;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{
    [SerializeField] private int _requiredAmount = 1;
    [SerializeField] private List<RuntimeAnimatorController> _animatorControllers;
    
    private GameManager _gameManager;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnEnemyDefeated += OnEnemyDefeated;
    }

    private void OnEnemyDefeated()
    {
        var defeatedEnemies = _gameManager.DefeatedEnemies;
        var currentAnimatorIndex = defeatedEnemies / _requiredAmount;
        _animator.runtimeAnimatorController = _animatorControllers[currentAnimatorIndex];
    }

    private void OnDestroy()
    {
        if (_gameManager != null)
        {
            _gameManager.OnEnemyDefeated -= OnEnemyDefeated;
        }
    }
}
