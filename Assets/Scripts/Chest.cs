using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour
{
    [SerializeField] private Sprite openSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        float dur = 0.1f;

        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.75f, t / dur);
            yield return null;
        }

        GetComponent<SpriteRenderer>().sprite = openSprite;

        t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one * 0.75f, Vector3.one, t / dur);
            yield return null;
        }

        this.enabled = false;
    }
}
