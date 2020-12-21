using UnityEngine;

[CreateAssetMenu(menuName = "HitPoints")]

public class HitPoints : ScriptableObject
{
    public float value;
    public bool playerIsHit;
    public float playerStartingHitPoints = 5;
    public bool instantDeath;

    private void OnEnable()
    {
        value = playerStartingHitPoints;
        instantDeath = false;
    }
}
