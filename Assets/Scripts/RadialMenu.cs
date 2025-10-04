using UnityEngine;
using System.Collections.Generic;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionPrefab;
    [SerializeField][Range(0f, 5f)] private float wheelRadius = 3f;
    [SerializeField] private float highlightScale = 1.3f;
    [SerializeField] private Sprite[] sprites;

    private int optionCount;
    private List<Transform> options = new List<Transform>();
    private Vector2 dir;
    private int currentIndex = -1;

    private void Awake()
    {
        Setup();
    }

    void Setup()
    {
        options.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--) DestroyImmediate(transform.GetChild(i).gameObject);

        optionCount = sprites.Length;
        if (optionCount < 1) return;

        float angleStep = 360f / optionCount;

        for (int i = 0; i < optionCount; i++)
        {
            float angle = (i * angleStep + 90f) * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * wheelRadius;

            GameObject opt = Instantiate(optionPrefab, transform.position + pos, Quaternion.identity, transform);
            opt.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[i];
            options.Add(opt.transform);
        }
    }

    public void SetAimDir(Vector2 d)
    {
        dir = d;
        if (optionCount < 1) return;
        UpdateHighlight();
    }

    void UpdateHighlight()
    {
        if (dir.sqrMagnitude < 0.01f) return;

        float aimAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (aimAngle < 0) aimAngle += 360f;

        float angleStep = 360f / optionCount;
        int index = Mathf.RoundToInt((aimAngle - 90f) / angleStep);
        index = (index % optionCount + optionCount) % optionCount;

        if (index == currentIndex) return;

        if (currentIndex >= 0 && currentIndex < options.Count) options[currentIndex].localScale = Vector3.one;

        // highlight selection
        options[index].localScale = Vector3.one * highlightScale;
        currentIndex = index;
    }

    public Sprite GetSprite()
    {
        return sprites[currentIndex];
    }
}
