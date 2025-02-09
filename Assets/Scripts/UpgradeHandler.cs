using System.Collections.Generic;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{
    [SerializeField] private int _requiredAmount = 1;
    [SerializeField] private List<RuntimeAnimatorController> _animatorControllers;
    [SerializeField] private ParticleSystem _particles;
    
    private GameManager _gameManager;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
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

        var currentAnimatorController = _animator.runtimeAnimatorController;
        var newAnimatorController = _animatorControllers[currentAnimatorIndex];
        _animator.runtimeAnimatorController = newAnimatorController;
        
        if (newAnimatorController != currentAnimatorController)
        {
            _particles.Play();
        }
    }

    private void OnDestroy()
    {
        if (_gameManager != null)
        {
            _gameManager.OnEnemyDefeated -= OnEnemyDefeated;
        }
    }
}
