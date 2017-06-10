using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour {

	public int dmg = 1;

	public void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			collision.GetComponent<Player>().TakeDamage(dmg);
		}
	}

}
