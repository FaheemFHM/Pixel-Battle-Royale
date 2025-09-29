using UnityEngine;

[CreateAssetMenu(fileName = "GunSO", menuName = "Scriptable Objects/GunSO")]
public class GunSO : ScriptableObject
{
    public Sprite sprite;

    [Range(0.01f, 1f)] public float fireRate;

    [Range(5f, 15f)] public float bulletSpeed;
    [Range(1, 10)] public int bulletDamage;

    public bool useCustomCrosshair;
    public Sprite crosshairSprite;
    [Range(0.1f, 2f)] public float crosshairScale;
}
