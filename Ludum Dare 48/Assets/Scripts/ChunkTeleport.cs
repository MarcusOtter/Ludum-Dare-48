using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTeleport : MonoBehaviour
{
    [SerializeField] private float yOffset = 250f;

    private void OnTriggerEnter(Collider other)
    {
        var playerController = other.GetComponentInParent<PlayerController>();
        if (playerController == null) { return; }
        playerController.TriggerHitChunkEnd(other.transform.position.y + yOffset);
    }
}
