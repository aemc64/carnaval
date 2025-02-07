using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private RhythmController _rhythmController;
    [SerializeField] private ActionController _player;
    [SerializeField] private Transform _enemiesParent;

    private ActionController[] _enemyControllers;
    
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
        
        _enemyControllers = _enemiesParent.GetComponentsInChildren<ActionController>();
    }

    private void OnBeatResult(bool onBeat)
    {
        _player.UpdateIntendedAction();

        foreach (var enemy in _enemyControllers)
        {
            enemy.UpdateIntendedAction();
        }
        
        _player.DoAction(onBeat);
        
        foreach (var enemy in _enemyControllers)
        {
            enemy.DoAction(onBeat);
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
