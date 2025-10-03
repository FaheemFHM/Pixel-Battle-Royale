using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] private Transform crosshair;

    [SerializeField] private Sprite crosshairSpriteDefault;
    [SerializeField] private float crosshairScaleDefault;

    [SerializeField] [Range(1f, 10f)] private float maxDist = 3f;

    private InputManager input;

    private void Awake()
    {
        input = GetComponent<InputManager>();
    }

    private void OnEnable()
    {
        crosshair.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        crosshair.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        bool useAim = input.Look.sqrMagnitude > 0.01f;
        Vector2 aimDir = useAim ? input.Look.normalized : input.Move.normalized;
        float distance = Mathf.Clamp01(useAim ? input.Look.magnitude : input.Move.magnitude) * maxDist;

        crosshair.localPosition = aimDir * distance;
    }

    public void SetCrosshair(GunSO data)
    {
        if (data == null || !data.useCustomCrosshair)
        {
            crosshair.GetComponent<SpriteRenderer>().sprite = crosshairSpriteDefault;
            crosshair.localScale = Vector3.one * crosshairScaleDefault;
        }
        else
        {
            crosshair.GetComponent<SpriteRenderer>().sprite = data.crosshairSprite;
            crosshair.localScale = Vector3.one * data.crosshairScale;
        }
    }
}
