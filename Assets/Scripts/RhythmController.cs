using System;
using System.Collections;
using UnityEngine;

public enum BeatResultType
{
    None,
    Success,
    Failure
}

public class RhythmController : MonoBehaviour
{
    [SerializeField] private float _beatTime;
    [SerializeField] private float _tolerance = 0.25f;
    
    public float BeatTime => _beatTime;

    public event Action OnActivate;
    public event Action OnBeat;
    public event Action<BeatResultType> OnBeatResult;

    private float _timer;
    private bool _isActive;
    private float _lastInputTime;
    private bool _waitingForInput = true;

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
        
        var beatResultType = BeatResultType.None;
        
        if (!_waitingForInput)
        {
            var timeDiff = Time.time - _lastInputTime;
            var success = timeDiff <= _tolerance;
            beatResultType = success ? BeatResultType.Success : BeatResultType.Failure;
        }
        
        OnBeatResult?.Invoke(beatResultType);
        
        OnBeat?.Invoke();
        ResetBeat();
    }

    private void ResetBeat()
    {
        _timer = _beatTime;
        _waitingForInput = true;
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
        _waitingForInput = false;
    }

    public void Deactivate()
    {
        _isActive = false;
    }
}
