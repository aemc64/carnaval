using DG.Tweening;
using UnityEngine;

public class CameraShakeOnDamage : MonoBehaviour
{
    private Health _health;
    private Camera _mainCamera;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDamageTaken += OnDamageTaken;
        
        _mainCamera = Camera.main;
    }

    private void OnDamageTaken()
    {
        _mainCamera.DOShakePosition(0.25f,  0.5f, 20);
    }
}
