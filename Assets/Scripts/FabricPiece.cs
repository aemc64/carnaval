using System;
using DG.Tweening;
using UnityEngine;

public class FabricPiece : MonoBehaviour
{
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private Vector2 _offset;
 
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        var targetPosition = transform.position + (Vector3)_offset;
        
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(targetPosition, _moveDuration));
        sequence.AppendInterval(_moveDuration / 2);
        sequence.Join(_spriteRenderer.DOFade(0f, _moveDuration / 2));
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
