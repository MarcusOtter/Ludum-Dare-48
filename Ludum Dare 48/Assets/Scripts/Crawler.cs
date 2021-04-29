using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Crawler : MonoBehaviour
{
    [SerializeField] private float minPositionX;
    [SerializeField] private float maxPositionX;
    [SerializeField] [Range(0f, 90f)] private float minRotationZ = 25f;
    [SerializeField] [Range(0f, 90f)] private float maxRotationZ = 75f;
    [SerializeField] private float movementSpeed;   

    private Rigidbody _rigidbody;
    private Transform _transform;

    private float _startRotationZ;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        _startRotationZ = _transform.localEulerAngles.z;
    }

    private void Start()
    {
        SetNewRotation(Random.Range(0, 2) == 0);
    }

    private void Update()
    {
        var positionX = _transform.position.x;

        if (positionX < minPositionX)
        {
            SetNewRotation(false);
        }
        else if (positionX > maxPositionX)
        {
            SetNewRotation(true);
        }

        _rigidbody.velocity = _transform.up * movementSpeed;
    }

    private void SetNewRotation(bool facingLeft)
    {
        var newRotationZ = Random.Range(minRotationZ, maxRotationZ);
        if (!facingLeft) { newRotationZ *= -1; }

        _rigidbody.rotation = Quaternion.Euler(_transform.localEulerAngles.With(z: newRotationZ));
    }
}
