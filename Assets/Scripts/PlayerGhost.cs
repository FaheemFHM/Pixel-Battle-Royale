using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    [SerializeField] private Color rampColour;
    private SpriteRenderer[] rends;

    private void Awake()
    {
        rends = GetComponentsInChildren<SpriteRenderer>(true);
    }

    public void SetRamp(bool onRamp)
    {
        Color c = onRamp ? rampColour : Color.white;
        foreach (SpriteRenderer r in rends) r.color = c;
    }
}
