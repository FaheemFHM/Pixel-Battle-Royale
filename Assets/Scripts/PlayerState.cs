using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public int Level { get; set; } = 0;
    public int TeamId { get; set; } = 0;
    public int Health { get; set; } = 100;
    public Vector2 PrevDir { get; set; } = Vector2.right;
}
