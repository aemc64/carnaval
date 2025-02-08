using UnityEngine;

public class Teacher : MonoBehaviour
{
    [SerializeField] private int _beatsToFlip = 4;
    [SerializeField] private Collider2D _visionCollider;
    
    private RhythmController _rhythmController;
    private SpriteRenderer _spriteRenderer;
    private ActionController _player;
    
    private int _currentBeats;
    private bool _isInVisionCollider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeat += OnBeat;
        
        _player = GameManager.Instance.Player;
    }

    private void Update()
    {
        var visionBounds = _visionCollider.bounds;
        if (visionBounds.Contains(_player.ActualPosition) && !_isInVisionCollider)
        {
            _isInVisionCollider = true;
            _player.ReflectAttack = true;
        }
        else if (!visionBounds.Contains(_player.ActualPosition) && _isInVisionCollider)
        {
            _isInVisionCollider = false;
            _player.ReflectAttack = false;
        }
    }

    private void OnBeat()
    {
        _currentBeats++;

        if (_currentBeats != _beatsToFlip)
        {
            return;
        }

        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        var visionPosition = _visionCollider.transform.localPosition;
        visionPosition.x *= -1;
        _visionCollider.transform.localPosition = visionPosition;
        
        _currentBeats = 0;
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeat -= OnBeat;
        }
    }
}
