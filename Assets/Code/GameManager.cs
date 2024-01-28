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
    [SerializeField] private TMP_Text level;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject home;

    [SerializeField] private Image battery;
    [SerializeField] private Sprite batteryFull;
    [SerializeField] private Sprite batteryHalf;
    [SerializeField] private Sprite batteryLow;
    [SerializeField] private Sprite batteryEmpty;

    [SerializeField] private Canvas canvas;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip progressSound, successSound, failSound, levelUpSound, gameOverSound;

    [SerializeField] private GameObject[] microGames;
    [SerializeField] private string[] hints;
    [SerializeField] private TMP_Text hint;

    private GameObject _currentMicroGame;
    private GameObject _currentMicroGamePrefab;

    private const float TimerMax = 6.0f;
    private const float LevelUpCount = 5.0f;

    private int _score = 0;
    private int _gamesFinished = 0;
    private int _highScore;
    private float _timer = TimerMax;
    private int _life = 3;

    private float _speed = 1.0f;

    public GameState state;
    public event Action OnTick;

    private void Start() {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScore.text = $"High Score: {_highScore} minutes";
        score.text = $"Screen Time: {_score} minutes";
        NewGame();
    }

    public void NewGame() {
        _score = 0;
        _timer = TimerMax;
        SetLife(3);
        SetScoreAndLevel(0, 0);
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
        do {
            index = random.Next(0, microGames.Length);
        } while (microGames[index] == _currentMicroGamePrefab);
        _currentMicroGamePrefab = microGames[index];
        hint.text = hints[index] + " ->";
        return Instantiate(_currentMicroGamePrefab,  canvas.transform);
    }
    
    public void FinishMicroGame(bool success) {
        Destroy(_currentMicroGame);
        if (!success) {
            SetLife(_life - 1);
        }
        if (_life == 0) {
            SetState(GameState.GameOver);
            return;
        }

        PlayAudioClip(success ? successSound : failSound);
        _timer = TimerMax;
        SetScoreAndLevel(success? _score + 10 : _score, _gamesFinished + 1);
        SetState(GameState.Home);
    }

    private void SetScoreAndLevel(int newScore, int gamesFinished) {
        _score = newScore;
        _highScore = Math.Max(_score, _highScore);
        score.text = $"Screen Time: {_score} minutes";
        highScore.text = $"High Score: {_highScore} minutes";

        int oldLevel = Mathf.FloorToInt(_gamesFinished / LevelUpCount) + 1;
        _gamesFinished = gamesFinished;
        int newLevel = Mathf.FloorToInt(_gamesFinished / LevelUpCount) + 1;
        if (oldLevel == newLevel) return;

        level.text = $"Level {newLevel}";
        _speed = 1.0f + (newLevel - 1) * .2f;
        musicSource.pitch = _speed;
        SetLife(_life + 1);
        PlayAudioClip(levelUpSound);
    }

    private void PlayAudioClip(AudioClip clip) {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlayProgressSound() => PlayAudioClip(progressSound);

    private void SetLife(int life) {
        _life = Math.Clamp(life, 0, 3);
        battery.sprite = _life switch {
            3 => batteryFull,
            2 => batteryHalf,
            1 => batteryLow,
            _ => batteryEmpty
        };
    }

    private void SetState(GameState newState) {
        state = newState;
        switch (newState) {
            case GameState.Start:
                break;
            case GameState.Home:
                gameOver.gameObject.SetActive(false);
                hint.gameObject.SetActive(false);
                home.SetActive(true);
                break;
            case GameState.MicroGame:
                home.SetActive(false);
                hint.gameObject.SetActive(true);
                break;
            case GameState.GameOver:
                home.SetActive(true);
                gameOver.gameObject.SetActive(true);
                PlayAudioClip(gameOverSound);
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
