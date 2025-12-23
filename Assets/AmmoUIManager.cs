using UnityEngine;
using TMPro;

public class AmmoUIManager : MonoBehaviour
{
    public static AmmoUIManager Instance;

    public TextMeshProUGUI ammoText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateAmmo(int current, int max)
    {
        if (ammoText == null) return;

        ammoText.text = $"Ammo: {current} / {max}";
    }
}
