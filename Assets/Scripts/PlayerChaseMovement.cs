using UnityEngine;

public class PlayerChaseMovement : MonoBehaviour, IMovement
{
    [SerializeField] private int _beatsToMove = 2;
    [SerializeField] private int _radius = 6;

    private ActionController _actionController;
    
    private int _currentBeats;
    private ActionController _player;
    private Vector3 _originalPosition;

    private void Awake()
    {
        _actionController = GetComponent<ActionController>();
        _originalPosition = transform.position;
    }

    private void Start()
    {
        _player = GameManager.Instance.Player;
    }

    public Direction GetDirection()
    {
        _currentBeats++;

        if (_currentBeats != _beatsToMove)
        {
            return Direction.None;
        }
        
        _currentBeats = 0;

        var distanceToPlayer = Vector2.Distance(_player.ActualPosition, _originalPosition);
        return _actionController.ActualPosition.GetDirectionTo(distanceToPlayer < _radius
            ? _player.ActualPosition
            : _originalPosition);
    }
}
