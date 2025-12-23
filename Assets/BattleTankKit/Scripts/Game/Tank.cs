using UnityEngine;
using System.Collections;
using HWRWeaponSystem;

[RequireComponent (typeof(AudioSource))]
[RequireComponent (typeof(WeaponController))]
public class Tank : DamageManager
{

	public GameObject Turret;
	public GameObject MainGun;
	public Vector3 TurretBaseAxis = new Vector3 (0, 1, 0);
	public Vector3 MainGunBaseAxis = new Vector3 (1, 0, 0);

	public float TurretSpeed = 20;
	public float TankSpeed = 5;
	public float TurnSpeed = 20;

	public float MaingunMinTurnX = -10;
	public float MaingunMaxTurnX = 45;


	public float FlipRatio = 0.7f;
	private float rotationX = 0;
	private Vector3 turrentEulerAngle;
	private Vector3 maingunEulerAngle;
	private TankTrackAnimation track;
	private AudioSource audioSource;
	private Vector3 positionTemp;

	[HideInInspector]
	public WeaponController weapon;
	[HideInInspector]
	public GameObject LatestHit;
	[HideInInspector]
	public float AimingAngle;


	void Awake ()
	{
		track = GetComponentInChildren<TankTrackAnimation> ();
		audioSource = GetComponent<AudioSource>();
		weapon = this.transform.GetComponentInChildren (typeof(WeaponController)).GetComponent<WeaponController> ();
	}

	void Start ()
	{
		// Save all position and rotation of the gun.
		if (Turret != null) {
			turrentEulerAngle = Turret.transform.localEulerAngles;
		}
		if (MainGun != null) {
			maingunEulerAngle = MainGun.transform.localEulerAngles;
		}
	}

	void Update ()
	{
		// Damage the tank when it fliped 
		if (Vector3.Dot (this.transform.up, Vector3.up) < FlipRatio) {
			ApplyDamage (3);
		}
	}

	public void FireWeapon ()
	{
		if (weapon)
			weapon.LaunchWeapon ();
	}

	public void FireWeapon (int index)
	{
		if (weapon)
			weapon.LaunchWeapon (index);
	}

	public void SwitchWeapon ()
	{
		weapon.SwitchWeapon ();
	}

	public void Move (Vector2 moveVector)
	{
		if (Vector3.Dot (this.transform.up, Vector3.up) < FlipRatio)
			return;

		positionTemp = this.transform.position;
		this.transform.Rotate (new Vector3 (0, moveVector.x * TurnSpeed * Time.deltaTime, 0));
		this.transform.position += this.transform.forward * moveVector.y * TankSpeed * Time.deltaTime;
		// Track animation working
		if (track)
			track.MoveTrack (new Vector2 (Mathf.Clamp (moveVector.y + moveVector.x, -1, 1), 0));

		// increase Pitch when moving
		if(audioSource){
			audioSource.pitch = Mathf.Clamp((this.transform.position - positionTemp).magnitude * 100,1,2f);
			audioSource.spatialBlend = 1;
		}
	}


	public void MoveTo (NavigatorInstance navigator)
	{
		// This tank move along with navigator object.
		if (Vector3.Dot (this.transform.up, Vector3.up) < FlipRatio)
			return;

		Quaternion rotationTarget = navigator.transform.rotation;
		rotationTarget.eulerAngles = new Vector3 (this.transform.eulerAngles.x, rotationTarget.eulerAngles.y, this.transform.eulerAngles.z);
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation, rotationTarget, TurnSpeed * 0.1f * Time.deltaTime);
		positionTemp = this.transform.position;
		this.transform.position += this.transform.forward * TankSpeed * Time.deltaTime;

		// Track animation working
		if (track)
			track.MoveTrack (new Vector2 (1, 0));

		// increase Pitch when moving
		if(audioSource){
			audioSource.pitch = Mathf.Clamp((this.transform.position - positionTemp).magnitude * 100,1,2f);
			audioSource.spatialBlend = 1;
		}
	}


	public void Aim (Vector3 target)
	{
		// Aim to specific position
		float aimAngleTurret = 0;
		float aimAngleMaingun = 0;

		if (Turret != null) {
			Vector3 localTarget = Turret.transform.parent.InverseTransformPoint (target);
			Quaternion targetlook = Quaternion.LookRotation (localTarget - Turret.transform.localPosition);

			targetlook.eulerAngles = 
			new Vector3 (
				(TurretBaseAxis.x * targetlook.eulerAngles.y) + ((1 - TurretBaseAxis.x) * turrentEulerAngle.x), 
				(TurretBaseAxis.y * targetlook.eulerAngles.y) + ((1 - TurretBaseAxis.y) * turrentEulerAngle.y), 
				(TurretBaseAxis.z * targetlook.eulerAngles.y) + ((1 - TurretBaseAxis.z) * turrentEulerAngle.z)
			);

			Turret.transform.localRotation = Quaternion.Lerp (Turret.transform.localRotation, targetlook, TurretSpeed * Time.deltaTime * 0.1f);
			aimAngleTurret = Quaternion.Angle(Turret.transform.localRotation,targetlook);
		}

		if (MainGun != null && aimAngleTurret < 3) {

			Vector3 localTarget = MainGun.transform.parent.InverseTransformPoint (target);
			float distance = Vector2.Distance (new Vector2 (localTarget.x, localTarget.z), new Vector2 (MainGun.transform.localPosition.x, MainGun.transform.localPosition.z));
			float angle = Mathf.Atan (distance / localTarget.y) * Mathf.Rad2Deg;
			if (target.y < MainGun.transform.position.y) {
				angle = -angle;
			}
			Quaternion targetlook = MainGun.transform.localRotation;
			targetlook.eulerAngles = new Vector3 ((MainGunBaseAxis.x * angle) + maingunEulerAngle.x, (MainGunBaseAxis.y * angle) + maingunEulerAngle.y, (MainGunBaseAxis.z * angle) + maingunEulerAngle.z);
			MainGun.transform.localRotation = Quaternion.Lerp (MainGun.transform.localRotation, targetlook, TurretSpeed * Time.deltaTime * 0.1f);
			aimAngleMaingun = Quaternion.Angle(MainGun.transform.localRotation,targetlook);
		}
		AimingAngle = aimAngleTurret + aimAngleMaingun;
	}


	public void Aim (Vector2 aimVector)
	{
		// aim with cursor vector
		if (Turret != null) {
			float rotationY = Turret.transform.localEulerAngles.y + aimVector.x * TurretSpeed * Time.deltaTime;
			Turret.transform.localEulerAngles = 
				new Vector3 (
				(TurretBaseAxis.x * rotationY) + ((1 - TurretBaseAxis.x) * turrentEulerAngle.x),
				(TurretBaseAxis.y * rotationY) + ((1 - TurretBaseAxis.y) * turrentEulerAngle.y),
				(TurretBaseAxis.z * rotationY) + ((1 - TurretBaseAxis.z) * turrentEulerAngle.z)
			);

		}
		if (MainGun != null) {
			rotationX += aimVector.y * TurretSpeed * Time.deltaTime;
			rotationX = Mathf.Clamp (rotationX, MaingunMinTurnX, MaingunMaxTurnX);

			MainGun.transform.localEulerAngles = 
			new Vector3 (
				(MainGunBaseAxis.x * -rotationX) + maingunEulerAngle.x,
				(MainGunBaseAxis.y * -rotationX) + maingunEulerAngle.y, 
				(MainGunBaseAxis.z * -rotationX) + maingunEulerAngle.z
			);
		}
	}

	public void ApplyDamage (DamagePack damage)
	{
		if (HP < 0)
			return;

		LatestHit = damage.Owner;
		HP -= damage.Damage;
		if (HP <= 0) {
            FindObjectOfType<WaveController>()?.RegisterEnemyDeath();
            Dead();
		}
	}
}
