using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionPrefab;
    [SerializeField][Range(1, 12)] private int optionCount = 5;
    [SerializeField][Range(0f, 5f)] private float wheelRadius = 3f;

/*    private void Start()
    {
        Setup();
    }

    void Setup()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (!Application.isPlaying)
                DestroyImmediate(transform.GetChild(i).gameObject);
            else
                Destroy(transform.GetChild(i).gameObject);
        }

        float angleStep = 360f / optionCount;

        for (int i = 0; i < optionCount; i++)
        {
            float angle = (i * angleStep + 90f) * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * wheelRadius;

            Instantiate(optionPrefab, transform.position + pos, Quaternion.identity, transform);
        }
    }*/
}
