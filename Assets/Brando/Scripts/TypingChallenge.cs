using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TypingChallenge : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI startCountdownText;
    public Button restartButton;
    public Button nextLevelButton;
    public Button quitButton;

    [Header("Game Settings")]
    public int maxAttempts = 2;
    public float startDelay = 3f;
    private int maxLevels = 3;

    [Header("Progressive Difficulty")]
    public float baseTimeLimit = 20f;
    public float timeReductionPerWin = 0.5f;
    public float minTimeLimit = 5f;
    private static int difficultyBonus = 0;

    [Header("Visual Settings")]
    public Gradient timeColorGradient;
    public float blinkSpeed = 2f;

    // --- Variables privadas de estado ---
    private string correctSentence;
    private float timeLimit;
    private float timer;
    private int attemptsLeft;
    private string currentInput = "";
    private bool isPlaying = false;
    private bool timerStarted = false;
    private bool isRewinding = false;
    public float rewindSpeed = 0.05f;

    private static int currentLevel = 1;

    void Start()
    {
        if (timeColorGradient == null || timeColorGradient.colorKeys.Length == 0)
        {
            GradientColorKey[] colorKey = new GradientColorKey[3];
            colorKey[0].color = Color.green; colorKey[0].time = 1.0f;
            colorKey[1].color = Color.yellow; colorKey[1].time = 0.5f;
            colorKey[2].color = Color.red; colorKey[2].time = 0.0f;
            GradientAlphaKey[] alphaKey = new GradientAlphaKey[1];
            alphaKey[0].alpha = 1.0f; alphaKey[0].time = 0.0f;
            timeColorGradient = new Gradient();
            timeColorGradient.SetKeys(colorKey, alphaKey);
        }

        restartButton.onClick.AddListener(RestartGame);
        nextLevelButton.onClick.AddListener(GoToNextLevel);

        if (targetText == null || playerText == null || timerText == null || attemptsText == null || startCountdownText == null || restartButton == null || nextLevelButton == null || quitButton == null)
        {
            Debug.LogError("¡ERROR! Una o más referencias de UI no están asignadas en el Inspector de TypingChallenge.");
            return;
        }

        StartGame();
    }

    void Update()
    {
        if (!isPlaying || isRewinding) return;

        if (!timerStarted && Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            timerStarted = true;
        }

        if (!timerStarted) return;

        HandleTimer();
        HandleInput();
    }

    private void HandleTimer()
    {
        timer -= Time.deltaTime;
        timerText.text = $"Tiempo: {timer:F1}s";
        float t = Mathf.Clamp01(timer / timeLimit);
        targetText.color = timeColorGradient.Evaluate(t);
        if (t < 0.2f) targetText.alpha = Mathf.Lerp(0.3f, 1f, Mathf.PingPong(Time.time * blinkSpeed, 1f));
        else targetText.alpha = 1f;
        if (timer <= 0) GameOver("¡Se acabó el tiempo!");
    }

    private void HandleInput()
    {
        if (Input.inputString.Length > 0)
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b') { if (currentInput.Length > 0) currentInput = currentInput.Substring(0, currentInput.Length - 1); }
                else if (c != '\n' && c != '\r')
                {
                    currentInput += c;
                    if (!correctSentence.StartsWith(currentInput)) { FailAttempt(); return; }
                }
            }
            playerText.text = HighlightCorrectLetters(currentInput, correctSentence);
            if (currentInput == correctSentence) Win();
        }
    }

    string HighlightCorrectLetters(string i, string t) { return $"<color=#00FF00>{i}</color>" + (i.Length < t.Length ? $"<color=#808080>{t.Substring(i.Length)}</color>" : ""); }

    void FailAttempt() { isRewinding = true; StartCoroutine(RewindInput()); }

    IEnumerator RewindInput()
    {
        while (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            playerText.text = $"<color=#FF0000>{currentInput}</color><color=#808080>{correctSentence.Substring(currentInput.Length)}</color>";
            yield return new WaitForSeconds(rewindSpeed);
        }
        attemptsLeft--;
        attemptsText.text = $"Intentos: {attemptsLeft}";
        isRewinding = false;
        if (attemptsLeft <= 0) GameOver("¡Te quedaste sin intentos!");
        else playerText.text = HighlightCorrectLetters("", correctSentence);
    }

    IEnumerator StartCountdown()
    {
        isPlaying = false; timerStarted = false; isRewinding = false; currentInput = ""; timer = timeLimit;
        targetText.text = correctSentence; targetText.color = Color.white; targetText.alpha = 1f;
        playerText.text = HighlightCorrectLetters("", correctSentence);
        attemptsText.text = $"Intentos: {attemptsLeft}";
        timerText.text = $"Tiempo: {timer:F1}s";
        startCountdownText.gameObject.SetActive(true);
        for (int i = (int)startDelay; i > 0; i--) { startCountdownText.text = $"Comenzando en {i}..."; yield return new WaitForSeconds(1f); }
        startCountdownText.text = "¡Ya!"; yield return new WaitForSeconds(0.5f);
        startCountdownText.gameObject.SetActive(false);
        isPlaying = true;
    }

    void Win()
    {
        isPlaying = false;
        targetText.color = Color.green;
        targetText.alpha = 1f;

        difficultyBonus++;

        if (currentLevel < maxLevels)
        {
            // Ganas un nivel intermedio (Nivel 1 o 2)
            timerText.text = "¡Correcto!";
            restartButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }
        else
        {
            // Ganas el último nivel (Nivel 3)
            timerText.text = "¡Felicidades! ¡Has completado el juego!";
            restartButton.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(true);
        }
        GameManager.Instance.AddCandies(2);
    }

    void GameOver(string reason)
    {
        isPlaying = false;
        timerText.text = reason;
        targetText.color = Color.red;
        targetText.alpha = 1f;

        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(false);
        //reduce candies
        GameManager.Instance.ReduceCandies(2);
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        StartGame();
    }

    public void GoToNextLevel()
    {
        currentLevel++;
        StartGame();
    }

    

    public void StartGame()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("¡No se puede iniciar el juego! No se encuentra una instancia de GameManager en la escena.");
            return;
        }

        correctSentence = GameManager.Instance.GetSentenceByDifficulty(currentLevel);
        if (string.IsNullOrEmpty(correctSentence) || correctSentence.StartsWith("No hay frases"))
        {
            Debug.LogError($"¡No se pudo obtener una frase para el nivel {currentLevel}! Revisa la configuración de TextSentences en el GameManager.");
            correctSentence = "Error: no se encontró una frase.";
        }

        timeLimit = Mathf.Max(baseTimeLimit - (difficultyBonus * timeReductionPerWin), minTimeLimit);

        restartButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        attemptsLeft = maxAttempts;
        StopAllCoroutines();
        StartCoroutine(StartCountdown());
    }
}