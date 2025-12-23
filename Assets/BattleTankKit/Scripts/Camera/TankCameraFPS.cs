using UnityEngine;
using System.Collections;

public class TankCameraFPS : MonoBehaviour {

	Vector3 positionTemp;
	void Start () {
		positionTemp = this.transform.localPosition;
	}

	void Update () {
		if(CameraEffects.CameraFX!=null)
			this.transform.localPosition = positionTemp + CameraEffects.CameraFX.PositionShaker;
	}
}
