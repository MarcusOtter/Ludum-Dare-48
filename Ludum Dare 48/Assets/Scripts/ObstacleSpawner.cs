using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] obstacles;
    [SerializeField] private PlayerController player;
    [SerializeField] private float spawnDistanceFromPlayer = 40;

    private float _previousSpawnPlayerPositionY = float.MaxValue;

    private void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(() => _previousSpawnPlayerPositionY = float.MaxValue);
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerHitChunkEnd += AccountForPlayerTeleportation;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHitChunkEnd -= AccountForPlayerTeleportation;
    }

    private void AccountForPlayerTeleportation(float offsetY, PlayerController _)
    {
        _previousSpawnPlayerPositionY -= offsetY;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameIsRunning) { return; }
        var playerPositionY = player.transform.position.y;
        var spawnOffset = GameManager.Instance.GetObstacleSpawnOffset();

        if (_previousSpawnPlayerPositionY - playerPositionY > spawnOffset)
        {
            SpawnRandomObstacle();
            _previousSpawnPlayerPositionY = playerPositionY;
        }
    }

    private void SpawnRandomObstacle()
    {
        var obstacle = obstacles[Random.Range(0, obstacles.Length)];
        var spawnPosition = Vector3.up * (player.transform.position.y - spawnDistanceFromPlayer);
        // Scale hack is to prevents z-fighting with level walls since we also render the inside of the cubes
        MovingObjectsHandler.Instance.SpawnMovingObject(obstacle, spawnPosition, scale: Vector3.one * 0.999f); 
    }

}
