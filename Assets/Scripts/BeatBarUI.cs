using DG.Tweening;
using TMPro;
using UnityEngine;

public class BeatBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultPrefab;
    [SerializeField] private Vector2 _offset;

    private RhythmController _rhythmController;
    
    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeatResult += OnBeatResult;
    }
    
    private void OnBeatResult(BeatResultType beatResultType)
    {
        if (beatResultType == BeatResultType.None)
        {
            return;
        }
        
        var textInstance = Instantiate(_resultPrefab, transform);
        textInstance.text = beatResultType == BeatResultType.Success ? "Good!" : "Miss";
        var rectTransform = (RectTransform)textInstance.transform;
        rectTransform
            .DOAnchorPos(rectTransform.anchoredPosition + _offset, GameManager.Instance.RhythmController.BeatTime)
            .OnComplete(() => { Destroy(textInstance.gameObject); });
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeatResult -= OnBeatResult;
        }
    }
}
