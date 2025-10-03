using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private UI ui;
    private UIWorld uiWorld;
    private PlayerGhost ghost;

    [Header("Health")]
    public int maxHealth = 100;
    public float healthRegenDelay = 3f;
    public float healthRegenRate = 10f;
    private float healthRegenTimer;
    private float healthRegenTimerStep;
    private int health = 0;
    public int Health
    {
        get => health;
        private set
        {
            int clamped = Mathf.Clamp(value, 0, maxHealth);
            if (clamped == health) return;
            health = clamped;
            uiWorld.SetHealth(health);
        }
    }

    [Header("Stamina")]
    public float stamina;
    public float staminaMax = 100;
    public float staminaDepletionSprint = 10f;
    public float staminaRegenDelay = 2f;
    public float staminaRegenRate = 15f;
    public bool isSprinting;
    public bool sprintConsumed;
    private float staminaRegenTimer;

    [Header("Performance")]
    private int kills = 0;
    public int Kills
    {
        get => kills;
        set
        {
            kills = value;
            ui.SetKills(kills);
        }
    }
    private int deaths = 0;
    public int Deaths
    {
        get => deaths;
        set
        {
            deaths = value;
            ui.SetDeaths(deaths);
        }
    }
    private int shotsHit;
    public int ShotsHit
    {
        get => shotsHit;
        set
        {
            shotsHit = value;
            int val = ShotsHit + ShotsMissed;
            Accuracy = val > 0 ? (ShotsHit / val) : 0f;
        }
    }
    private int shotsMissed;
    public int ShotsMissed
    {
        get => shotsMissed;
        set
        {
            shotsMissed = value;
            int val = ShotsHit + ShotsMissed;
            Accuracy = val > 0 ? (ShotsHit / val) : 0f;
        }
    }
    private float accuracy = 0f;
    public float Accuracy
    {
        get => accuracy;
        set
        {
            accuracy = value;
            ui.SetAccuracy(accuracy);
        }
    }

    [Header("Generic")]
    public int TeamId { get; set; } = 0;
    private int level = 0;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            ui.SetLevel(level);
        }
    }
    private bool onRamp = false;
    public bool OnRamp
    {
        get => onRamp;
        set
        {
            onRamp = value;
            ghost.SetRamp(value);
        }
    }
    public Vector2 PrevDir { get; set; } = Vector2.right;
    private bool isDead;

    private void Awake()
    {
        ghost = GetComponent<PlayerGhost>();
        uiWorld = transform.root.GetComponentInChildren<UIWorld>();
        ui = FindFirstObjectByType<UI>();
    }

    private void Start()
    {
        // health
        uiWorld.SetMaxHealth(maxHealth);
        Health = maxHealth;

        // stamina
        stamina = staminaMax;
        ui.SetStamina(1f);

        // stats
        Kills = 0;
        Deaths = 0;
        ShotsHit = 0;
        ShotsMissed = 0;

        // elevation
        Level = 0;
        OnRamp = false;
    }

    private void Update()
    {
        if (isDead) return;
        HandleHealthRegen();
        DepleteStamina();
    }

    public void EditHealth(int amount)
    {
        Health += amount;
        if (Health < 1) Die();
    }

    private void HandleHealthRegen()
    {
        if (healthRegenDelay > 0f) healthRegenDelay -= Time.deltaTime;
        else if (healthRegenTimerStep > 0f) healthRegenTimerStep -= Time.deltaTime;
        else if (Health < maxHealth)
        {
            Health++;
            healthRegenTimerStep = 1f / healthRegenRate;
        }
    }

    void DepleteStamina()
    {
        if (isSprinting && !sprintConsumed)
        {
            // drain stamina
            stamina -= staminaDepletionSprint * Time.deltaTime;

            staminaRegenTimer = staminaRegenDelay;

            // if depleted
            if (stamina < 0f)
            {
                stamina = 0f;
                sprintConsumed = true;
            }
        }
        else if (staminaRegenTimer > 0f)
        {
            // wait for recharge delay
            staminaRegenTimer -= Time.deltaTime;
        }
        else
        {
            // do regen
            stamina += staminaRegenRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, staminaMax);
        }

        ui.SetStamina(stamina / staminaMax);
    }

    void Die()
    {
        ui.enabled = false;
        uiWorld.enabled = false;
        GetComponent<PlayerMove>().Die();
        Deaths++;
        isDead = true;

    }
}
