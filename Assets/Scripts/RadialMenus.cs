using UnityEngine;

public class RadialMenus : MonoBehaviour
{
    public RadialMenu emoteMenu;
    public EmoteManager emoteManager;
    private InputManager input;

    private Vector2 aimDir = Vector2.up;
    private bool emotesActive = false;

    private void Start()
    {
        input = transform.root.GetComponentInChildren<InputManager>();
        foreach (Transform t in transform) t.gameObject.SetActive(false);
        emoteMenu.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (input == null) input = transform.root.GetComponentInChildren<InputManager>();

        input.OnEmote += ToggleEmoteWheel;
        input.OnBack += Close;
    }

    private void OnDisable()
    {
        emotesActive = false;
        emoteMenu.gameObject.SetActive(false);

        foreach (Transform t in transform) t.gameObject.SetActive(false);

        input.OnEmote -= ToggleEmoteWheel;
        input.OnBack -= Close;
    }

    private void LateUpdate()
    {
        if (!emotesActive) return;

        Vector2 aim = input.IsLooking ? input.Look : input.Move;
        if (aim.sqrMagnitude > 0.01f) aimDir = aim.normalized;

        emoteMenu.SetAimDir(aimDir);
    }

    void ToggleEmoteWheel(bool isPressing)
    {
        // We only care about button press, not release
        if (!isPressing) return;

        emotesActive = !emotesActive;

        if (emotesActive)
        {
            // --- Open wheel ---
            emoteMenu.gameObject.SetActive(true);
        }
        else
        {
            // --- Close and show selected emote ---
            emoteMenu.gameObject.SetActive(false);

            Sprite selected = emoteMenu.GetSprite();
            if (selected != null)
                emoteManager.ShowEmote(selected);
        }
    }

    void Close()
    {
        // --- Close without showing emote ---
        if (!emotesActive) return;

        emotesActive = false;
        emoteMenu.gameObject.SetActive(false);
    }
}
