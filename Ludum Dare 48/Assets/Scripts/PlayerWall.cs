using System.Collections.Generic;
using UnityEngine;

public class PlayerWall : MonoBehaviour
{
    //private readonly Dictionary<JumpDirection, PlayerWall> _otherWalls;

    [SerializeField] private List<JumpDirection> directions;
    [SerializeField] private PlayerWall[] otherWalls;
    [SerializeField] private Vector3 cameraEulerAngles;

    #if UNITY_EDITOR
    private void Awake()
    {
        if (directions.Count != otherWalls.Length)
        {
            Debug.LogError($"Length of directions does not match length of other walls on {name}");
        }
    }
    #endif

    public Vector3 GetTargetCameraEulerAngle()
    {
        return cameraEulerAngles;
    }

    /// <summary>Returns null if no walls correlate with that direction code</summary>
    public PlayerWall GetWallForDirection(JumpDirection key)
    {
        var index = directions.IndexOf(key);
        return index < 0 ? null : otherWalls[index];
    }
}
