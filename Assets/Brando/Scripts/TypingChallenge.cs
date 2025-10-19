using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TypingChallenge : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI startCountdownText;
    public Button restartButton;
    public Button quitButton;

    [Header("Game Settings")]
    [TextArea]
    public string correctSentence = "El zorro rapido salta sobre el perro perezoso.";
    public float timeLimit = 15f;
    public int maxAttempts = 2;
    public float startDelay = 3f;

    [Header("Visual Settings")]
    public Gradient timeColorGradient;
    public float blinkSpeed = 2f;

    private float timer;
    private int attemptsLeft;
    private string currentInput = "";
    private bool isPlaying = false;
    private bool blinking = false;
    private bool timerStarted = false;
    private bool gameStarted = false;

    private bool isRewinding = false;
    public float rewindSpeed = 0.05f;

    void Start()
    {
        if (timeColorGradient == null)
        {
            GradientColorKey[] colorKey = new GradientColorKey[3];
            colorKey[0].color = Color.green;
            colorKey[0].time = 1.0f;
            colorKey[1].color = Color.yellow;
            colorKey[1].time = 0.5f;
            colorKey[2].color = Color.red;
            colorKey[2].time = 0.0f;

            GradientAlphaKey[] alphaKey = new GradientAlphaKey[1];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;

            timeColorGradient = new Gradient();
            timeColorGradient.SetKeys(colorKey, alphaKey);
        }

        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(RestartGame);

        StartGame();
    }

    void Update()
    {
        if (!isPlaying || isRewinding) return;

        if (!timerStarted && Input.anyKeyDown)
        {
            timerStarted = true;
            gameStarted = true;
        }

        if (!timerStarted) return;

        timer -= Time.deltaTime;
        timerText.text = $"Tiempo: {timer:F1}s";

        float t = Mathf.Clamp01(timer / timeLimit);
        Color timeColor = timeColorGradient.Evaluate(t);
        targetText.color = timeColor;

        if (t < 0.2f)
        {
            blinking = true;
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            targetText.alpha = Mathf.Lerp(0.3f, 1f, alpha);
        }
        else
        {
            blinking = false;
            targetText.alpha = 1f;
        }

        if (timer <= 0)
        {
            GameOver();
            return;
        }

        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else if (c == '\n' || c == '\r') { }
            else
            {
                currentInput += c;
                if (!correctSentence.StartsWith(currentInput))
                {
                    FailAttempt();
                    return;
                }
            }
        }

        playerText.text = HighlightCorrectLetters(currentInput, correctSentence);

        if (currentInput == correctSentence)
        {
            Win();
        }
    }

    string HighlightCorrectLetters(string input, string target)
    {
        string result = "";
        int length = Mathf.Min(input.Length, target.Length);
        for (int i = 0; i < length; i++)
        {
            if (input[i] == target[i])
                result += $"<color=#00FF00>{target[i]}</color>";
            else
                result += $"<color=#FF0000>{target[i]}</color>";
        }
        if (input.Length < target.Length)
            result += $"<color=#808080>{target.Substring(input.Length)}</color>";
        return result;
    }

    string HighlightIncorrectRewind(string input, string target)
    {
        string result = "";
        int length = Mathf.Min(input.Length, target.Length);
        for (int i = 0; i < length; i++)
        {
            result += $"<color=#FF0000>{target[i]}</color>";
        }
        if (input.Length < target.Length)
            result += $"<color=#808080>{target.Substring(input.Length)}</color>";
        return result;
    }

    void FailAttempt()
    {
        isRewinding = true;
        StartCoroutine(RewindInput());
    }

    IEnumerator RewindInput()
    {
        while (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            playerText.text = HighlightIncorrectRewind(currentInput, correctSentence);
            yield return new WaitForSeconds(rewindSpeed);
        }

        attemptsLeft--;
        attemptsText.text = $"Intentos: {attemptsLeft}";

        if (attemptsLeft <= 0)
        {
            GameOver();
        }
        else
        {
            playerText.text = "";
            isRewinding = false;
        }
    }

    void Win()
    {
        isPlaying = false;
        timerText.text = "¡Correcto!";
        targetText.color = Color.green;
        targetText.alpha = 1f;
        quitButton.gameObject.SetActive(true);
    }

    void GameOver()
    {
        isPlaying = false;
        timerText.text = "Fin del juego";
        targetText.color = Color.red;
        targetText.alpha = 1f;
        restartButton.gameObject.SetActive(true);
    }

    IEnumerator StartCountdown()
    {
        isPlaying = false;
        timerStarted = false;
        currentInput = "";
        playerText.text = "";
        targetText.text = correctSentence;
        targetText.alpha = 1f;
        targetText.color = Color.white;
        timer = timeLimit;
        attemptsText.text = $"Intentos: {attemptsLeft}";
        startCountdownText.gameObject.SetActive(true);

        for (int i = (int)startDelay; i > 0; i--)
        {
            startCountdownText.text = $"Comenzando en {i}...";
            yield return new WaitForSeconds(1f);
        }

        startCountdownText.text = "¡Ya!";
        yield return new WaitForSeconds(0.5f);
        startCountdownText.gameObject.SetActive(false);
        isPlaying = true;
        timerStarted = false;
        timerText.text = $"Tiempo: {timer:F1}s";
    }

    public void RestartGame()
    {
        StopAllCoroutines(); 
        StartGame();
    }

    public void StartGame()
    {
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        attemptsLeft = maxAttempts;

        isRewinding = false;
        currentInput = "";
        playerText.text = "";

        StartCoroutine(StartCountdown());
    }
}
