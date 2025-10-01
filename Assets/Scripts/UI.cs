using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private RadialFill staminaBar;

    public void SetStamina(float val)
    {
        staminaBar.SetValue(val);
        staminaBar.gameObject.SetActive(val < 0.99f);
    }
}
