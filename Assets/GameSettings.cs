using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public RocketFlightSettings currentRocketSettings;
    [Header("Base / Tank Settings")]
    public int baseHealth = 100;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
