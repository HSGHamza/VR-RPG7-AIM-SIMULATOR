using UnityEngine;
using System.Collections;


public class TankCameraEffect : MonoBehaviour
{

	public float MaxDistance = 100;
	public Vector3 PositionShaker;

	void Start ()
	{

		CameraEffects.CameraFX = this;
	}

	Vector3 forcePower;

	public void Shake (Vector3 power, Vector3 position)
	{

		float distance = Vector3.Distance (this.transform.position, position);
		float damping = (1.0f / MaxDistance) * Mathf.Clamp (MaxDistance - distance, 0, MaxDistance);
		forcePower = -power * damping;
	}

	void Update ()
	{
		forcePower = Vector3.Lerp (forcePower, Vector3.zero, Time.deltaTime * 5);	
		PositionShaker = new Vector3 (Mathf.Cos (Time.time * 80) * forcePower.x, Mathf.Cos (Time.time * 80) * forcePower.y, Mathf.Cos (Time.time * 80) * forcePower.z);
	}
}

public static class CameraEffects
{
	public static TankCameraEffect CameraFX;

	public static void Shake (Vector3 power, Vector3 position)
	{
		if (CameraFX != null)
			CameraFX.Shake (power, position);
	}
}

