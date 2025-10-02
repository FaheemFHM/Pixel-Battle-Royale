using UnityEngine;

public class Ramps : MonoBehaviour
{
    [SerializeField][Range(0, 5)] private int fromLevel = 0;
    [SerializeField][Range(0, 5)] private int toLevel = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if there isnt a player move component, return
        var s = other.GetComponent<StatsManager>();
        if (s == null) return;

        // set ramp damping
        s.OnRamp = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // get player state component
        var s = other.GetComponent<StatsManager>();
        if (s == null) return;

        // get last input direction
        Vector2 dir = s.PrevDir;

        // going "up" the ramp
        if (dir.y > 0.1f && s.Level == fromLevel) s.Level = toLevel;

        // going "down" the ramp
        else if (dir.y < -0.1f && s.Level == toLevel) s.Level = fromLevel;

        // release ramp damping
        s.OnRamp = false;
    }
}
