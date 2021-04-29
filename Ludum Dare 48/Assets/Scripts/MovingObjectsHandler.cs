using UnityEngine;

public class MovingObjectsHandler : MonoBehaviour
{
    internal static MovingObjectsHandler Instance { get; private set; }

    private void Awake()
    {
        SingletonSetup();
    }

    private void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(DestroyAllChildren);
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerHitChunkEnd += OnPlayerChunkTeleport;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHitChunkEnd -= OnPlayerChunkTeleport;
    }

    // https://stackoverflow.com/a/60391826/10615308
    private void DestroyAllChildren()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private void OnPlayerChunkTeleport(float offsetPositionY, PlayerController _)
    {
        foreach (Transform child in transform)
        {
            child.position = child.position.Add(y: offsetPositionY * -1);
        }
        //transform.position = transform.position.Add();
    }

    internal T SpawnMovingObject<T>(T objectPrefab, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null) where T : Component
    {
        position ??= Vector3.zero;
        rotation ??= Quaternion.identity;

        var obj = Instantiate(objectPrefab, position.Value, rotation.Value, transform);

        if (scale.HasValue)
        {
            obj.transform.localScale = scale.Value;
        }

        return obj;
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
