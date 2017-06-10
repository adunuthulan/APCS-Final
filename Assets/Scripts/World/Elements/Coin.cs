using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    AudioSource source;
    private bool used;

    void Start()
    {
        source = GetComponent<AudioSource>();
        used = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If hit the player, add 1 coin to player, play the sound and destroy
        if (other.gameObject.tag == "Player" && !used)
        {
            used = true;
            StartCoroutine(PlayTheSound());
            other.GetComponent<Player>().AddCoin(1);
        }
    }

	//plays coin song
    IEnumerator PlayTheSound()
    {
        source.PlayOneShot(source.clip);
        Destroy(this.GetComponent<MeshRenderer>());
        yield return new WaitForSeconds(source.clip.length - 0.05f);
        Destroy(this.gameObject);
    }
}
