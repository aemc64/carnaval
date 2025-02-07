using System;
using System.Collections;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    [SerializeField] private float _beatTime;
    [SerializeField] private float _tolerance = 0.25f;
    
    public float BeatTime => _beatTime;

    public event Action OnActivate;
    public event Action OnBeat;
    public event Action<bool> OnBeatResult;

    private float _timer;
    private bool _isActive;
    private float _lastInputTime;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Activate();
    }

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
        
        var timeDiff = Time.time - _lastInputTime;
        var success = timeDiff <= _tolerance;
        OnBeatResult?.Invoke(success);
        
        OnBeat?.Invoke();
        ResetBeat();
    }

    private void ResetBeat()
    {
        _timer = _beatTime;
    }

    private void Activate()
    {
        OnActivate?.Invoke();
        _isActive = true;
        ResetBeat();
    }

    public void UpdateLastInputTime()
    {
        _lastInputTime = Time.time;
    }
}
