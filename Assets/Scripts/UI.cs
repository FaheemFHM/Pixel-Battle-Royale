using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private RadialFill staminaBar;

    [SerializeField] private TextMeshProUGUI statKills;
    [SerializeField] private TextMeshProUGUI statDeaths;
    [SerializeField] private TextMeshProUGUI statAccuracy;
    [SerializeField] private TextMeshProUGUI statLevel;

    public void SetStamina(float val)
    {
        staminaBar.SetValue(val);
        staminaBar.gameObject.SetActive(val < 0.99f);
    }

    public void SetKills(int kills) => statKills.text = kills.ToString();
    public void SetDeaths(int deaths) => statDeaths.text = deaths.ToString();
    public void SetAccuracy(float acc) => statAccuracy.text = ((int)(acc * 100)).ToString() + "%";
    public void SetLevel(int level) => statLevel.text = level.ToString();
}
