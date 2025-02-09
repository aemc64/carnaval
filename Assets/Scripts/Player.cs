using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IMovement
{
    private ActionController _actionController;
    private InputAction _leftAction;
    private InputAction _rightAction;
    private InputAction _downAction;
    private InputAction _upAction;
    
    private LayerMask _wallLayer; // Capa para detectar los muros
    
    public bool PressedAnyKey { get; private set; }

    private void Awake()
    {
        _actionController = GetComponent<ActionController>();
        
        _leftAction = InputSystem.actions.FindAction("Left");
        _rightAction = InputSystem.actions.FindAction("Right");
        _downAction = InputSystem.actions.FindAction("Down");
        _upAction = InputSystem.actions.FindAction("Up");
        
        _wallLayer = 1 << LayerMask.NameToLayer("Wall");
    }

    private RhythmController _rhythmController;
    private Direction _direction;
    
    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeat += OnBeat;
    }

    private void OnBeat()
    {
        _direction = Direction.None;
    }

    private void Update()
    {
        var updateInputTime = false;
        
        // Detectar teclas de movimiento
        if (_upAction.WasPressedThisFrame()) // Tecla W o Flecha arriba
        {
            _direction = Direction.Up;
            updateInputTime = true;
        }
        else if (_downAction.WasPressedThisFrame()) // Tecla S o Flecha abajo
        {
            _direction = Direction.Down;
            updateInputTime = true;
        }
        else if (_leftAction.WasPressedThisFrame()) // Tecla A o Flecha izquierda
        {
            _direction = Direction.Left;
            updateInputTime = true;
        }
        else if (_rightAction.WasPressedThisFrame()) // Tecla D o Flecha derecha
        {
            _direction = Direction.Right;
            updateInputTime = true;
        }

        if (!updateInputTime)
        {
            return;
        }

        PressedAnyKey = true;
        _rhythmController.UpdateLastInputTime();
    }
    
    // Usa un Raycast para detectar muros antes de moverse
    private bool IsWall(Direction direction)
    {
        var moveDirection = GameUtils.Directions[direction];
        var hit = Physics2D.Raycast(_actionController.ActualPosition, moveDirection, GameUtils.TileSize, _wallLayer.value);
        return hit.collider != null; // Si el Raycast detecta un muro, bloquea el movimiento
    }
    
    public Direction GetDirection()
    {
        if (_direction != Direction.None && IsWall(_direction))
        {
            _direction = Direction.None;
        }
        
        return _direction;
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeat -= OnBeat;
        }
    }
}