using UnityEngine;
using TMPro;

public class MemoryGameManager : MonoBehaviour
{
    public CardsController cardsController;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject exitButton; //  botón de salir solo aparece en el nivel 3
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI triesText;

    public float gameTime = 30f;
    public int maxMistakes = 5;

    private float currentTime;
    private int pairsFound = 0;
    private int totalPairs;
    private int mistakes = 0;
    private bool isGameOver = false;
    private bool isPlaying = false;

    private int currentLevel = 1; //  nivel inicial

    private void Start()
    {
        if (cardsController == null)
            cardsController = FindFirstObjectByType<CardsController>();

        StartGame(1);
    }

    public void StartGame(int level)
    {
        // Reiniciar variables
        currentTime = gameTime;
        pairsFound = 0;
        mistakes = 0;
        totalPairs = 0;
        isGameOver = false;
        isPlaying = true;

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        if (exitButton != null) exitButton.SetActive(false);

        if (triesText != null)
            triesText.text = $"Mistakes: {mistakes}/{maxMistakes}";


        cardsController.StartGame(level);
    }

    private void Update()
    {
        if (!isPlaying || isGameOver) return;

        currentTime -= Time.deltaTime;
        if (timerText != null)
            timerText.text = $"Time: {currentTime:F1}";

        if (totalPairs == 0 && cardsController.cards.Count > 0)
            totalPairs = cardsController.cards.Count / 2;

        if (currentTime <= 0f)
            LoseGame();
    }

    public void OnPairMatched()
    {
        pairsFound++;

        if (pairsFound >= totalPairs)
            WinGame();
    }

    public void OnPairMistake()
    {
        mistakes++;

        if (triesText != null)
            triesText.text = $"Mistakes: {mistakes}/{maxMistakes}";

        if (mistakes >= maxMistakes)
            LoseGame();
    }

    void WinGame()
    {
        isGameOver = true;
        isPlaying = false;
        winScreen.SetActive(true);

        // Esperar un momento antes de avanzar de nivel
        if (currentLevel < 3)
            Invoke(nameof(NextLevel), 2f);
        else
            ShowExitButton(); //  si ya es nivel 3, muestra botón salir
    }

    void LoseGame()
    {
        isGameOver = true;
        isPlaying = false;
        loseScreen.SetActive(true);
    }

    void NextLevel()
    {
        currentLevel++;
        StartGame(currentLevel);
    }

    void ShowExitButton()
    {
        if (exitButton != null)
            exitButton.SetActive(true);
    }
}
