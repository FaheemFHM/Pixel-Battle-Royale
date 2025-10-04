using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EmoteManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve animScale;
    [SerializeField] private float animDur = 0.25f;
    [SerializeField] private float emoteDur = 2f;
    [SerializeField] private Sprite defaultEmote;
    [SerializeField] private Sprite defaultStyle;

    private Image emote;
    private Coroutine currentAnim;

    private void Awake()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = defaultStyle;
        emote = transform.GetChild(1).GetComponent<Image>();
        transform.localScale = Vector3.zero;
    }

    public void ShowEmote(Sprite s)
    {
        if (currentAnim != null) StopCoroutine(currentAnim);

        if (s == null) s = defaultEmote;
        emote.sprite = s;
        currentAnim = StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        transform.localScale = Vector3.zero;

        // --- Scale Up ---
        float t = 0f;
        while (t < animDur)
        {
            t += Time.deltaTime;
            float val = animScale.Evaluate(t / animDur);
            transform.localScale = Vector3.one * val;
            yield return null;
        }
        transform.localScale = Vector3.one;

        // --- Hold ---
        yield return new WaitForSeconds(emoteDur);

        // --- Scale Down (reverse animation) ---
        t = 0f;
        while (t < animDur)
        {
            t += Time.deltaTime;
            float val = animScale.Evaluate(1f - (t / animDur));
            transform.localScale = Vector3.one * val;
            yield return null;
        }

        transform.localScale = Vector3.zero;

        currentAnim = null;
    }
}
