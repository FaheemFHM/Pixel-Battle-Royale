using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statKills;
    [SerializeField] private TextMeshProUGUI statDeaths;
    [SerializeField] private TextMeshProUGUI statAccuracy;
    [SerializeField] private TextMeshProUGUI statLevel;

    public void SetKills(int kills) => statKills.text = kills.ToString();
    public void SetDeaths(int deaths) => statDeaths.text = deaths.ToString();
    public void SetAccuracy(float acc) => statAccuracy.text = ((int)(acc * 100)).ToString() + "%";
    public void SetLevel(int level) => statLevel.text = level.ToString();
}
