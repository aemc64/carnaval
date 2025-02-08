using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public RectTransform arrowImage;  // Flecha seleccionadora
    public Button[] menuButtons;      // Lista de botones
    private int selectedIndex = 0;    // Índice del botón seleccionado

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame(); // Espera a que Unity organice los botones

        selectedIndex = 0;
        UpdateArrowPosition();
        EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
    }


    void Update()
    {
        // Detectar entrada del teclado
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            UpdateArrowPosition();
            Debug.Log("Avance hacia abajo. Índice seleccionado: " + selectedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            UpdateArrowPosition();
            Debug.Log("Avance hacia arriba. Índice seleccionado: " + selectedIndex);
        }

        // Confirmar selección con ENTER o Espacio
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            menuButtons[selectedIndex].onClick.Invoke();
        }
    }

    // Mueve la flecha a la opción seleccionada
    void UpdateArrowPosition()
    {
        if (selectedIndex >= 0 && selectedIndex < menuButtons.Length)
        {
            // Obtenemos la posición de los botones en el espacio local del canvas
            Vector3 targetPosition = menuButtons[selectedIndex].transform.position;

            // Asegurémonos de que la flecha se mueva correctamente
            // Depuramos la posición para asegurarnos de que esté bien
            Debug.Log("Posición del botón seleccionado: " + targetPosition);

            // Mantenemos la misma X para la flecha, pero utilizamos la posición Y del botón
            targetPosition.x = arrowImage.position.x; // Mantener la misma X para la flecha

            // Actualizamos la posición de la flecha
            arrowImage.position = targetPosition;

            // Depuración adicional
            Debug.Log("Nueva posición de la flecha: " + arrowImage.position);
        }
    }
    

    // Método para mover la flecha cuando el ratón pasa sobre un botón
    public void OnMouseHover(int index)
    {
        Debug.Log("Ratón sobre opción: " + index); 
        selectedIndex = index;
        UpdateArrowPosition();
    }

    // Métodos para los botones
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // Cambia "GameScene" por el nombre de tu escena
    }

    public void ShowInstructions()
    {
        Debug.Log("Mostrar instrucciones...");
    }

    public void ShowCredits()
    {
        Debug.Log("Mostrar créditos...");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
