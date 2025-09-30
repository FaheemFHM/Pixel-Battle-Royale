using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button btnHost;
    [SerializeField] private Button btnJoin;
    [SerializeField] private Button btnQuit;

    [SerializeField] private Sprite[] selectedSprites;
    [SerializeField] private Sprite[] unselectedSprites;

    [SerializeField] private RectTransform arrowTransform;
    private float xArrowOffset;

    private void Start()
    {
        xArrowOffset = arrowTransform.anchoredPosition.x;

        // Selection listeners
        AddSelectionListeners(btnHost);
        AddSelectionListeners(btnJoin);
        AddSelectionListeners(btnQuit);

        // Set default selected button
        EventSystem.current.SetSelectedGameObject(btnHost.gameObject);
    }

    private void AddSelectionListeners(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // OnSelect
        var selectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        selectEntry.callback.AddListener((_) => OnButtonSelected(button));
        trigger.triggers.Add(selectEntry);

        // OnDeselect
        var deselectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Deselect };
        deselectEntry.callback.AddListener((_) => OnButtonDeselected(button));
        trigger.triggers.Add(deselectEntry);
    }

    private void OnButtonSelected(Button button)
    {
        int j = 0;
        foreach (Image i in button.GetComponentsInChildren<Image>())
        {
            i.sprite = selectedSprites[j];
            j++;
        }

        arrowTransform.anchoredPosition = button.GetComponent<RectTransform>().anchoredPosition + Vector2.right * xArrowOffset;
    }

    private void OnButtonDeselected(Button button)
    {
        int j = 0;
        foreach (Image i in button.GetComponentsInChildren<Image>())
        {
            i.sprite = unselectedSprites[j];
            j++;
        }
    }

    public void Host()
    {
        Debug.Log("Host clicked");
        // NetworkManager.Singleton.StartHost();
    }

    public void Join()
    {
        Debug.Log("Join clicked");
        // NetworkManager.Singleton.StartClient();
    }

    public void Quit()
    {
        Debug.Log("Quit clicked");
        Application.Quit();
    }
}
