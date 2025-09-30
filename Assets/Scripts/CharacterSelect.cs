using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class Character
{
    public string text;
    public Image sprite;
    public Image arrow;
    public bool isActive;
}

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private Character[] characters;
    [SerializeField] private int characterIndex = 0;
    [SerializeField] private TextMeshProUGUI nameText;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(characters[0].sprite.gameObject);

        UpdateSelection();

        for (int i = 0; i < characters.Length; i++) AddSelectionListener(characters[i].sprite.gameObject, i);
    }

    public void NextCharacter()
    {
        characterIndex++;
        if (characterIndex >= characters.Length) characterIndex = 0;

        UpdateSelection();
    }

    public void PreviousCharacter()
    {
        characterIndex--;
        if (characterIndex < 0) characterIndex = characters.Length - 1;

        UpdateSelection();
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < characters.Length; i++) characters[i].arrow.enabled = false;
        characters[characterIndex].arrow.enabled = true;
        nameText.text = characters[characterIndex].text;
    }

    void SelectMe(GameObject g)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].sprite.gameObject == g)
            {
                characterIndex = i;
                UpdateSelection();
                break;
            }
        }
    }

    public void MakeChoice(GameObject g)
    {
        Debug.Log(g);
    }

    private void AddSelectionListener(GameObject g, int index)
    {
        EventTrigger trigger = g.GetComponent<EventTrigger>();
        trigger = g.AddComponent<EventTrigger>();
        
        var selectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        
        selectEntry.callback.AddListener((_) => SelectMe(g));
        trigger.triggers.Add(selectEntry);
    }
}
