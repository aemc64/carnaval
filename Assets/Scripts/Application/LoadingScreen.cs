using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private float _changeDuration = 0.25f;
    
    public void Show(Action onComplete = null)
    {
        _background.raycastTarget = true;
        _background.transform.DOScale(Vector3.zero, _changeDuration).OnComplete(() => { onComplete?.Invoke(); });
    }

    public void Hide(Action onComplete = null)
    {
        _background.raycastTarget = false;
        _background.transform.DOScale(Vector3.one, _changeDuration).OnComplete(() => { onComplete?.Invoke(); });
    }
}
