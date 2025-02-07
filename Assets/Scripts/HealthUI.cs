using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _healthContainer;

    private void Awake()
    {
        _health.OnHealthChanged += OnHealthChanged;
        UpdateHealth();
    }

    private void OnHealthChanged()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        var currentHealth = _health.CurrentHealth;
        for (var i = 0; i < _healthContainer.childCount; i++)
        {
            var child = _healthContainer.GetChild(i);
            child.gameObject.SetActive(i < currentHealth);
        }
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnHealthChanged -= OnHealthChanged;
        }
    }
}
