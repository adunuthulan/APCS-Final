using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//starts the scene and sets it up
public class Setup : MonoBehaviour {

	GameObject player;

	public string sceneName;

	public string firstLevel;


	void Start () {

		if (SceneManager.GetActiveScene().name == sceneName){
			player = GameObject.Find("Player");
			PlayerUI.instance.loadingScreen.SetActive(true);
			StartCoroutine(Load());
		}
	}

	void Update () {
		if (SceneManager.GetActiveScene().name == sceneName){
			if (player != null)
				player.GetComponent<PlayerController>().StopMovements();
		}
	}

	IEnumerator Load () {


		SceneManager.LoadScene(firstLevel);

		yield return new WaitForSeconds(0.25f);

		StartCoroutine(player.GetComponent<Player>().Restart());
		player.GetComponent<Player>().SetDefaults();

		PlayerUI.instance.loadingScreen.SetActive(false);
	}

}
