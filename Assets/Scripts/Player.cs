using System;
using UnityEngine;

public class Player : MonoBehaviour, IMovement
{
    private ActionController _actionController;
    
    private LayerMask _wallLayer; // Capa para detectar los muros

    private void Awake()
    {
        _actionController = GetComponent<ActionController>();
        
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
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // Tecla W o Flecha arriba
        {
            _direction = Direction.Up;
            updateInputTime = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // Tecla S o Flecha abajo
        {
            _direction = Direction.Down;
            updateInputTime = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) // Tecla A o Flecha izquierda
        {
            _direction = Direction.Left;
            updateInputTime = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) // Tecla D o Flecha derecha
        {
            _direction = Direction.Right;
            updateInputTime = true;
        }

        if (!updateInputTime)
        {
            return;
        }
        
        _rhythmController.UpdateLastInputTime();
            
        if (IsWall(_direction))
        {
            _direction = Direction.None;
        }
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