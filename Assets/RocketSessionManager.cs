using System.Collections.Generic;
using UnityEngine;

public class RocketSessionManager : MonoBehaviour
{
    public static RocketSessionManager Instance;

    public List<RocketFlightRecord> sessionRecords = new List<RocketFlightRecord>();
    public float sessionStartTime;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 🔒 persists across scene
        sessionStartTime = Time.time;
    }

    // ➕ Add rocket stats to current session
    public void AddRecord(RocketFlightRecord record)
    {
        sessionRecords.Add(record);
    }

    // 💾 Save everything at once
    public void SaveSession()
    {
        RocketStatsSaver.SaveSession(sessionRecords);
        sessionRecords.Clear(); // optional
    }
}
