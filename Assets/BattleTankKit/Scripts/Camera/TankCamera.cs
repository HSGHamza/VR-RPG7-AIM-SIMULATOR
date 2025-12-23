using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]
public class TankCamera : MonoBehaviour
{

	public GameObject Target;
	public Camera[] Cameras;

	public float TurnSpeedMult = 5;

	private int indexCamera;
	public Vector3 Offset = new Vector3 (0, 0.5f, 0);
	public float Distance = 5;
	public float Damping = 0.5f;



	public void SwitchCameras ()
	{
		indexCamera += 1;
		if (indexCamera >= Cameras.Length) {
			indexCamera = 0;
		}
		// disable all the cameras
		for (int i = 0; i < Cameras.Length; i++) {
			if (Cameras [i] && Cameras [i])
				Cameras [i].enabled = false;
			if (Cameras [i] && Cameras [i].GetComponent<AudioListener> ())
				Cameras [i].GetComponent<AudioListener> ().enabled = false;
		}
		// enable only selected
		if (Cameras [indexCamera]) {
			if (Cameras [indexCamera] && Cameras [indexCamera])
				Cameras [indexCamera].enabled = true;
			if (Cameras [indexCamera] && Cameras [indexCamera].GetComponent<AudioListener> ())
				Cameras [indexCamera].GetComponent<AudioListener> ().enabled = true;
		}
	}

	void Awake ()
	{
		AddCamera (this.gameObject.GetComponent<Camera> ());
	}

	public void AddCameras(GameObject root){
		// finding all the cameras in root object
		Camera[] cams = (Camera[])root.GetComponentsInChildren<Camera>();
		foreach(Camera ca in cams){
			AddCamera(ca);
		}
	}

	public void ClearCameras(){
		Cameras = null;
	}

	public void AddCamera (Camera cam)
	{
		Camera[] temp = new Camera[Cameras.Length + 1];
		Cameras.CopyTo (temp, 0);
		Cameras = temp;
		Cameras [temp.Length - 1] = cam;
	}

	void Start ()
	{

	}

	void UpdateCamera ()
	{
		if (!Target)
			return;
		
		Vector3 VectorUp = Target.transform.up;
		this.transform.LookAt (Target.transform.position - VectorUp * Distance);

		Vector3 positionTarget = (Target.transform.position + VectorUp * Distance) + Offset;
		this.transform.position = Vector3.Lerp (this.transform.position, positionTarget, Damping) + CameraEffects.CameraFX.PositionShaker;
		
	}

	void LateUpdate(){
		UpdateCamera ();
	}

	void Update ()
	{
		

		bool activecheck = false;
		for (int i = 0; i < Cameras.Length; i++) {
			if (Cameras [i] && Cameras [i].enabled) {
				activecheck = true;
				break;	
			}
		}
		if (!activecheck) {
			this.GetComponent<Camera> ().enabled = true;
			if (this.gameObject.GetComponent<AudioListener> ())
				this.gameObject.GetComponent<AudioListener> ().enabled = true;
		}

		if(Target == null){
			TankPlayer tankTarget = (TankPlayer)GameObject.FindObjectOfType(typeof(TankPlayer));
			if(tankTarget != null && tankTarget.tank != null){
				if(tankTarget.tank.MainGun){
					Target = tankTarget.tank.MainGun.gameObject;
				}else{
					if(tankTarget.tank.Turret){
						Target = tankTarget.tank.Turret.gameObject;
					}else{
						Target = tankTarget.tank.gameObject;
					}
				}
			}
		}
	}
}
