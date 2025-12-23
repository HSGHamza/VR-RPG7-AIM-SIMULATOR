using UnityEngine;
using UnityEditor;
using System.Collections;
using HWRWeaponSystem;

public class EditorTankCreator : MonoBehaviour
{

	[MenuItem ("Window/BattleTank/Tank/Create Tank")]
	static void CreateTank ()
	{
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.GetComponent<Rigidbody> () == null)
				Selection.activeGameObject.AddComponent<Rigidbody> ();
			if (Selection.activeGameObject.GetComponent<BoxCollider> () == null)
				Selection.activeGameObject.AddComponent<BoxCollider> ();
			if (Selection.activeGameObject.GetComponent<Tank> () == null)
				Selection.activeGameObject.AddComponent<Tank> ();

		}
	}

	[MenuItem ("Window/BattleTank/Tank/Create Player Tank")]
	static void CreatePlayerTank ()
	{
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.GetComponent<Rigidbody> () == null)
				Selection.activeGameObject.AddComponent<Rigidbody> ();
			if (Selection.activeGameObject.GetComponent<BoxCollider> () == null)
				Selection.activeGameObject.AddComponent<BoxCollider> ();
			if (Selection.activeGameObject.GetComponent<TankPlayer> () == null || Selection.activeGameObject.GetComponent<Tank> () == null)
				Selection.activeGameObject.AddComponent<TankPlayer> ();

			if (Selection.activeGameObject.GetComponent<TankPlayer> () != null) {
				Selection.activeGameObject.GetComponent<TankPlayer> ().IsMine = true;
			}

			if (Selection.activeGameObject.GetComponent<TankAI> () != null) {
				DestroyImmediate (Selection.activeGameObject.GetComponent<TankAI> ());
			}
		}
	}

	[MenuItem ("Window/BattleTank/Tank/Create AI Tank")]
	static void CreateAITank ()
	{
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.GetComponent<Rigidbody> () == null)
				Selection.activeGameObject.AddComponent<Rigidbody> ();
			if (Selection.activeGameObject.GetComponent<BoxCollider> () == null)
				Selection.activeGameObject.AddComponent<BoxCollider> ();
			if (Selection.activeGameObject.GetComponent<TankAI> () == null || Selection.activeGameObject.GetComponent<Tank> () == null)
				Selection.activeGameObject.AddComponent<TankAI> ();

			if (Selection.activeGameObject.GetComponent<TankAI> () != null) {
				NavigatorInstance prefab = (NavigatorInstance)AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Prefabs/Game/Navigator.prefab", typeof(NavigatorInstance));
				Selection.activeGameObject.GetComponent<TankAI> ().Navigator = prefab;
			}

			if (Selection.activeGameObject.GetComponent<TankPlayer> () != null) {
				DestroyImmediate (Selection.activeGameObject.GetComponent<TankPlayer> ());
			}
		}
	}

	[MenuItem ("Window/BattleTank/Game/Game Setup")]
	static void SetupGame ()
	{

		TankCamera tankCam = (TankCamera)GameObject.FindObjectOfType (typeof(TankCamera));

		if (tankCam == null) {
			Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Prefabs/Game/TankCamera.prefab", typeof(GameObject));
			GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			clone.transform.position = Vector3.zero;
			clone.name = "TankCamera";
		}

		TankHUD hud = (TankHUD)GameObject.FindObjectOfType (typeof(TankHUD));
		if (hud == null) {
			Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Prefabs/Game/TankHUD.prefab", typeof(GameObject));
			GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			clone.transform.position = Vector3.zero;
			clone.name = "TankHUD";
		}

		GameManager gameManager = (GameManager)GameObject.FindObjectOfType (typeof(GameManager));
		if (gameManager == null) {
			Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Prefabs/Game/GameManager.prefab", typeof(GameObject));
			GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			clone.transform.position = Vector3.zero;
			clone.name = "GameManager";
		}

		WeaponSystemManager weaponManager = (WeaponSystemManager)GameObject.FindObjectOfType (typeof(WeaponSystemManager));
		if (weaponManager == null) {
			Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Component/WeaponSystem/WeaponSystem.prefab", typeof(GameObject));
			GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			clone.transform.position = Vector3.zero;
			clone.name = "WeaponSystem";
		}
	}

	[MenuItem ("Window/BattleTank/Game/Create Spawner")]
	static void CreateSpawn ()
	{
		Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Prefabs/Game/Spawner.prefab", typeof(GameObject));
		GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
		clone.transform.position = Vector3.zero;
		clone.name = "Spawner";
	}

	[MenuItem ("Window/BattleTank/Weapons/Attach 75mm Cannon")]
	static void Attach75mmWeapon ()
	{
		if (Selection.activeGameObject != null) {
			Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Prefabs/Weapons/75mmCannon.prefab", typeof(GameObject));
			GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			clone.transform.SetParent (Selection.activeGameObject.transform);
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localRotation = Quaternion.identity;
			clone.transform.localScale = Vector3.one;
		}
	}

	[MenuItem ("Window/BattleTank/Weapons/Attach 7mm Machinegun")]
	static void Attach7mmMG ()
	{
		if (Selection.activeGameObject != null) {
			Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Prefabs/Weapons/7mmMachinegun.prefab", typeof(GameObject));
			GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			clone.transform.SetParent (Selection.activeGameObject.transform);
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localRotation = Quaternion.identity;
			clone.transform.localScale = Vector3.one;
		}
	}
	[MenuItem ("Window/BattleTank/Weapons/Attach Tow Missile Launcher")]
	static void AttachTow ()
	{
		if (Selection.activeGameObject != null) {
			Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/BattleTankKit/Prefabs/Weapons/TOWMissileLauncher.prefab", typeof(GameObject));
			GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			clone.transform.SetParent (Selection.activeGameObject.transform);
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localRotation = Quaternion.identity;
			clone.transform.localScale = Vector3.one;
		}
	}

	[MenuItem ("Window/BattleTank/Effects/Track Animation")]
	static void CreateTrackAnimation ()
	{
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.GetComponent<TankTrackAnimation> () == null) {
				Selection.activeGameObject.AddComponent<TankTrackAnimation> ();
			}
		}
	}

	[MenuItem ("Window/BattleTank/Effects/Decay Effect")]
	static void CreateDecayEffect ()
	{
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.GetComponent<DamageDecay> () == null) {
				Selection.activeGameObject.AddComponent<DamageDecay> ();
			}
		}
	}
}
