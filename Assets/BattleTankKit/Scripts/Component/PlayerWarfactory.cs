using UnityEngine;
using HWRWeaponSystem;
using UnityEngine.SceneManagement;

public class PlayerWarfactory : DamageManager
{
    [HideInInspector]
    public GameObject LatestHit;

    private bool gameEnded = false;

    public void ApplyDamage(DamagePack damage)
    {
        if (HP < 0 || gameEnded)
            return;

        LatestHit = damage.Owner;

        // ONE HIT = DEAD
        HP = 0;
        gameEnded = true;

        Debug.Log("PLAYER DEAD");

        Dead();
        EndGame();
    }

    void EndGame()
    {
        Debug.Log("GAME OVER: Base Destroyed");
        SceneManager.LoadScene("Menu 3D");
        // ✅ Send the text to the UI manager
        if (RocketSessionManager.Instance != null)
        {
            RocketSessionManager.Instance.SaveSession();
            Debug.Log("Rocket session saved");
        }
    }
}
