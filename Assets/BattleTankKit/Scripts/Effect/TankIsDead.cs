using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

public class TankIsDead : MonoBehaviour
{

	public int ScorePlus = 1;

	void Start ()
	{
	
	}

	// OnDead will called once after tank is dead.
	public void OnDead ()
	{
		if (this.gameObject.GetComponent<Tank> ()) {
			if (TankGame.TankGameManager != null && TankGame.TankGameManager.TankControl != null && TankGame.TankGameManager.TankControl.TargetTank != null) {
				if (this.gameObject.GetComponent<Tank> ().LatestHit == TankGame.TankGameManager.TankControl.TargetTank.gameObject) {
					TankGame.PlayerScore += ScorePlus;
				}
			}
		}
	}
}
