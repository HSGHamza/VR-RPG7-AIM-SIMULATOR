using UnityEngine;

[CreateAssetMenu(fileName = "RocketFlightSettings", menuName = "RPG/Rocket Flight Settings")]
public class RocketFlightSettings : ScriptableObject
{
    [Header("Flight")]
    public float initialSpeed;
    public float thrustForce;
    public float burnTime;
    public float linearDrag;
    public float gravityMultiplier;
    public float autoDestroyTime;
}
