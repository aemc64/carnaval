using UnityEngine;
using UnityEngine.InputSystem;

public class BeatHandler : MonoBehaviour
{
    private RhythmController _rhythmController;
    private InputAction _beatAction;

    private void Awake()
    {
        _beatAction = InputSystem.actions.FindAction("Jump");
    }

    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
    }

    private void Update()
    {
        if (_beatAction.WasPressedThisFrame())
        {
            _rhythmController.UpdateLastInputTime();
        }
    }
}
