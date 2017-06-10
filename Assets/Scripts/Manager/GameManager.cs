using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomInputManager;

public class GameManager : MonoBehaviour {

    public string[] levelNames;
    public int currentLevel;

    public static GameManager instance;

    public string mainMenuLevel;
    public float timeToExit;
    private float timeLongPress;

    private void Awake()
    {
        // Check for duplicates to use instances
        if (instance != null)
            Debug.LogError("There are more than one GameManagers in the scene");
        else
            instance = this;
    }

    private void Update()
    {
        // If player is pressing pause
        if (CustomInput.OnKey("Pause"))
        {
            // Add to timer
            timeLongPress += Time.deltaTime;

            // Do quitting UI with . based on progress
            if (timeLongPress / timeToExit >= 2f / 3f)
                PlayerUI.instance.SetQuittingText("Quitting...");
            else if (timeLongPress / timeToExit >= 1f / 3f)
                PlayerUI.instance.SetQuittingText("Quitting..");
            else
                PlayerUI.instance.SetQuittingText("Quitting.");


            // If timer has reached max
            if (timeLongPress >= timeToExit)
            {
                // Reset quitting text and exit app
                PlayerUI.instance.SetQuittingText("");
                Application.Quit();
                timeLongPress = 0f;
            }
        }
        else
        {
            // Reset timer
            timeLongPress = 0f;
            PlayerUI.instance.SetQuittingText("");
        }

    }

    // Switch to next level
    public void NextLevel()
    {
        currentLevel += 1;

        SceneManager.LoadScene(levelNames[currentLevel]);

        Debug.Log("Loaded level " + levelNames[currentLevel]);

        PlayerUI.instance.loadingScreen.SetActive(true);

        //sets the player's currentCoin value to their levelCoin
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        p.GetComponent<Player>().levelCoins = p.GetComponent<Player>().currentCoins;

        StartCoroutine(ResetPlayer());
    }

    private IEnumerator ResetPlayer()
    {

        yield return new WaitForSeconds(1.5f);

        // Reset player stats and go to spawn
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(p.Restart());
        p.GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(0.1f);

        PlayerUI.instance.loadingScreen.SetActive(false);
    }

}
