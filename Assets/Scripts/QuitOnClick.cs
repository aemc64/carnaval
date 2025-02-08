using UnityEngine;
using UnityEngine.UI;

public class QuitOnClick : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(Application.Quit);
    }
}
