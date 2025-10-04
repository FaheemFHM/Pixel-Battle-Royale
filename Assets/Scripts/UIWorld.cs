using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWorld : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private int maxHealth;
    [SerializeField] private Slider staminaBar;

    private void OnDisable()
    {
        healthBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(false);
    }

    public void SetMaxHealth(int val)
    {
        healthBar.maxValue = val;
        maxHealth = val;
    }

    public void SetHealth(int val)
    {
        healthBar.value = val;
        healthBar.gameObject.SetActive(val < maxHealth);
    }

    public void SetStamina(float val)
    {
        staminaBar.value = val;
        staminaBar.gameObject.SetActive(val < 1f);
    }
}
