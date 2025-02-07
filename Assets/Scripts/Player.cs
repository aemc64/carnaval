using UnityEngine;

public class Player : MonoBehaviour, IMovement
{
    // Tamaño del salto
    public LayerMask wallLayer; // Capa para detectar los muros
    
    private RhythmController _rhythmController;
    private Direction _direction;
    
    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
    }

    private void Update()
    {
        var direction = Direction.None;
        var updateInputTime = false;
        
        // Detectar teclas de movimiento
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // Tecla W o Flecha arriba
        {
            direction = Direction.Up;
            updateInputTime = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // Tecla S o Flecha abajo
        {
            direction = Direction.Down;
            updateInputTime = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) // Tecla A o Flecha izquierda
        {
            direction = Direction.Left;
            updateInputTime = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) // Tecla D o Flecha derecha
        {
            direction = Direction.Right;
            updateInputTime = true;
        }

        if (updateInputTime)
        {
            _rhythmController.UpdateLastInputTime();
        }
       
        // Mover el jugador
        if (direction != Direction.None && !IsWall(direction))
        {
            _direction = direction;
        }
    }
    
    // Usa un Raycast para detectar muros antes de moverse
    private bool IsWall(Direction direction)
    {
        var moveDirection = GameUtils.Directions[direction];
        var hit = Physics2D.Raycast(transform.position, moveDirection, GameUtils.TileSize, wallLayer);
        return hit.collider != null; // Si el Raycast detecta un muro, bloquea el movimiento
    }
    
    public Direction GetDirection()
    {
        return _direction;
    }
}