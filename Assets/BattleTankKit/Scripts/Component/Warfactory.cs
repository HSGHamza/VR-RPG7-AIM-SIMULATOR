using UnityEngine;
using HWRWeaponSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR;
using System.Collections.Generic;

public class Warfactory : DamageManager
{
    [HideInInspector]
    public GameObject LatestHit;

    private bool gameEnded = false;

    // UI
    public TextMeshProUGUI healthText;

    // XR Input
    private InputDevice rightController;
    private bool bPressedLastFrame = false;

    private void Start()
    {
        // 🔗 Apply selected base health
        if (GameSettings.Instance != null)
        {
            HP = GameSettings.Instance.baseHealth;
        }

        UpdateHealthUI();
        Debug.Log("Base HP loaded: " + HP);

        // 🎮 Get right-hand controller (Meta Quest 2)
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
        {
            rightController = devices[0];
            Debug.Log("Right controller detected");
        }
    }

    private void Update()
    {
        if (gameEnded) return;

        // 🅱 B Button → End Game
        if (rightController.isValid)
        {
            if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool bPressed))
            {
                if (bPressed && !bPressedLastFrame)
                {
                    Debug.Log("🅱 B Button Pressed → Ending Game");
                    EndGame();
                }

                bPressedLastFrame = bPressed;
            }
        }
    }

    public void ApplyDamage(DamagePack damage)
    {
        if (HP <= 0 || gameEnded)
            return;

        LatestHit = damage.Owner;
        HP -= damage.Damage;

        UpdateHealthUI();

        if (HP <= 0)
        {
            HP = 0;
            UpdateHealthUI();

            Debug.Log("Base destroyed by damage");
            EndGame();
        }
    }

    // Update health UI
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Base Health: " + HP.ToString();
        }
    }

    void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        // 💾 Save rocket session
        if (RocketSessionManager.Instance != null)
        {
            RocketSessionManager.Instance.SaveSession();
            Debug.Log("Rocket session saved");
        }
        else
        {
            Debug.LogWarning("RocketSessionManager instance NOT found");
        }

        Debug.Log("GAME OVER → Loading Menu");
        SceneManager.LoadScene("Menu 3D");
    }
}
