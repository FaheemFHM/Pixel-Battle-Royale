using UnityEngine;
using System.Collections;

public class GunManager : MonoBehaviour
{
    [Header("Orbit")]
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform gunSprite;
    [SerializeField][Range(0f, 720f)] private float orbitSpeed = 120f;
    [SerializeField][Range(0f, 1f)] private float orbitDistance = 0.35f;
    [SerializeField] private bool isClockwise = false;

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletFolder;
    [SerializeField] private GunSO data;

    [Header("Recoil")]
    [SerializeField][Range(0.01f, 0.1f)] private float recoilDuration = 0.05f;
    [SerializeField][Range(0.01f, 0.25f)] private float recoilDistance = 0.1f;
    private Vector3 recoilPosStart;
    private Vector3 recoilPos;
    private float recoilDur;

    private Transform firePoint;
    private InputManager input;
    private Vector2 aimDir = Vector2.right;
    private Vector2 startPos;
    private float currentAngle = 0f;

    private float fireCooldown = 0f;

    private void Awake()
    {
        // components
        input = GetComponent<InputManager>();

        // variables
        startPos = pivot.localPosition;
        firePoint = gunSprite.GetChild(0);

        // recoil
        recoilDur = recoilDuration < data.fireRate ? recoilDuration : data.fireRate * 0.8f;
    }

    private void Update()
    {
        TryShooting();
    }

    private void LateUpdate()
    {
        if (input == null) return;

        // --- Update Aim Direction ---
        if (input.Look.sqrMagnitude > 0.01f) aimDir = input.Look.normalized;
        else if (input.Move.sqrMagnitude > 0.01f) aimDir = input.Move.normalized;

        // --- Orbit the gun around pivot ---
        currentAngle += (isClockwise ? -1 : 1) * orbitSpeed * Time.deltaTime;
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitDistance;
        pivot.localPosition = startPos + offset;

        // --- Rotate the gun to aim ---
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        gunSprite.rotation = Quaternion.Euler(0f, 0f, angle);

        // --- Flip gun if aiming left ---
        if (aimDir.x < 0) gunSprite.localScale = new Vector3(1f, -1f, 1f);
        else gunSprite.localScale = Vector3.one;
    }

    void TryShooting()
    {
        if (fireCooldown > 0f) fireCooldown -= Time.deltaTime;
        else if (input.IsPrimary) Shoot();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, bulletFolder);

        bullet.GetComponent<Rigidbody2D>().linearVelocity = aimDir * data.bulletSpeed;

        Destroy(bullet, 5f);
        
        fireCooldown = data.fireRate;

        StartCoroutine(Recoil());
    }

    IEnumerator Recoil()
    {
        recoilPosStart = gunSprite.localPosition;
        recoilPos = recoilPosStart - gunSprite.right * recoilDistance;

        float t = 0f;
        while (t < recoilDur)
        {
            t += Time.deltaTime;
            gunSprite.localPosition = Vector3.Lerp(startPos, recoilPos, t / recoilDur);
            yield return null;
        }

        t = 0f;
        while (t < recoilDur)
        {
            t += Time.deltaTime;
            gunSprite.localPosition = Vector3.Lerp(recoilPos, startPos, t / recoilDur);
            yield return null;
        }

        gunSprite.localPosition = startPos;
    }
}
