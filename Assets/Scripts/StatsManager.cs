using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private UI ui;
    private UIWorld uiWorld;

    [Header("Health")]
    public int health;
    public int maxHealth = 100;

    [Header("Stamina")]
    public float stamina;
    public float staminaMax = 100;
    public float staminaDepletionSprint = 10f;
    public float staminaRegenDelay = 2f;
    public float staminaRegenRate = 15f;
    public bool isSprinting;
    public bool sprintConsumed;
    private float regenTimer;

    private void Start()
    {
        health = maxHealth;
        uiWorld = transform.root.GetComponentInChildren<UIWorld>();
        uiWorld.SetMaxHealth(maxHealth);
        uiWorld.SetHealth(health);

        stamina = staminaMax;
        ui = FindFirstObjectByType<UI>();
        ui.SetStamina(1f);
    }

    private void Update()
    {
        // stamina management
        if (isSprinting && !sprintConsumed)
        {
            // drain stamina
            stamina -= staminaDepletionSprint * Time.deltaTime;

            regenTimer = staminaRegenDelay;

            // if depleted
            if (stamina < 0f)
            {
                stamina = 0f;
                sprintConsumed = true;
            }
        }
        else if (regenTimer > 0f)
        {
            // wait for recharge delay
            regenTimer -= Time.deltaTime;
        }
        else
        {
            // do regen
            stamina += staminaRegenRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, staminaMax);
        }

        ui.SetStamina(stamina / staminaMax);
    }
}
