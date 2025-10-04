using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "Scriptable Objects/GunSO")]
public class GunSO : ScriptableObject
{
    [Header("General")]
    public bool isAuto = true;
    public Sprite sprite;

    [Header("Bullet")]
    public float bulletSpeed = 10f;
    public float range = 100f;
    public int damage = 1;

    [Header("Firing")]
    [Range(0f, 10f)] public float fireRate = 0.1f;

    [Header("Burst")]
    public bool isBurst = false;
    [Range(0f, 1f)] public float burstRate = 0.05f;
    [Range(1, 5)] public int burstCount = 3;

    [Header("Shotgun")]
    public bool isShotgun = false;
    [Range(0f, 45f)] public float spreadAngle = 5f;
    [Range(1, 10)] public int spreadCount = 5;

    [Header("Sounds")]
    public bool useCustomSound = false;
    public AudioClip[] shotClips;
    [Range(0f, 3f)] public float pitchMin = 1f;
    [Range(0f, 3f)] public float pitchMax = 1.2f;

    [Header("Crosshair")]
    public bool useCustomCrosshair = false;
    public Sprite crosshairSprite;
    [Range(0.1f, 2f)] public float crosshairScale = 1f;
}
