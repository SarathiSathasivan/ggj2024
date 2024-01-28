using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class GameManager : StaticInstance<GameManager> {
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text highScore;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject home;

    [SerializeField] private Image battery;
    [SerializeField] private Sprite batteryFull;
    [SerializeField] private Sprite batteryHalf;
    [SerializeField] private Sprite batteryLow;
    [SerializeField] private Sprite batteryEmpty;

    [SerializeField] private Canvas canvas;

    [SerializeField] private GameObject[] microGames;
    private GameObject _currentMicroGame;
    private GameObject _currentMicroGamePrefab;

    private const float TimerMax = 5.0f;

    private int _score = 0;
    private int _highScore;
    private float _timer = TimerMax;
    private int _life = 3;

    private float _speed = 1.0f;
    public GameState state;
    public event Action OnTick;

    private void Start() {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScore.text = $"High Score: {_highScore}";
        score.text = $"Score: {_score}";
        NewGame();
    }

    public void NewGame() {
        _score = 0;
        _timer = TimerMax;
        SetLife(3);
        SetState(GameState.Home);
    }

    private void SpawnMicroGame() {
        GameObject microGame = GetRandomMicroGame();
        microGame.GetComponent<MicroGame>().Initialize();
        _currentMicroGame = microGame;
        SetState(GameState.MicroGame);
    }

    private GameObject GetRandomMicroGame() {
        Random random = new Random();
        int index = 0;
        // do {
        //     index = random.Next(0, microGames.Length);
        // } while (microGames[index] == _currentMicroGamePrefab);
        _currentMicroGamePrefab = microGames[index];
        return Instantiate(_currentMicroGamePrefab,  canvas.transform);
    }
    
    public void FinishMicroGame(bool success) {
        Destroy(_currentMicroGame);
        if (!success) {
            SetLife(_life - 1);
        }
        if (_life == 0) {
            GameOver();
            return;
        }
        _timer = TimerMax;
        UpdateScoreAndLevel(success);
        SetState(GameState.Home);
    }

    private void UpdateScoreAndLevel(bool success) {
        _score += success ? 100 : 000;
        _highScore = Math.Max(_score, _highScore);
        score.text = $"Score: {_score}";
        highScore.text = $"High Score: {_highScore}";
        // TODO level
    }

    private void SetLife(int life) {
        _life = Math.Clamp(life, 0, 3);
        battery.sprite = _life switch {
            3 => batteryFull,
            2 => batteryHalf,
            1 => batteryLow,
            _ => batteryEmpty
        };
    }

    private void GameOver() {
        SetState(GameState.GameOver);
    }

    private void SetState(GameState newState) {
        state = newState;
        switch (newState) {
            case GameState.Start:
                break;
            case GameState.Home:
                gameOver.gameObject.SetActive(false);
                home.SetActive(true);
                break;
            case GameState.MicroGame:
                home.SetActive(false);
                break;
            case GameState.GameOver:
                home.SetActive(true);
                gameOver.gameObject.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void Update() {
        if (state != GameState.Home && state != GameState.MicroGame) return;
        _timer -= Time.deltaTime * _speed;
        if (_timer < 1.0f) {
            _timer += TimerMax;
            switch (state) {
                case GameState.Home:
                    SpawnMicroGame();
                    break;
                case GameState.MicroGame:
                    FinishMicroGame(false);
                    break;
            }
        }
        string newTimerText = $"{Mathf.Floor(_timer)}";
        if (timer.text == newTimerText) return;
        OnTick?.Invoke();
        timer.text = newTimerText;
    }
    
    public enum GameState {
        Start,
        Home,
        MicroGame,
        GameOver
    }
}
