using UnityEngine;

public class Ramps : MonoBehaviour
{
    [SerializeField] private int fromLevel;   // level at bottom of ramp
    [SerializeField] private int toLevel;     // level at top of ramp

    private void OnTriggerExit2D(Collider2D other)
    {
        // if there isnt a player move component, return
        var s = other.GetComponent<PlayerState>();
        if (s == null) return;

        // get last input direction
        Vector2 dir = s.PrevDir;

        // going "up" the ramp
        if (dir.y > 0.1f && s.Level == fromLevel) s.Level = toLevel;
        // going "down" the ramp
        else if (dir.y < -0.1f && s.Level == toLevel) s.Level = fromLevel;
    }
}
