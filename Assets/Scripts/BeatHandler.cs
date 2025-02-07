using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatHandler : MonoBehaviour
{
    [SerializeField] private float _tolerance = 0.25f;
    
    private RhythmController _rhythmController;
    private InputAction _beatAction;
    private float _lastInputTime;

    private void Awake()
    {
        _beatAction = InputSystem.actions.FindAction("Jump");
    }

    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeat += OnBeat;
    }

    private void OnBeat()
    {
        var timeDiff = Time.time - _lastInputTime;
        var success = timeDiff <= _tolerance;
        _rhythmController.CheckBeat(success);
    }

    private void Update()
    {
        if (_beatAction.WasPressedThisFrame())
        {
            _lastInputTime = Time.time;
        }
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeat -= OnBeat;
        }
    }
}
