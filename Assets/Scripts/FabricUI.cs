using TMPro;
using UnityEngine;

public class FabricUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private GameManager _gameManager;
    
    public void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnEnemyDefeated += OnEnemyDefeated;
        
        OnEnemyDefeated();
    }

    private void OnEnemyDefeated()
    {
        _text.text = $"{_gameManager.DefeatedEnemies}/{_gameManager.MaxEnemies}";
    }

    private void OnDestroy()
    {
        if (_gameManager != null)
        {
            _gameManager.OnEnemyDefeated -= OnEnemyDefeated;
        }
    }
}
