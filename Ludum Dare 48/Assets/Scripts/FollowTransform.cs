using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform transformToFollow;

    [Header("Fields")]
    [SerializeField] private bool followPositionX = true;
    [SerializeField] private bool followPositionY = true;
    [SerializeField] private bool followPositionZ = true;
    [SerializeField] private bool followRotationX;
    [SerializeField] private bool followRotationY;
    [SerializeField] private bool followRotationZ;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        if (transformToFollow == null) { return; }

        var newPosition = _transform.position;
        if (followPositionX) { newPosition.x = transformToFollow.position.x; }
        if (followPositionY) { newPosition.y = transformToFollow.position.y; }
        if (followPositionZ) { newPosition.z = transformToFollow.position.z; }

        var newRotation = _transform.localEulerAngles;
        if (followRotationX) { newRotation.x = transformToFollow.localEulerAngles.x; }
        if (followRotationY) { newRotation.y = transformToFollow.localEulerAngles.y; }
        if (followRotationZ) { newRotation.z = transformToFollow.localEulerAngles.z; }

        _transform.position = newPosition;
        _transform.localEulerAngles = newRotation;
    }
}
