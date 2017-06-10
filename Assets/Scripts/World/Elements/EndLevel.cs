using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        // If hit the player, start ending the level
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(EndTheLevel());
        }
    }

    private IEnumerator EndTheLevel()
    {
        // Enable "Level Complete" text
        PlayerUI.instance.endLevel.SetActive(true);

        // Prevent the player from moving
        Rigidbody player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        player.isKinematic = true;

        yield return new WaitForSeconds(2f);

        // Disable "Level Complete" text
        PlayerUI.instance.endLevel.SetActive(false);

        // Switch to next level
        GameManager.instance.NextLevel();

    }

}
