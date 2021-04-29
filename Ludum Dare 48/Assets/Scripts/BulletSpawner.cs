using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private TextMeshProUGUI ammoText;

    [Header("Fields")]
    [SerializeField] private float bulletVelocity = 30;
    [SerializeField] private float shotsPerSecond = 10;
    [SerializeField] private int startingAmmo = 64;

    private float _previousShotTime;
    private float _shootDelay;
    private int _currentAmmo;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GameManager.Instance.OnGameStart.AddListener(Init);
    }

    private void Init()
    {
        _previousShotTime = Time.time;
        _shootDelay = 1f / shotsPerSecond;
        _currentAmmo = startingAmmo;
        ModifyAmmo(0);
    }

    private void Update()
    {
        if (!GameManager.Instance.GameIsRunning) { return; }
        if (!Input.GetButton("Fire1")) { return; }
        if (Time.time < _previousShotTime + _shootDelay) { return; }
        if (_currentAmmo <= 0) { return; }

        Shoot();
        _previousShotTime = Time.time;
        ModifyAmmo(-1);
    }

    internal void AddAmmo(int amount)
    {
        ModifyAmmo(amount);
    }

    private void ModifyAmmo(int delta)
    {
        _currentAmmo += delta;
        ammoText.text = _currentAmmo.ToString();
    }

    private void Shoot()
    {
        var bullet = MovingObjectsHandler.Instance.SpawnMovingObject(bulletPrefab, transform.position);
        var bulletSpeed = GameManager.Instance.GetPlayerFallSpeed() + bulletVelocity;
        bullet.Shoot(bulletSpeed, transform.forward);
    }
}
