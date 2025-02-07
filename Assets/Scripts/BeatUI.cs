using DG.Tweening;
using UnityEngine;

public class BeatUI : MonoBehaviour
{
    [SerializeField] private Vector2 _offset;

    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private RhythmController _rhythmController;
    
    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _originalPosition = _rectTransform.anchoredPosition;
    }

    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeat += OnBeat;
        _rhythmController.OnActivate += OnBeat;
    }

    private void OnBeat()
    {
        _rectTransform.anchoredPosition = _originalPosition;
        _rectTransform.DOAnchorPos(_originalPosition + _offset, _rhythmController.BeatTime).SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeat -= OnBeat;
        }
    }
}
