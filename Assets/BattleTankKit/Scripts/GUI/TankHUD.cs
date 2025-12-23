using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HWRWeaponSystem;

public class TankHUD : MonoBehaviour
{

	public TankController TankController;
	public Text HPText;
	public Text AmmoText;
	public Text ScoreText;
	public GameObject GameMenu;
	void Start ()
	{
		if(GameMenu)
			GameMenu.SetActive(false);

		TankController = (TankController)GameObject.FindObjectOfType (typeof(TankController));
	}

	public void ShowMenu(bool active){
		if(GameMenu){
			GameMenu.SetActive(active);
			MouseLock.MouseLocked = !active;
		}
	}

	void Update ()
	{
		if (TankController != null && TankController.TargetTank != null) {

			if (AmmoText != null) {
				if (TankController.TargetTank.weapon != null) {
					WeaponLauncher weapon = TankController.TargetTank.weapon.GetCurrentWeapon ();
					if (weapon) {
						if(weapon.Reloading){
							AmmoText.text = Mathf.Floor((1-weapon.ReloadingProcess) * 100)+"%";
						}else{
							AmmoText.text = weapon.Ammo.ToString();
						}

					}
				}
			}

			if (HPText != null) {
				HPText.text = TankController.TargetTank.HP.ToString ();
			}
			if (ScoreText != null) {
				ScoreText.text = TankGame.PlayerScore.ToString ();
			}

		}
	}
}
