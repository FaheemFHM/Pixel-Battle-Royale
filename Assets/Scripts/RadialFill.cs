using UnityEngine;
using UnityEngine.UI;

public class RadialFill : MonoBehaviour
{
    private Image parentImage;
    private Image fillImage;

    private void Awake()
    {
        parentImage = GetComponent<Image>();
        fillImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void SetColour(Color c)
    {
        parentImage.color = c;
        fillImage.color = c;
    }

    public void SetValue(float val)
    {
        fillImage.fillAmount = Mathf.Clamp01(val);
    }
}
