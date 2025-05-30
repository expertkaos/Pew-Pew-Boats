using UnityEngine;
using TMPro;
using System;

public class UpgradeSelector : MonoBehaviour
{
    [Header("UI References")]
    public GameObject[] upgradeCards;
    public GameObject redPointerPanel;
    public GameObject greenPointerPanel;
    public TextMeshProUGUI infoText;

    [Header("Player Stats")]
    public PlayerStats redPlayerStats;
    public PlayerStats greenPlayerStats;

    private int selectedCardIndex = 1;
    private string currentLoser = "Red";

    private float[] pointerAngles = { 45f, 0f, -45f };
    private GameObject activePointer;

    public event Action OnUpgradeSelected;

    void Start()
    {
        gameObject.SetActive(false); // Hide panel on start
    }

    void OnEnable()
    {
        Time.timeScale = 0f;
        selectedCardIndex = 1;
        UpdatePointer();
        UpdateCardHighlight();
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (currentLoser == "Red")
        {
            // Red player uses Arrow keys and Return
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selectedCardIndex = Mathf.Max(0, selectedCardIndex - 1);
                UpdatePointer();
                UpdateCardHighlight();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                selectedCardIndex = Mathf.Min(2, selectedCardIndex + 1);
                UpdatePointer();
                UpdateCardHighlight();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ApplyUpgrade();
            }
        }
        else if (currentLoser == "Green")
        {
            // Green player uses A/D and G
            if (Input.GetKeyDown(KeyCode.A))
            {
                selectedCardIndex = Mathf.Max(0, selectedCardIndex - 1);
                UpdatePointer();
                UpdateCardHighlight();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                selectedCardIndex = Mathf.Min(2, selectedCardIndex + 1);
                UpdatePointer();
                UpdateCardHighlight();
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                ApplyUpgrade();
            }
        }
    }

    public void SetLoser(string loser)
    {
        currentLoser = loser;

        if (infoText != null)
            infoText.text = $"{loser} Player: Choose an upgrade!";

        // Show cards and pointer again
        foreach (GameObject card in upgradeCards)
        {
            card.SetActive(true);
        }

        if (loser == "Red")
        {
            redPointerPanel.SetActive(true);
            greenPointerPanel.SetActive(false);
            activePointer = redPointerPanel;
        }
        else
        {
            greenPointerPanel.SetActive(true);
            redPointerPanel.SetActive(false);
            activePointer = greenPointerPanel;
        }

        selectedCardIndex = 1;
        UpdatePointer();
        UpdateCardHighlight();
    }


    void UpdatePointer()
    {
        if (activePointer != null)
            activePointer.transform.localEulerAngles = new Vector3(0, 0, pointerAngles[selectedCardIndex]);
    }

    void UpdateCardHighlight()
    {
        for (int i = 0; i < upgradeCards.Length; i++)
        {
            CanvasGroup group = upgradeCards[i].GetComponent<CanvasGroup>();
            if (group != null)
                group.alpha = (i == selectedCardIndex) ? 1f : 0.5f;
        }
    }

    void ApplyUpgrade()
    {
        PlayerStats stats = (currentLoser == "Red") ? redPlayerStats : greenPlayerStats;

        switch (selectedCardIndex)
        {
            case 0:
                stats.IncreaseMaxHealth(10);
                break;
            case 1:
                stats.IncreaseSpeed(1f);
                break;
            case 2:
                stats.IncreaseDamage(5);
                break;
        }

        stats.ResetHealth();

        for (int i = 0; i < upgradeCards.Length; i++)
        {
            upgradeCards[i].SetActive(false);
        }

        if (activePointer != null)
            activePointer.SetActive(false);

        OnUpgradeSelected?.Invoke(); // Notify GameLoop
    }


}
