using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    internal static GameManager Instance { get; private set; }
    internal bool GameIsRunning { get; private set; }

    [Header("Unity events")]
    [SerializeField] internal UnityEvent OnGameStart;
    [SerializeField] internal UnityEvent OnGameEnd;

    [Header("References")]
    [SerializeField] private Text depthText;

    [Header("Difficulty scaling")]
    [SerializeField] private float timeUntilMaxDifficultyInSeconds = 120f;

    [SerializeField] private float playerFallSpeedStart = 10f;
    [SerializeField] private float playerFallSpeedEnd = 20f;

    [SerializeField] private float obstacleSpawnOffsetStart = 20f;
    [SerializeField] private float obstacleSpawnOffsetEnd = 5f;

    private float _elapsedTime;
    private float _points;

    private void Awake()
    {
        SingletonSetup();

        OnGameStart.AddListener(Init);
        OnGameEnd.AddListener(GameEnded);
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerHitChunkEnd += AddPoints;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHitChunkEnd -= AddPoints;
    }

    private void AddPoints(float points, PlayerController _)
    {
        _points -= points;
    }

    private void Init()
    {
        GameIsRunning = true;
        _elapsedTime = 0;
        _points = 0;
    }

    private void GameEnded()
    {
        GameIsRunning = false;
        var extraPoints = FindObjectOfType<PlayerController>().transform.position.y;
        _points -= extraPoints;
        depthText.text = ((int) _points).ToString();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (GameIsRunning) { return; }

        if (Input.GetMouseButtonDown(0)) { return; }
        if (Input.GetMouseButtonDown(1)) { return; }
        if (Input.GetMouseButtonDown(2)) { return; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        if (!Input.anyKeyDown) { return; }

        OnGameStart?.Invoke();
    }

    internal float GetPlayerFallSpeed()
    {
        return Mathf.Lerp(playerFallSpeedStart, playerFallSpeedEnd, _elapsedTime / timeUntilMaxDifficultyInSeconds);
    }

    internal float GetObstacleSpawnOffset()
    {
        return Mathf.Lerp(obstacleSpawnOffsetStart, obstacleSpawnOffsetEnd, _elapsedTime / timeUntilMaxDifficultyInSeconds);
    }

    private void SingletonSetup()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
