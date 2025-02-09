using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultPrefab;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Image _goalImage;
    [SerializeField] private float _feedBackDuration = 0.15f;
    [SerializeField] private Color _successColor = Color.green;
    [SerializeField] private Color _failColor = Color.red;

    private RhythmController _rhythmController;
    private readonly Color _transparentColor = new Color(0, 0, 0, 0);
    
    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeatResult += OnBeatResult;
    }
    
    private void OnBeatResult(BeatResultType beatResultType)
    {
        _goalImage.color = _transparentColor;
        
        if (beatResultType == BeatResultType.None)
        {
            return;
        }

        var success = beatResultType == BeatResultType.Success;
        var textInstance = Instantiate(_resultPrefab, transform);
        textInstance.text = success ? "Good!" : "Miss";
        var rectTransform = (RectTransform)textInstance.transform;
        rectTransform
            .DOAnchorPos(rectTransform.anchoredPosition + _offset, GameManager.Instance.RhythmController.BeatTime)
            .OnComplete(() => { Destroy(textInstance.gameObject); });
        
        _goalImage.DOColor(success ? _successColor : _failColor, _feedBackDuration);
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeatResult -= OnBeatResult;
        }
    }
}
