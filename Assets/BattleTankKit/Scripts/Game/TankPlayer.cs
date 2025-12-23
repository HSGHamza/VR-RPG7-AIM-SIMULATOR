using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Tank))]
public class TankPlayer : MonoBehaviour {

	public string Name;
	public bool IsMine;
	[HideInInspector]
	public Tank tank;

	void Awake(){
		tank = this.GetComponent<Tank>();
	}

	void Start () {
	
	}

}
