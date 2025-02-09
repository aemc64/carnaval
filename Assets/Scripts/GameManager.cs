using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private RhythmController _rhythmController;
    [SerializeField] private ActionController _player;
    [SerializeField] private Transform _enemiesParent;

    [SerializeField] private TextMeshProUGUI _screenCenterText;

    private int _missesCount;

    private List<ActionController> _enemyControllers;
    
    public RhythmController RhythmController => _rhythmController;
    public ActionController Player => _player;

    public event Action OnEnemyDefeated;
    
    public int DefeatedEnemies { get; private set; }
    public int MaxEnemies { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        _rhythmController.OnBeatResult += OnBeatResult;
        
        _enemyControllers = new List<ActionController>(_enemiesParent.GetComponentsInChildren<ActionController>());
        MaxEnemies = _enemyControllers.Count;
    }

    private void Start()
    {
        var sequence = DOTween.Sequence();
        
        AddCountdownTween(sequence, 3.ToString());
        AddCountdownTween(sequence, 2.ToString());
        AddCountdownTween(sequence, 1.ToString());
        AddCountdownTween(sequence, "GO!");
        
        sequence.OnComplete(() =>
        {
            _screenCenterText.gameObject.SetActive(false);
            _rhythmController.Activate();
        });
    }

    private void AddCountdownTween(Sequence sequence, string text)
    {
        sequence.AppendCallback(() =>
        {
            _screenCenterText.text = text;
            _screenCenterText.transform.localScale = Vector3.zero;
        });
        sequence.Append(_screenCenterText.transform.DOScale(Vector3.one, 0.25f));
        sequence.AppendInterval(0.75f);
    }

    private void OnBeatResult(BeatResultType beatResultType)
    {
        var onBeat = beatResultType == BeatResultType.Success;

        if (beatResultType == BeatResultType.Failure)
        {
            _missesCount++;
        }
        
        _player.UpdateIntendedAction(onBeat);
        _player.DoAction();

        foreach (var enemy in _enemyControllers)
        {
            enemy.UpdateIntendedAction(onBeat);
            enemy.DoAction();
        }

        if (!_player.enabled)
        {
            GameOver(false);
            return;
        }

        for (var i = _enemyControllers.Count - 1; i >= 0; i--)
        {
            if (!_enemyControllers[i].enabled)
            {
                _enemyControllers.RemoveAt(i);
                DefeatedEnemies++;
                
                OnEnemyDefeated?.Invoke();
            }
        }

        if (DefeatedEnemies == MaxEnemies)
        {
            GameOver(true);
        }
    }

    private void GameOver(bool playerWon)
    {
        _rhythmController.Deactivate();

        _screenCenterText.text = "FINISH!";
        _screenCenterText.gameObject.SetActive(true);
        
        Core.Instance.PlayerWonLastGame = playerWon;

        StartCoroutine(DelayAndLoadGameFinishedScene());
    }

    private static IEnumerator DelayAndLoadGameFinishedScene()
    {
        yield return new WaitForSeconds(1f);
        Core.Instance.LoadScene("GameFinished");
    }

    private void OnDestroy()
    {
        if (_rhythmController != null)
        {
            _rhythmController.OnBeatResult -= OnBeatResult;
        }
    }
}
