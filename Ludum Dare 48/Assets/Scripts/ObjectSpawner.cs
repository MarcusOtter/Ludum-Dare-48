using UnityEngine;

// Used as singleton spawner
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;

    private void Awake()
    {
        foreach (var obj in objectsToSpawn)
        {
            Instantiate(obj, Vector3.zero, Quaternion.identity);
        }
    }
}
