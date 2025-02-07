using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2;
    public float stepSize = 1f; // Tamaño del salto
    public LayerMask wallLayer; // Capa para detectar los muros

    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveDirection = Vector2.zero;

        // Detectar teclas de movimiento
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))  // Tecla W o Flecha arriba
            moveDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // Tecla S o Flecha abajo
            moveDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) // Tecla A o Flecha izquierda
            moveDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) // Tecla D o Flecha derecha
            moveDirection = Vector2.right;
       
        // Mover el jugador
        if (moveDirection != Vector2.zero)
        {
            // Solo mover si no hay muro adelante
            if (!IsWall(moveDirection))
            {
                Vector2 targetPosition = transform.position + (Vector3)moveDirection * stepSize;
                transform.position = targetPosition;
            }
        }
    }

    // Usa un Raycast para detectar muros antes de moverse
    private bool IsWall(Vector2 moveDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, stepSize, wallLayer);
        return hit.collider != null; // Si el Raycast detecta un muro, bloquea el movimiento
    }
}