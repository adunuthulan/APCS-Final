using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//code for ending mario reference
public class EndTube : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        // If hit player, trigger end
        if (other.tag == "Player")
            StartCoroutine(End());
    }

	//ends the game
    private IEnumerator End()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody>().isKinematic = true;

        string menu = GameManager.instance.mainMenuLevel;

        foreach (DontDestroy d in DontDestroy.dontDestroyObjects)
        {
            if (d.gameObject.tag != "Player")
                Destroy(d.gameObject);
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(menu);

        Destroy(player);
    }

}
