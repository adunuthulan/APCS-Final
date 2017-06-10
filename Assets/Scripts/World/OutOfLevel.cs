using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfLevel : MonoBehaviour {

	private void OnTriggerEnter (Collider c) {

		if (c.gameObject.tag == "Player"){

			// Restart player pos
			StartCoroutine(c.gameObject.GetComponent<Player>().Restart());

			// Reset health
			c.gameObject.GetComponent<Player>().ResetStats();
		}
	}
}
