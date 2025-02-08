using UnityEngine;

public class PlayerChaseMovement : MonoBehaviour, IMovement
{
    [SerializeField] private int _beatsToMove = 2;

    private ActionController _actionController;
    
    private int _currentBeats;
    private Transform _playerTransform;

    private void Awake()
    {
        _actionController = GetComponent<ActionController>();
    }

    private void Start()
    {
        _playerTransform = GameManager.Instance.Player.transform;
    }

    public Direction GetDirection()
    {
        _currentBeats++;

        if (_currentBeats != _beatsToMove)
        {
            return Direction.None;
        }
        
        _currentBeats = 0;
            
        var direction = _actionController.ActualPosition.GetDirectionTo(_playerTransform.position);
        return direction;
    }
}
