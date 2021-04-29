using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AmmoPickup standardAmmoPrefab;
    [SerializeField] private AmmoPickup rareAmmoPrefab;
    [SerializeField] private Transform[] playerWalls;

    [Header("Fields")]
    [SerializeField] private float rollDelay = 10f;
    [SerializeField] [Range(0f, 1f)] private float standardChance = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float rareChance = 0.2f;
    [SerializeField] private float playerOffsetY = 50f;
    [SerializeField] private float ammoDestroyDelay = 15f;

    private Transform _transform;

    private float _previousRollTime;

    private void Awake()
    {
        _previousRollTime = Time.time;
        _transform = transform;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameIsRunning) { return; }
        if (Time.time - _previousRollTime < rollDelay) { return; }

        _previousRollTime = Time.time;

        var rareRoll = Random.Range(0f, 1f);
        if (rareRoll < rareChance)
        {
            var spawnPosition = Vector3.zero.With(y: _transform.position.y - playerOffsetY);
            var spawned = MovingObjectsHandler.Instance.SpawnMovingObject(rareAmmoPrefab, spawnPosition);
            Destroy(spawned.gameObject, ammoDestroyDelay);
            return;
        }

        var standardRoll = Random.Range(0f, 1f);
        if (standardRoll < standardChance)
        {
            var spawnPosition = playerWalls[Random.Range(0, playerWalls.Length)].position.Add(y: playerOffsetY * -1);
            var spawned = MovingObjectsHandler.Instance.SpawnMovingObject(standardAmmoPrefab, spawnPosition);
            Destroy(spawned.gameObject, ammoDestroyDelay);
        }

    }


}
