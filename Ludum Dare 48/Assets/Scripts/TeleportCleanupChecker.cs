using UnityEngine;

public class TeleportCleanupChecker : MonoBehaviour
{
    [SerializeField] private float _maxDistanceYFromPlayer = 100f;


    private void OnEnable()
    {
        PlayerController.OnPlayerHitChunkEnd += CleanupIfRequired;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHitChunkEnd -= CleanupIfRequired;
    }

    private void CleanupIfRequired(float _, PlayerController player)
    {
        var differenceY = Mathf.Abs(transform.position.y) - Mathf.Abs(player.transform.position.y);
        if (differenceY > _maxDistanceYFromPlayer)
        {
            Destroy(gameObject);
        }
    }
}
