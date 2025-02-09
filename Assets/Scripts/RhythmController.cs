using System;
using DG.Tweening;
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
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _beatBar;
    
    public float BeatTime => _beatTime;

    public event Action OnActivate;
    public event Action OnBeat;
    public event Action<BeatResultType> OnBeatResult;

    private float _timer;
    private bool _isActive;
    private float _lastInputTime;
    private bool _waitingForInput = true;

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

    public void Activate()
    {
        _beatBar.SetActive(true);
        _audioSource.Play();
        
        OnActivate?.Invoke();
        _isActive = true;
        ResetBeat();
    }

    public void UpdateLastInputTime()
    {
        if (!_waitingForInput)
        {
            return;
        }
        
        _lastInputTime = Time.time;
        _waitingForInput = false;
    }

    public void Deactivate()
    {
        _isActive = false;
    }
}
