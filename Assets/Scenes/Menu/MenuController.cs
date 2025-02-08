using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private RectTransform _arrowTransform;
    
    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = EventSystem.current;
        
        CheckSelectedButton();
    }

    private void Update()
    {
        CheckSelectedButton();
    }

    private void CheckSelectedButton()
    {
        foreach (var button in _buttons)
        {
            if (_eventSystem.currentSelectedGameObject == button.gameObject)
            {
                OnButtonSelected(button);
            }
        }
    }

    private void OnButtonSelected(Button button)
    {
        var buttonPositionY = button.transform.position.y;
        
        var arrowPosition = _arrowTransform.position;
        arrowPosition.y = buttonPositionY;
        _arrowTransform.position = arrowPosition;
        
        _arrowTransform.gameObject.SetActive(true);
    }
}
