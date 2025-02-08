using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyMovement : MonoBehaviour, IMovement
{
    [SerializeField] private int _beatsToMove = 3;
    [SerializeField] private Transform _targetsContainer;

    private ActionController _actionController;
    
    private RhythmController _rhythmController;
    private int _currentBeats;
    private int _currentWaypointIndex = 1;

    private readonly List<Vector3> _waypoints = new List<Vector3>();

    private void Awake()
    {
        _actionController = GetComponent<ActionController>();
        
        _waypoints.Add(transform.position);
        
        foreach (Transform target in _targetsContainer)
        {
            _waypoints.Add(target.position);
        }
    }

    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeat += OnBeat;
    }

    private void OnBeat()
    {
        if (_currentBeats != 0)
        {
            return;
        }

        if (!_actionController.ActualPosition.IsAt(_waypoints[_currentWaypointIndex]))
        {
            return;
        }
        
        _currentWaypointIndex++;
        if (_currentWaypointIndex == _waypoints.Count)
        {
            _currentWaypointIndex = 0;
        }
    }

    public Direction GetDirection()
    {
        _currentBeats++;

        if (_currentBeats != _beatsToMove)
        {
            return Direction.None;
        }
        
        _currentBeats = 0;
            
        var destination = _waypoints[_currentWaypointIndex];
        var direction = _actionController.ActualPosition.GetDirectionTo(destination);
        return direction;
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeat -= OnBeat;
        }
    }
}
