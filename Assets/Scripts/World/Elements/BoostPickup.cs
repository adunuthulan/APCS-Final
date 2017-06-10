using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPickup : MonoBehaviour {

    public float pickupTime = 40.0f;
    public float speedBoostFactor = 1.5f;

    public void OnTriggerEnter(Collider collision)
    {
        // If hit the player, give it a speed boost and destroy
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerController>().EnableSpeedBoost(pickupTime, speedBoostFactor);
            Destroy(this.gameObject);
        }
    }


}
