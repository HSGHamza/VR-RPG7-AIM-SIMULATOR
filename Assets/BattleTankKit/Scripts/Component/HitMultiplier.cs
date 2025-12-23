using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

public class HitMultiplier : MonoBehaviour {

	public float DamageMultiplier = 2;
	private Tank tank;
	void Start () {
		tank = this.transform.root.GetComponentInChildren<Tank>();
	}
	
	public void ApplyDamage (DamagePack damage)
	{
		if(tank == null)
		return;

		if (tank.HP < 0)
			return;

		tank.LatestHit = damage.Owner;
		tank.HP -= (int)((float)damage.Damage * DamageMultiplier);
		if (tank.HP <= 0) {
			tank.Dead ();
		}
	}
}
