using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private RhythmController _rhythmController;
    [SerializeField] private ActionController _player;
    [SerializeField] private Transform _enemiesParent;

    private List<ActionController> _enemyControllers;
    
    public RhythmController RhythmController => _rhythmController;
    public ActionController Player => _player;

    public event Action OnEnemyDefeated;
    
    public int DefeatedEnemies { get; private set; }
    public int MaxEnemies { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        _rhythmController.OnBeatResult += OnBeatResult;
        
        _enemyControllers = new List<ActionController>(_enemiesParent.GetComponentsInChildren<ActionController>());
        MaxEnemies = _enemyControllers.Count;
    }

    private void OnBeatResult(bool onBeat)
    {
        _player.UpdateIntendedAction(onBeat);
        _player.DoAction();

        foreach (var enemy in _enemyControllers)
        {
            enemy.UpdateIntendedAction(onBeat);
            enemy.DoAction();
        }

        if (!_player.enabled)
        {
            GameOver(false);
            return;
        }

        for (var i = _enemyControllers.Count - 1; i >= 0; i--)
        {
            if (!_enemyControllers[i].enabled)
            {
                _enemyControllers.RemoveAt(i);
                DefeatedEnemies++;
                
                OnEnemyDefeated?.Invoke();
            }
        }

        if (DefeatedEnemies == MaxEnemies)
        {
            GameOver(true);
        }
    }

    private void GameOver(bool playerWon)
    {
        _rhythmController.Deactivate();
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeatResult -= OnBeatResult;
        }
    }
}
