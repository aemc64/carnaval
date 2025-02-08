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
    }

    private void OnBeatResult(bool onBeat)
    {
        _player.UpdateIntendedAction(onBeat);

        foreach (var enemy in _enemyControllers)
        {
            enemy.UpdateIntendedAction(onBeat);
        }
        
        _player.DoAction(onBeat);
        
        foreach (var enemy in _enemyControllers)
        {
            enemy.DoAction(onBeat);
        }

        for (var i = _enemyControllers.Count - 1; i >= 0; i--)
        {
            if (!_enemyControllers[i].enabled)
            {
                _enemyControllers.RemoveAt(i);
            }
        }
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeatResult -= OnBeatResult;
        }
    }
}
