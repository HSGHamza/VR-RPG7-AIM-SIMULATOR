using UnityEngine;
using TMPro;

public class HealthDropdown : MonoBehaviour
{
    public TMP_Dropdown healthDropdown;

    void Start()
    {
        // Optional: sync dropdown with current value
        ApplyHealth(healthDropdown.value);
    }

    public void OnHealthChanged(int index)
    {
        ApplyHealth(index);
    }

    void ApplyHealth(int index)
    {
        int selectedHealth = 200; // default

        switch (index)
        {
            case 0:
                selectedHealth = 100;
                break;
            case 1:
                selectedHealth = 200;
                break;
            case 2:
                selectedHealth = 300;
                break;
            case 3:
                selectedHealth = 400;
                break;
            case 4:
                selectedHealth = 500;
                break;
        }

        GameSettings.Instance.baseHealth = selectedHealth;

        Debug.Log("Base health set to: " + selectedHealth);
    }
}
