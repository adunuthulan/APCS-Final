using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public static PlayerUI instance;

    public GameObject endLevel;

    public GameObject loadingScreen;

    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private Text Coins;

    [SerializeField]
    private Image CoinImage;

    [SerializeField]
    private GameObject healthFill;

    [SerializeField]
    private GameObject gameOver;

    [SerializeField]
    private Text Power;

    [SerializeField]
    private Image PowerImage;

    [SerializeField]
    private Text quitText;

    public float powerTime;

    private Player p;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("There are more than one PlayerUIs in the scene!");
        else
            instance = this;

        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        Coins.text = "Coins: " + p.currentCoins;
        Power.text = "Speed: ";
        powerTime = 0.0f;
    }

    private void Update()
    {
        if (p == null)
        {
            // Keep searching for player
            p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        else
        {
            // If player is not dead, show green bar and coins and powerup text
            if (p.CurrentHealth() > 0)
            {
                // Resummons GUI
                healthFill.SetActive(true);
                Coins.transform.localScale = new Vector3(1, 1, 1);
                CoinImage.transform.localScale = new Vector3(.5f, .5f, .5f);
                Power.transform.localScale = new Vector3(1, 1, 1);
                PowerImage.transform.localScale = new Vector3(.5f, .5f, .5f);

                // Set Max Value to Max Health
                healthBar.maxValue = p.maxHealth;

                // Set HealthBar percentage
                healthBar.value = p.CurrentHealth();

                // Updates coin value
                Coins.text = "Coins: " + p.currentCoins;

                // Updates Power
                Power.text = "Speed: " + Mathf.Round(powerTime);


            }
            // If player is dead (0 health), hide green bar and coins
            else
            {
                healthFill.SetActive(false);
                Coins.transform.localScale = new Vector3(0, 0, 0);
                CoinImage.transform.localScale = new Vector3(0, 0, 0);

                Power.transform.localScale = new Vector3(0, 0, 0);
                PowerImage.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    public void SetQuittingText(string newText)
    {
        if (quitText.text != newText)
            quitText.text = newText;
    }

    public void SetGameOverScreen(bool enabled)
    {
        if (gameOver.activeSelf != enabled)
            gameOver.SetActive(enabled);
    }
}