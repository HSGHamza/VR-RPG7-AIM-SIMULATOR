using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

public class TankController : MonoBehaviour
{

	private TankCamera tankCamera;
	private RadarSystem radar;
	public Tank TargetTank;

	void Start ()
	{
		tankCamera = this.GetComponent<TankCamera> ();
		radar = this.GetComponent<RadarSystem> ();
		if(tankCamera!=null && TargetTank!= null)
			tankCamera.AddCameras(TargetTank.gameObject);
	}

	void KeyController(){
		// This is an Input controller
		if (TargetTank == null || Time.timeScale == 0)
			return;

		MouseLock.MouseLocked = true;

		if (Input.GetButton ("Fire1")) {
			TargetTank.FireWeapon ();
		}
		if (Input.GetButtonDown ("Fire2")) {
			
			tankCamera.SwitchCameras();
		}
		if(Input.GetKeyDown(KeyCode.Q)){
			TargetTank.SwitchWeapon ();
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			TankGame.TankGameManager.Pause();
		}
	
		Vector2 moveVector = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		TargetTank.Move (moveVector);

		Vector2 aimVector = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
		TargetTank.Aim (aimVector);
	}


	void Update ()
	{

		if (TargetTank == null) {
			TankPlayer tankTarget = (TankPlayer)GameObject.FindObjectOfType (typeof(TankPlayer));
			if (tankTarget != null && tankTarget.IsMine && tankTarget.tank!=null) {
				TargetTank = tankTarget.tank;
				if(tankCamera!=null)
					tankCamera.AddCameras(TargetTank.gameObject);
			}

			if (tankCamera != null) {
				if (tankCamera.Target == null && tankTarget !=null && tankTarget.tank!=null) {
					if (tankTarget.tank.MainGun) {
						tankCamera.Target = TargetTank.MainGun.gameObject;
					} else {
						if (tankTarget.tank.Turret) {
							tankCamera.Target = TargetTank.Turret.gameObject;
						} else {
							tankCamera.Target = TargetTank.gameObject;
						}
					}
				}
			}
		}
		if(radar && TargetTank)
			radar.Player = TargetTank.gameObject;
		KeyController();
	}
}
