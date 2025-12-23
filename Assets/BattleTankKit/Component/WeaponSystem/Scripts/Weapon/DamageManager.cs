using UnityEngine;
using System.Collections;

namespace HWRWeaponSystem
{
	public class DamageManager : MonoBehaviour
	{
		public GameObject Effect;
		public int HP = 100;
		private ObjectPool objPool;


		private void Awake ()
		{
			objPool = this.GetComponent<ObjectPool> ();	
		}
	
		private void Start ()
		{

		}

		public void ApplyDamage (int damage)
		{
			if (HP < 0)
				return;

			HP -= damage;
			if (HP <= 0) {
				Dead ();
			}
		}

		public void Dead ()
		{
			if (Effect) {
				if (WeaponSystem.Pool != null && Effect.GetComponent<ObjectPool>()) {
					WeaponSystem.Pool.Instantiate (Effect, transform.position, transform.rotation);
				} else {
					GameObject.Instantiate (Effect, transform.position, transform.rotation);
				}
			}
			if (objPool != null) {
				objPool.Destroying ();
			} else {
				Destroy (this.gameObject);
			}
			this.gameObject.SendMessage ("OnDead", SendMessageOptions.DontRequireReceiver);
		}

	}
}
