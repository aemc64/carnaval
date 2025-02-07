using System;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    [SerializeField] private float _beatTime;

    public event Action OnActivate;
    public event Action OnBeat;

    private float _timer;
    private bool _isActive;
    
    public void Update()
    {
        if (!_isActive)
        {
            return;
        }

        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            return;
        }
        
        OnBeat?.Invoke();
        ResetBeat();
    }

    private void ResetBeat()
    {
        _timer = _beatTime;
    }

    public void Activate()
    {
        OnActivate?.Invoke();
        ResetBeat();
    }

    public void CheckBeat(bool onBeat)
    {
        
    }
}
