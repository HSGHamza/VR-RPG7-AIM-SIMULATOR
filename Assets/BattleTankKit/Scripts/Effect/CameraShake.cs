using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

	public Vector3 Power = Vector3.up;

	void OnEnable(){
		CameraEffects.Shake (Power,this.transform.position);
	}

	void Start ()
	{
			
		
	}
}
