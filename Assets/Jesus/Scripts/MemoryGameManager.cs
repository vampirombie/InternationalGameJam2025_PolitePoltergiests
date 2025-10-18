using UnityEngine;
using TMPro;

public class MemoryGameManager : MonoBehaviour
{
    public CardsController cardsController;
    public GameObject winScreen;
    public GameObject loseScreen;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI triesText;

    public float gameTime = 30f;
    public int maxMistakes = 5;

    private float currentTime;
    private int pairsFound = 0;
    private int totalPairs;
    private int mistakes = 0;
    private bool isGameOver = false;

    void Start()
    {
        currentTime = gameTime;
        cardsController = FindFirstObjectByType<CardsController>();

        if (triesText != null)
            triesText.text = $"Mistakes: {mistakes}/{maxMistakes}";
    }

    void Update()
    {
        if (isGameOver) return;

        currentTime -= Time.deltaTime;
        timerText.text = $"Time: {currentTime:F1}";

        if (totalPairs == 0 && cardsController.cards.Count > 0)
        {
            totalPairs = cardsController.cards.Count / 2;
        }

        if (currentTime <= 0f)
        {
            LoseGame();
        }
    }

    public void OnPairMatched()
    {
        pairsFound++;
        Debug.Log($"Pares encontrados: {pairsFound}/{totalPairs}");

        if (pairsFound >= totalPairs)
        {
            WinGame();
        }
    }

    public void OnPairMistake()
    {
        mistakes++;

        if (triesText != null)
            triesText.text = $"Mistakes: {mistakes}/{maxMistakes}";

        Debug.Log($"Error {mistakes}/{maxMistakes}");

        if (mistakes >= maxMistakes)
        {
            LoseGame();
        }
    }

    void WinGame()
    {
        isGameOver = true;
        winScreen.SetActive(true);
    }

    void LoseGame()
    {
        isGameOver = true;
        loseScreen.SetActive(true);
    }
}
