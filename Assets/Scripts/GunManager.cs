using UnityEngine;
using System.Collections;

public class GunManager : MonoBehaviour
{
    [Header("Orbit")]
    [SerializeField] private Transform gunPivot;
    [SerializeField][Range(0f, 720f)] private float orbitSpeed = 120f;
    [SerializeField][Range(0f, 1f)] private float orbitDistance = 0.35f;
    [SerializeField] private bool isClockwise = false;
    private Transform gunOrbit;
    private Transform gunSprite;
    private Transform firePoint;

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletFolder;
    [SerializeField] private GunSO data;

    [Header("Recoil")]
    [SerializeField][Range(0.01f, 0.1f)] private float recoilDuration = 0.05f;
    [SerializeField][Range(0.01f, 0.25f)] private float recoilDistance = 0.1f;

    private Vector2 aimDir = Vector2.right;
    private float currentAngle = 0f;
    private SpriteRenderer gunSpriteRend;
    private InputManager input;
    private CrosshairManager crosshairManager;
    private StatsManager stats;

    // Shooting state
    private bool isFiring;
    private bool ammoConsumed;
    private float nextFire;

    private void Awake()
    {
        // Components
        input = GetComponent<InputManager>();
        crosshairManager = GetComponent<CrosshairManager>();
        stats = GetComponent<StatsManager>();

        gunOrbit = gunPivot.GetChild(0);
        gunSprite = gunOrbit.GetChild(0);
        firePoint = gunSprite.GetChild(0);
        gunSpriteRend = gunSprite.GetComponent<SpriteRenderer>();

        // Setup
        SwitchGun(data);
    }

    private void OnEnable()
    {
        gunSprite.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        gunSprite.gameObject.SetActive(false);
    }

    private void Update()
    {
        TryShooting();
        
        if (!input.IsPrimary) ammoConsumed = false;
    }

    private void LateUpdate()
    {
        // --- Update Aim Direction ---
        if (input.Look.sqrMagnitude > 0.01f)
            aimDir = input.Look.normalized;
        else if (input.Move.sqrMagnitude > 0.01f)
            aimDir = input.Move.normalized;

        // --- Orbit the gun around pivot ---
        currentAngle += (isClockwise ? -1 : 1) * orbitSpeed * Time.deltaTime;
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitDistance;
        gunOrbit.localPosition = offset;

        // --- Rotate the gun to aim ---
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        gunSprite.rotation = Quaternion.Euler(0f, 0f, angle);

        // --- Flip gun if aiming left ---
        gunSprite.localScale = aimDir.x < 0 ? new Vector3(1f, -1f, 1f) : Vector3.one;
    }

    void SwitchGun(GunSO newData)
    {
        data = newData;
        gunSpriteRend.sprite = data.sprite;
        crosshairManager.SetCrosshair(data);
    }

    void TryShooting()
    {
        // escape clauses
        if (!input.IsPrimary) return;
        if (isFiring) return;
        if (ammoConsumed) return;
        if (Time.time < nextFire) return;
        if (stats != null && stats.OnRamp) return;

        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        isFiring = true;

        // semi-auto: consume one shot per click
        if (!data.isAuto)
            ammoConsumed = true;

        int burstCount = data.isBurst ? data.burstCount : 1;

        for (int i = 0; i < burstCount; i++)
        {
            // --- Recoil ---
            StartCoroutine(Recoil());

            // --- Fire Type ---
            if (data.isShotgun)
                FireShotgun();
            else
                FireBullet();

            // --- Burst Delay ---
            if (data.isBurst)
                yield return new WaitForSeconds(data.burstRate);
        }

        // --- Cooldown before next shot ---
        nextFire = Time.time + data.fireRate;

        isFiring = false;
    }

    void FireBullet()
    {
        if (!bulletPrefab) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, bulletFolder);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = aimDir * data.bulletSpeed;

        Destroy(bullet, 5f);
    }

    void FireShotgun()
    {
        if (!bulletPrefab) return;

        for (int i = 0; i < data.spreadCount; i++)
        {
            float spread = Random.Range(-data.spreadAngle * 0.5f, data.spreadAngle * 0.5f);
            Vector2 spreadDir = Quaternion.Euler(0, 0, spread) * aimDir;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, bulletFolder);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = spreadDir * data.bulletSpeed;

            Destroy(bullet, 5f);
        }
    }

    IEnumerator Recoil()
    {
        Vector3 start = gunSprite.localPosition;
        Vector3 target = start - gunSprite.right * recoilDistance;
        float dur = Mathf.Min(recoilDuration, data.fireRate * 0.8f);

        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            gunSprite.localPosition = Vector3.Lerp(start, target, t / dur);
            yield return null;
        }

        t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            gunSprite.localPosition = Vector3.Lerp(target, start, t / dur);
            yield return null;
        }

        gunSprite.localPosition = start;
    }
}
