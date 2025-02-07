using TMPro;
using UnityEngine;

public class BeatBarUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _result;

    private RhythmController _rhythmController;
    
    private void Start()
    {
        _rhythmController = GameManager.Instance.RhythmController;
        _rhythmController.OnBeatResult += OnBeatResult;
    }
    
    private void OnBeatResult(bool success)
    {
        _result.text = success ? "Good" : "Bad";
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeatResult -= OnBeatResult;
        }
    }
}
