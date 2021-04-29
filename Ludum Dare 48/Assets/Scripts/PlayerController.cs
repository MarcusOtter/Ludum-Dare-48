using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    internal static event Action<float, PlayerController> OnPlayerHitChunkEnd;

    [Header("References")]
    [SerializeField] private PlayerWall startingWall;

    [Header("Fields")]
    [SerializeField] private float lookSpeedX = 1f;
    [SerializeField] private float lookSpeedY = 1f;
    [SerializeField] private float maxRotationX = 30f;
    [SerializeField] private float jumpSpeed = 0.1f;
    [SerializeField] private float jumpCooldownInSeconds = 0.25f;

    // Private references
    private Rigidbody _rigidbody;
    private Transform _transform;
    private PlayerWall _currentWall;

    // Private fields
    private bool _isJumping;
    private Coroutine _activeJumpCoroutine;

    private Vector3 _gameStartPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;

        _currentWall = startingWall;
        _transform.position = startingWall.transform.position;
        _gameStartPosition = _transform.position;


        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(() => _transform.position = _gameStartPosition);
    }

    private void Update()
    {
        _rigidbody.velocity = Vector3.down * GameManager.Instance.GetPlayerFallSpeed();

        if (!GameManager.Instance.GameIsRunning) { return; }

        CheckForJumpInput();

        if (!_isJumping)
        {
            UpdateRotation();
        }
    }

    public void TriggerHitChunkEnd(float restartYPosition)
    {
        var currentPosition = _transform.position;

        var yPositionDifference = currentPosition.y - restartYPosition;
        _transform.position = currentPosition.With(y: restartYPosition);
        OnPlayerHitChunkEnd?.Invoke(yPositionDifference, this);
    }

    private void CheckForJumpInput()
    {
        if (_isJumping) { return; }

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical   = Input.GetAxisRaw("Vertical");

        if (horizontal > 0)      { StartJump(JumpDirection.Right);   }
        else if (horizontal < 0) { StartJump(JumpDirection.Left);    }
        else if (vertical > 0)   { StartJump(JumpDirection.Forward); }

        //_rigidbody.velocity = new Vector3(horizontal, -fallSpeed, vertical);
    }

    private void StartJump(JumpDirection direction)
    {
        if (_isJumping) { return; }
        if (_activeJumpCoroutine != null) { return; }

        var wall = _currentWall.GetWallForDirection(direction);
        _activeJumpCoroutine = StartCoroutine(JumpToWall(wall, direction != JumpDirection.Forward));
        _isJumping = true;
    }

    private IEnumerator JumpToWall(PlayerWall wall, bool shortJump)
    {
        var wallTransform = wall.transform;

        var startPosition = _transform.position;
        var startRotation = _transform.rotation;

        var targetPosition = wallTransform.position.With(y: startPosition.y);
        var targetRotation = Quaternion.Euler(wallTransform.eulerAngles.With(x: _transform.eulerAngles.x, z: 0));

        for (float t = 0f, increment; t <= 1; t += increment)
        {
            yield return null; // Wait for end of frame

            increment = jumpSpeed * Time.deltaTime;

            // Short jumps get double speed
            if (shortJump) { increment *= 2; } 

            _transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            _transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
        }

        _isJumping = false;
        _currentWall = wall;

        // Don't allow jumping again for a fraction of a second
        // (but allow looking around, therefore isJumping = false)
        yield return new WaitForSeconds(jumpCooldownInSeconds);

        if (_activeJumpCoroutine != null)
        {
            StopCoroutine(_activeJumpCoroutine);
            _activeJumpCoroutine = null;
        }
    }

    // ReSharper disable Unity.UnknownInputAxes
    private void UpdateRotation()
    {
        var mouseDeltaY = Input.GetAxis("Mouse Y");
        var mouseDeltaX = Input.GetAxis("Mouse X");

        // Caching for performance (ReSharper suggestion)
        var t = transform;
        var eulerAngles = t.localEulerAngles;

        // Ensure eulerAngles are between -180 and 180 degrees
        if (eulerAngles.x > 180f)       { eulerAngles.x -= 360f; }
        else if (eulerAngles.x < -180f) { eulerAngles.x += 360f; }

        var newXRotation = Mathf.Clamp(eulerAngles.x + mouseDeltaY * -lookSpeedX, -maxRotationX, maxRotationX);
        var newYRotation = eulerAngles.y + mouseDeltaX * lookSpeedY;

        t.localEulerAngles = new Vector3(newXRotation, newYRotation, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Wall")) { return; }
        GameManager.Instance.OnGameEnd?.Invoke();
    }
}
