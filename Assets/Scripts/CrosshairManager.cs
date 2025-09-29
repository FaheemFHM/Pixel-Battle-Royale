using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] private Transform crosshair;
    [SerializeField] [Range(1f, 10f)] private float maxDist = 3f;

    private InputManager input;
    private Camera mainCam;

    private void Awake()
    {
        input = GetComponent<InputManager>();
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        Vector2 aimDir = Vector2.zero;
        float distance = 0f;

        bool useAim = input.Look.sqrMagnitude > 0.01f;
        aimDir = useAim ? input.Look.normalized : input.Move.normalized;
        distance = Mathf.Clamp01(useAim ? input.Look.magnitude : input.Move.magnitude) * maxDist;

        crosshair.localPosition = aimDir * distance;
    }
}
