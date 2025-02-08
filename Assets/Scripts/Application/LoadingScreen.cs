using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private float _fadeDuration = 0.25f;
    
    public void Show(Action onComplete = null)
    {
        _background.raycastTarget = true;
        _background.DOFade(1f, _fadeDuration).OnComplete(() => { onComplete?.Invoke(); });
    }

    public void Hide(Action onComplete = null)
    {
        _background.raycastTarget = false;
        _background.DOFade(0f, _fadeDuration).OnComplete(() => { onComplete?.Invoke(); });
    }
}
