using UnityEngine;
using System.Collections;

public class NavigatorInstance : MonoBehaviour {

	public GameObject Owner;
	public UnityEngine.AI.NavMeshAgent Navigator;

	void Start () {
		Navigator = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	public void SetDestination(Vector3 target){
		if(Navigator){
			Navigator.SetDestination(target);
		}
	}

	public Vector3 GetDirection(){
		if(Navigator){
			return (Navigator.velocity - Owner.transform.position).normalized;
		}
		return Vector3.zero;
	}


	void Update () {
		if(Owner == null){
			GameObject.Destroy(this.gameObject);
		}
	}
}
