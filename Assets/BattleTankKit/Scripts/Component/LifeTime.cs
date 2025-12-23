using UnityEngine;
using System.Collections;

public class LifeTime : MonoBehaviour {

	public float Lifetime = 3;
	void Start () {
		GameObject.Destroy(this.gameObject,Lifetime);
	}

}
