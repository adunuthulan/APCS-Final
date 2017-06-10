using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    public int gain = 10;

    public void OnTriggerEnter(Collider collision)
    {
        // If it hits the player, then tell it to add health and destroy
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().AddHealth(gain);
            Destroy(this.gameObject);
        }
    }
}
