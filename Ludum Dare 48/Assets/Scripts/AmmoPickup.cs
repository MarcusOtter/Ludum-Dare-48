using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AmmoPickup : MonoBehaviour
{
    [SerializeField] private int ammoAmount;
    [SerializeField] private float relativeVelocityFactor = 0.5f; // relative to player
    // can add get slowmo bool and stuff here

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.velocity = Vector3.up * GameManager.Instance.GetPlayerFallSpeed() * relativeVelocityFactor;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player == null) { return; }
        var bulletSpawner = player.GetComponentInChildren<BulletSpawner>();
        bulletSpawner.AddAmmo(ammoAmount);
        Destroy(gameObject);
    }
}
