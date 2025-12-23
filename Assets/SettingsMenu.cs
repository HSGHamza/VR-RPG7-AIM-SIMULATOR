using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [Header("Rocket Flight Presets")]
    public RocketFlightSettings realistic;
    public RocketFlightSettings normalized;

    public void SetRealistic()
    {
        GameSettings.Instance.currentRocketSettings = realistic;
        Debug.Log("Rocket mode set to REALISTIC");
    }

    public void SetNormalized()
    {
        GameSettings.Instance.currentRocketSettings = normalized;
        Debug.Log("Rocket mode set to NORMALIZED");
    }
}
