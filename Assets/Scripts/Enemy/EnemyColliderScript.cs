using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliderScript : MonoBehaviour {

	public bool done;

	void onCollisionEnter(Collider c){
		//this.transform.parent.GetComponent<RobotEnemyScript> ().colliding = true;
		done = true;
	}
}
