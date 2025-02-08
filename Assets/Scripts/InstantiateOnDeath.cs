using UnityEngine;

public class InstantiateOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    
    private Health _health;
    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        Instantiate(_prefab, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.OnDeath -= OnDeath;
        }
    }
}