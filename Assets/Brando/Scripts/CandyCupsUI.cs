using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CandyCupsUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform[] cups;
    public RectTransform candy;
    public Button startButton;
    public Button restartButton; // Botón para "Empezar otra vez"
    public Button quitButton;    // Botón para "Salir del juego"
    public TMP_Text resultText;

    [Header("Configuración")]
    public float moveSpeed = 500f;
    public int mixCount = 5;

    private int correctCup = -1;
    private bool isMixing = false;
    private bool isPlayerChoosing = false; // Renombrado para más claridad

    private Vector2 candyStartPos;
    private Vector2[] cupsStartPos; // Para guardar las posiciones iniciales de los vasos

    void Start()
    {
        // Guardamos las posiciones iniciales
        candyStartPos = candy.anchoredPosition;
        cupsStartPos = new Vector2[cups.Length];
        for (int i = 0; i < cups.Length; i++)
        {
            cupsStartPos[i] = cups[i].anchoredPosition;
        }

        startButton.onClick.AddListener(StartChoosingCup);
        restartButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(StartGame);
        StartGame();
    }

    // Función para iniciar y reiniciar el juego
    void StartGame()
    {
        // Ocultamos los botones de fin de juego
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);

        resultText.text = "Haz clic en 'Empezar' y luego en un vaso para esconder el caramelo.";

        // Reseteamos el estado
        isMixing = false;
        isPlayerChoosing = false;
        correctCup = -1;

        // Devolvemos el caramelo a su sitio
        candy.SetParent(transform); // Aseguramos que el caramelo no sea hijo de un vaso
        candy.anchoredPosition = candyStartPos;

        // Devolvemos los vasos a su sitio
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].anchoredPosition = cupsStartPos[i];
        }

        DisableCupButtons();
    }


    void StartChoosingCup()
    {
        if (isMixing) return;

        startButton.gameObject.SetActive(false);
        resultText.text = "Elige un vaso para esconder el caramelo.";
        isPlayerChoosing = true;

        for (int i = 0; i < cups.Length; i++)
        {
            Button btn = cups[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = true;
                btn.onClick.RemoveAllListeners();
                int index = i;
                btn.onClick.AddListener(() => OnCupClickedToHide(index));
            }
        }
    }

    public void OnCupClickedToHide(int index)
    {
        if (!isPlayerChoosing || isMixing) return;

        correctCup = index;
        isPlayerChoosing = false;

        DisableCupButtons();

        resultText.text = "¡Caramelo escondido! Mezclando...";

        StartCoroutine(MoveCandyToCup(index));
    }


    IEnumerator MoveCandyToCup(int index)
    {
        Vector2 start = candy.anchoredPosition;
        Vector2 end = cups[index].anchoredPosition;

        float duration = 0.5f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            candy.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        // Fijamos el caramelo dentro del vaso
        candy.SetParent(cups[index]);
        candy.anchoredPosition = Vector2.zero;

        yield return new WaitForSeconds(0.3f);

        StartCoroutine(MixCups());
    }

    IEnumerator MixCups()
    {
        isMixing = true;

        for (int i = 0; i < mixCount; i++)
        {
            int a = Random.Range(0, cups.Length);
            int b;
            do
            {
                b = Random.Range(0, cups.Length);
            } while (a == b);

            Vector2 posA = cups[a].anchoredPosition;
            Vector2 posB = cups[b].anchoredPosition;

            float distance = Vector2.Distance(posA, posB);
            float duration = distance / moveSpeed;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                cups[a].anchoredPosition = Vector2.Lerp(posA, posB, t);
                cups[b].anchoredPosition = Vector2.Lerp(posB, posA, t);
                yield return null;
            }
            // Aseguramos las posiciones finales para evitar imprecisiones
            cups[a].anchoredPosition = posB;
            cups[b].anchoredPosition = posA;
        }

        isMixing = false;
        resultText.text = "¿Dónde está el caramelo?";
        EnableCupSelectionToFind();
    }

    void EnableCupSelectionToFind()
    {
        for (int i = 0; i < cups.Length; i++)
        {
            Button btn = cups[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = true;
                btn.onClick.RemoveAllListeners();
                int index = i;
                // Le pasamos el RectTransform del vaso clickado
                btn.onClick.AddListener(() => OnCupSelectedToFind(cups[index]));
            }
        }
    }

    public void OnCupSelectedToFind(RectTransform selectedCup)
    {
        if (isMixing || isPlayerChoosing) return;

        DisableCupButtons();

        if (selectedCup == cups[correctCup])
        {
            resultText.text = "¡Adivinaste! Ganaste 5× caramelos.";
        }
        else
        {
            resultText.text = "Fallaste. El caramelo estaba en otro vaso.";
        }

        candy.SetParent(cups[correctCup]);
        candy.anchoredPosition = Vector2.zero;

        StartCoroutine(ShowEndGameButtonsAfterDelay(1.5f));
    }

    IEnumerator ShowEndGameButtonsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }


    void DisableCupButtons()
    {
        for (int i = 0; i < cups.Length; i++)
        {
            Button btn = cups[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = false;
            }
        }
    }
    
}