using UnityEngine;

[CreateAssetMenu(menuName = "HitPoints")]

public class HitPoints : ScriptableObject
{
    public float value;
    public bool playerIsHit;
    public float playerStartingHitPoints = 5;

    private void OnEnable()
    {
        value = playerStartingHitPoints;
    }
}
