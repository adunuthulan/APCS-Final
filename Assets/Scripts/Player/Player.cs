using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public int health;
    public int maxHealth;

    // TODO: Remove coins
    public int levelCoins;
    public int currentCoins;

	public AudioSource source;
	public AudioClip dmgSound;
	public AudioClip healthSound;


    // When Game Starts count it as a Restart
    private void Start()
    {
        StartCoroutine(Restart());
    }

    private void Update()
    {
        // When Pressing Restart key (R) Invoke Restart
        if (CustomInputManager.CustomInput.OnKey("Restart"))
        {
            // Restart the position
            StartCoroutine(Restart());

            // Reset health
            ResetStats();
        }
    }

    public IEnumerator Restart()
    {
        // Return to SpawnPoint
        GoToSpawn();

        yield return new WaitForSeconds(0.1f);

        GetComponent<PlayerController>().boostFactor = 1;

        // Change Dimension to 2D
        DimensionManager.instance.SetDimension(DimensionManager.Dimension.TwoD);

        ResetItemBlocks();

        SetLevelCoin();

        PlayerUI.instance.SetGameOverScreen(false);
    }

    // Resets every item block back to its original state
    public void ResetItemBlocks()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("ItemBlock"))
            go.GetComponent<Block>().used = false;
    }

    // Resets level-based Stats
    public void ResetStats()
    {
        health = maxHealth;
    }

    // Resets all stats (even the ones that stay between levels)
    public void SetDefaults()
    {
        health = maxHealth;
        currentCoins = 0;
        levelCoins = 0;
    }

    // Find the nearest spawn and teleport to it
    public void GoToSpawn()
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("Spawn");

        GetComponent<PlayerController>().StopMovements();

        transform.position = spawn.transform.position;
        GetComponent<PlayerDim>().posX = spawn.transform.position.x;

        GetComponent<PlayerController>().StopMovements();
    }

    // Runs when the player dies
    private IEnumerator Die()
    {
        // Just a test because sometimes it's strange
        if (health > 0f) yield break;

        // Game Over Screen
        PlayerUI.instance.SetGameOverScreen(true);

        // Reset health
        health = maxHealth;
        currentCoins = levelCoins;

        yield return new WaitForSeconds(1f);

        // Reload the current level
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

        // Make sure health has been regen-ed (player can lose health when spawning in new scene and hasn't be spawned to a safe zone)
        health = maxHealth;

        // Spawn the player at it's position and do everything
        StartCoroutine(Restart());

        // Make sure health is good after spawning
        health = maxHealth;
    }

    // Take Damage and check for death
    public void TakeDamage(int dmg)
    {
		source.PlayOneShot (dmgSound);
        health -= dmg;

        // Check if the player is dead
        if (health <= 0)
            StartCoroutine(Die());
    }

    // Add Health back
    public void AddHealth(int h)
    {
		source.PlayOneShot (healthSound);
        health += h;

        // Make sure that the health isn't over the max
        if (health > maxHealth)
            health = maxHealth;
    }

    // Add Coins value
    public void AddCoin(int coin)
    {
        currentCoins += coin;
    }

    //sets the currentCoin value to the levelCoin value 
    public void SetLevelCoin()
    {
        currentCoins = levelCoins;
    }

    // Get the current health without being able to edit it
    public int CurrentHealth()
    {
        return health;
    }



}
