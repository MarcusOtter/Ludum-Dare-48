using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private Transform bulletHitEffectPrefab;
    [SerializeField] private float bulletHitEffectDestroyDelay = 2f;
    [SerializeField] private int maxBounces = 5;
    [SerializeField] private bool makeScaleSmallerOnHit;
    [SerializeField] [Range(0f, 10f)] private float minimumVelocity = 10f;
    [SerializeField] private float maxLifetimeInSeconds = 5f;

    private Rigidbody _rigidbody;
    private Transform _transform;

    private float _scaleStep;
    private int _bounces;
    private float _spawnTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        _scaleStep = _transform.localScale.x / (maxBounces + 1);
        _spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - _spawnTime > maxLifetimeInSeconds)
        {
            Destroy(gameObject);
        }

        var velocity = _rigidbody.velocity.magnitude;
        if (velocity < minimumVelocity)
        {
            // Destroy bullet if it is moving too slow
            Destroy(gameObject);
        }
    }

    internal void Shoot(float velocity, Vector3 direction)
    {
        transform.forward = direction;
        _rigidbody.velocity = direction * velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bulletHitEffectPrefab != null)
        {
            var bulletHit = MovingObjectsHandler.Instance.SpawnMovingObject(bulletHitEffectPrefab, _transform.position);
            //var bulletHit = Instantiate(bulletHitEffectPrefab, _transform.position, Quaternion.identity);
            Destroy(bulletHit.gameObject, bulletHitEffectDestroyDelay);
        }

        var damageable = collision.collider.GetComponent<IDamageable>();
        damageable?.TakeDamage(1);

        if (maxBounces <= 0 || ++_bounces > maxBounces)
        {
            Destroy(gameObject);
            return;
        }

        if (makeScaleSmallerOnHit)
        {
            _transform.localScale = _transform.localScale.Add(-_scaleStep, -_scaleStep, -_scaleStep);
        }
    }
}
