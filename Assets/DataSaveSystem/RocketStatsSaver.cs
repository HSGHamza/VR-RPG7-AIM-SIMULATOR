using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class RocketStatsSaver
{
    private static string FolderPath =>
        Path.Combine(Application.persistentDataPath, "RocketSessions");

    // Save a whole session list of records
    public static void SaveSession(List<RocketFlightRecord> records)
    {
        if (records == null || records.Count == 0)
        {
            Debug.LogWarning("No rocket records to save.");
            return;
        }

        // Build CSV header
        string csv = "HitTarget,TargetName,Range,FlightTime,ImpactSpeed,MaxSpeed,MaxAltitude,Dispersion,Timestamp\n";

        // Add each record
        foreach (var r in records)
        {
            csv += $"{r.hitTarget},{r.targetName},{r.range},{r.flightTime},{r.impactSpeed},{r.maxSpeed},{r.maxAltitude},{r.dispersion},{r.timestamp}\n";
        }

        // Ensure folder exists
        if (!Directory.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

        // Timestamped filename for the session
        string filePath = Path.Combine(FolderPath, $"Session_{System.DateTime.Now:yyyyMMdd_HHmmss}.csv");

        try
        {
            File.WriteAllText(filePath, csv);
            Debug.Log($"Rocket session saved to: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save session: {e.Message}");
        }
    }
}
