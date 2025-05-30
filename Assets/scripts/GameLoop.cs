using UnityEngine;
using TMPro;
using System.Collections;

public class GameLoop : MonoBehaviour
{
    public int player1RoundWins = 0;
    public int player2RoundWins = 0;
    public int player1SetWins = 0;
    public int player2SetWins = 0;

    public int roundsToWinSet = 2;
    public int setsToWinMatch = 10;

    public GameObject winScreenPanel;
    public TextMeshProUGUI winText;

    public PlayerController player1;
    public PlayerController player2;

    public PlayerStats player1Stats;
    public PlayerStats player2Stats;

    public HealthBar player1HealthBar;
    public HealthBar player2HealthBar;

    public UpgradeSelector upgradeSelector;
    private bool gameOver = false;
    private bool waitingForUpgrade = false;
    private string queuedLoser;
    private bool isInitialUpgradePhase = true;


    void Start()
    {
        ResetGame();

        if (upgradeSelector != null)
        {
            upgradeSelector.OnUpgradeSelected += OnUpgradeComplete;
            upgradeSelector.gameObject.SetActive(false);
        }

        StartCoroutine(StartGameUpgrades());
    }

    void Update()
    {
        if (gameOver || waitingForUpgrade) return;

        if (player1Stats.currentHealth <= 0)
        {
            player2RoundWins++;
            CheckRoundWinner("Red Wins the Round", "Red");
        }
        else if (player2Stats.currentHealth <= 0)
        {
            player1RoundWins++;
            CheckRoundWinner("Green Wins the Round", "Green");
        }
    }

    void ResetGame()
    {
        gameOver = false;

        player1RoundWins = 0;
        player2RoundWins = 0;
        player1SetWins = 0;
        player2SetWins = 0;

        isInitialUpgradePhase = true; // Reset upgrade phase

        player1Stats.ResetStats();
        player2Stats.ResetStats();

        player1HealthBar.SetMaxHealth(player1Stats.maxHealth);
        player1HealthBar.SetHealth(player1Stats.currentHealth);

        player2HealthBar.SetMaxHealth(player2Stats.maxHealth);
        player2HealthBar.SetHealth(player2Stats.currentHealth);

        winScreenPanel.SetActive(false);

        if (upgradeSelector != null)
            upgradeSelector.gameObject.SetActive(false);

        player1.canControl = false;
        player2.canControl = false;

        StartCoroutine(StartGameUpgrades());
    }

    IEnumerator StartGameUpgrades()
    {
        waitingForUpgrade = true;

        // Start with Green's upgrade
        queuedLoser = "Green";
        upgradeSelector.SetLoser(queuedLoser);
        upgradeSelector.gameObject.SetActive(true);
        Time.timeScale = 0f;

        yield return null;
    }

    IEnumerator StartSecondStartupUpgrade()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        // Start Red's upgrade
        queuedLoser = "Red";
        upgradeSelector.SetLoser(queuedLoser);
        upgradeSelector.gameObject.SetActive(true);
        Time.timeScale = 0f;

        yield return null;
    }

    void OnUpgradeComplete()
    {
        upgradeSelector.gameObject.SetActive(false);
        Time.timeScale = 1f;
        winScreenPanel.SetActive(false);

        if (isInitialUpgradePhase)
        {
            if (queuedLoser == "Green")
            {
                // Now Red gets their upgrade
                queuedLoser = "Red";
                StartCoroutine(StartSecondStartupUpgrade());
            }
            else
            {
                // Finished initial upgrades
                isInitialUpgradePhase = false;
                waitingForUpgrade = false;
                player1.canControl = true;
                player2.canControl = true;
                StartNextRound();
            }
        }
        else
        {
            // Regular upgrade after a set loss
            waitingForUpgrade = false;
            player1.canControl = true;
            player2.canControl = true;
            StartNextRound();
        }
    }


    void CheckRoundWinner(string roundMessage, string playerColor)
    {
        gameOver = true;

        winText.text = roundMessage;
        winScreenPanel.SetActive(true);

        if (player1RoundWins >= roundsToWinSet)
        {
            player1SetWins++;
            player1RoundWins = 0;
            player2RoundWins = 0;
            ShowSetWin("Green wins the set");
        }
        else if (player2RoundWins >= roundsToWinSet)
        {
            player2SetWins++;
            player1RoundWins = 0;
            player2RoundWins = 0;
            ShowSetWin("Red wins the set");
        }
        else
        {
            Invoke(nameof(StartNextRound), 3f);
        }
    }

    void ShowSetWin(string message)
    {
        winText.text = message;

        string lastSetWinner = null;
        string lastSetLoser = null;

        if (message.Contains("Green"))
        {
            player1SetWins++;
            lastSetWinner = "Green";
            lastSetLoser = "Red";
        }
        else if (message.Contains("Red"))
        {
            player2SetWins++;
            lastSetWinner = "Red";
            lastSetLoser = "Green";
        }

        if (player1SetWins >= setsToWinMatch)
        {
            winText.text = "Green Wins the Match";
            gameOver = true;                // Stop further input
            waitingForUpgrade = false;
            player1.canControl = false;
            player2.canControl = false;

            // Reset game after delay (3 seconds)
            Invoke(nameof(ResetGame), 3f);
        }
        else if (player2SetWins >= setsToWinMatch)
        {
            winText.text = "Red Wins the Match";
            gameOver = true;
            waitingForUpgrade = false;
            player1.canControl = false;
            player2.canControl = false;

            // Reset game after delay (3 seconds)
            Invoke(nameof(ResetGame), 3f);
        }
        else
        {
            if (upgradeSelector != null)
            {
                upgradeSelector.SetLoser(lastSetLoser);
                upgradeSelector.gameObject.SetActive(true);
                winScreenPanel.SetActive(true);
                Time.timeScale = 0f;

                player1.canControl = false;
                player2.canControl = false;

                waitingForUpgrade = true;
                queuedLoser = lastSetLoser;
            }
            else
            {
                Invoke(nameof(StartNextRound), 3f);
            }
        }
    }


    void StartNextRound()
    {
        gameOver = false;

        player1.canControl = true;
        player2.canControl = true;

        player1Stats.ResetHealth();
        player2Stats.ResetHealth();

        player1HealthBar.SetHealth(player1Stats.currentHealth);
        player2HealthBar.SetHealth(player2Stats.currentHealth);

        player1.ResetPlayerPosition();
        player2.ResetPlayerPosition();

        // Destroy all bullets
        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in allBullets)
        {
            Destroy(bullet);
        }

        winScreenPanel.SetActive(false);
    }
}
