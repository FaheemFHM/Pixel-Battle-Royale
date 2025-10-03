using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWorld : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private int maxHealth;

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
}
