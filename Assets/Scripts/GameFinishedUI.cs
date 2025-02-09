using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameFinishedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameFinishedText;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private AudioClip _loseSound;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private AudioSource _audioSource;
    
    private void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_gameFinishedText.DOFade(1, 0.25f));
        sequence.AppendInterval(2f);
        sequence.AppendCallback(() =>
        {
            if (Core.Instance.PlayerWonLastGame)
            {
                _winScreen.SetActive(true);
                _audioSource.clip = _winSound;
            }
            else
            {
                _loseScreen.SetActive(true);
                _audioSource.clip = _loseSound;
            }
            
            _audioSource.Play();
        });
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            _buttons.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_buttons.GetComponentInChildren<Button>().gameObject);
        });
    }
}
