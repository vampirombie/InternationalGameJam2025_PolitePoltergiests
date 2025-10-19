using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelConfig
{
    public int numberOfCups = 3;
    public int mixCount = 5;
    public float moveSpeed = 500f;
}

public class CandyCupsUI : MonoBehaviour
{
    [Header("Configuración de Niveles")]
    public LevelConfig[] levels;

    [Header("Referencias de Prefabs y Contenedores")]
    public GameObject cupPrefab;
    public Transform cupsContainer;
    public RectTransform candy;

    [Header("Referencias UI")]
    public Button startButton;
    public Button nextLevelButton;
    public Button retryButton;
    public Button restartGameButton;
    public Button quitButton;
    public TMP_Text resultText;

    private List<RectTransform> activeCups = new List<RectTransform>();
    private int correctCupIndex = -1; // Usaremos esto para saber dónde se escondió inicialmente
    private bool isMixing = false;
    private bool levelEnded = false;

    private int currentLevel = 0;

    private Vector2 candyInitialPosition;
    private Transform candyInitialParent;

    void Start()
    {
        // Guardamos la posición y el padre original del caramelo una sola vez.
        candyInitialParent = candy.parent;
        candyInitialPosition = candy.anchoredPosition;

        // Asignamos una acción permanente al botón de salir (opcional).
        // quitButton.onClick.AddListener(Application.Quit);

        // Iniciamos el juego desde el primer nivel.
        ConfigureLevel(0);
    }

    /// <summary>
    /// Método central para configurar y limpiar un nivel específico.
    /// Es la función más importante para reiniciar el estado del juego.
    /// </summary>
    /// <param name="levelIndex">El índice del nivel a cargar (0, 1, 2...).</param>
    void ConfigureLevel(int levelIndex)
    {
        currentLevel = Mathf.Clamp(levelIndex, 0, levels.Length - 1);
        Debug.Log($"Configurando nivel: {currentLevel}");

        // ===== SOLUCIÓN CLAVE #1: RESCATAR AL CARAMELO =====
        // Antes de destruir los vasos, devolvemos el caramelo a su estado original.
        // Esto evita que sea destruido junto con el vaso que lo contenía.
        if (candy != null)
        {
            candy.gameObject.SetActive(true);
            candy.SetParent(candyInitialParent);
            candy.anchoredPosition = candyInitialPosition;
        }

        // Reiniciamos los estados del juego para el nuevo nivel.
        isMixing = false;
        levelEnded = false;
        correctCupIndex = -1;

        // Limpiamos los vasos de la partida anterior.
        ClearPreviousCups();

        LevelConfig config = levels[currentLevel];
        float containerWidth = cupsContainer.GetComponent<RectTransform>().rect.width;
        float spacing = containerWidth / (config.numberOfCups + 1);

        // Creamos e instanciamos los nuevos vasos.
        for (int i = 0; i < config.numberOfCups; i++)
        {
            GameObject cupGO = Instantiate(cupPrefab, cupsContainer);
            RectTransform cupRect = cupGO.GetComponent<RectTransform>();
            cupRect.anchoredPosition = new Vector2((i + 1) * spacing - (containerWidth / 2), 0);
            activeCups.Add(cupRect);
        }

        // ===== SOLUCIÓN CLAVE #2: GESTIONAR LISTENERS AQUÍ =====
        // Limpiamos TODOS los listeners de los botones para evitar acciones incorrectas de rondas anteriores.
        startButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.RemoveAllListeners();
        retryButton.onClick.RemoveAllListeners();
        restartGameButton.onClick.RemoveAllListeners();

        // Asignamos las acciones correctas a cada botón para esta nueva ronda.
        startButton.onClick.AddListener(StartChoosingCup);
        nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        retryButton.onClick.AddListener(OnRetryClicked);
        restartGameButton.onClick.AddListener(OnRestartClicked);

        // Configuramos la visibilidad inicial de los botones.
        startButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        restartGameButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        resultText.text = $"Nivel {currentLevel + 1}/{levels.Length}. Presiona 'Empezar'.";

        DisableCupButtons();
    }

    void StartChoosingCup()
    {
        if (isMixing || levelEnded) return;

        startButton.gameObject.SetActive(false);
        resultText.text = "Elige un vaso para esconder el caramelo.";

        for (int i = 0; i < activeCups.Count; i++)
        {
            Button btn = activeCups[i].GetComponent<Button>();
            btn.interactable = true;
            btn.onClick.RemoveAllListeners();
            int index = i;
            btn.onClick.AddListener(() => OnCupClickedToHide(index));
        }
    }

    public void OnCupClickedToHide(int index)
    {
        if (isMixing) return;

        correctCupIndex = index;
        DisableCupButtons();

        resultText.text = "¡Caramelo escondido! Mezclando...";
        StartCoroutine(MoveCandyAndMix(index));
    }

    IEnumerator MoveCandyAndMix(int cupIndex)
    {
        // Animación para mover el caramelo hacia el vaso.
        Vector2 startPos = candy.position;
        Vector2 endPos = activeCups[cupIndex].position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 2.5f;
            candy.position = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        // Hacemos el caramelo hijo del vaso para que se mueva con él.
        candy.SetParent(activeCups[cupIndex]);
        candy.anchoredPosition = Vector2.zero; // Centrado dentro del vaso.
        candy.gameObject.SetActive(false); // Lo ocultamos visualmente.
        yield return new WaitForSeconds(0.5f);

        isMixing = true;
        LevelConfig config = levels[currentLevel];

        // Mezclamos los vasos.
        for (int i = 0; i < config.mixCount; i++)
        {
            int a = Random.Range(0, activeCups.Count);
            int b;
            do { b = Random.Range(0, activeCups.Count); } while (a == b);

            Vector2 posA = activeCups[a].anchoredPosition;
            Vector2 posB = activeCups[b].anchoredPosition;
            float duration = Vector2.Distance(posA, posB) / config.moveSpeed;

            float time = 0;
            while (time < 1)
            {
                time += Time.deltaTime / duration;
                activeCups[a].anchoredPosition = Vector2.Lerp(posA, posB, time);
                activeCups[b].anchoredPosition = Vector2.Lerp(posB, posA, time);
                yield return null;
            }

            activeCups[a].anchoredPosition = posB;
            activeCups[b].anchoredPosition = posA;

            // Importante: Actualizamos la lista para que coincida con la nueva posición visual.
            (activeCups[a], activeCups[b]) = (activeCups[b], activeCups[a]);
        }

        isMixing = false;
        resultText.text = "¿Dónde está el caramelo?";
        EnableCupSelectionToFind();
    }

    void EnableCupSelectionToFind()
    {
        for (int i = 0; i < activeCups.Count; i++)
        {
            Button btn = activeCups[i].GetComponent<Button>();
            btn.interactable = true;
            btn.onClick.RemoveAllListeners();
            int index = i;
            btn.onClick.AddListener(() => OnCupSelectedToFind(index));
        }
    }

    public void OnCupSelectedToFind(int selectedIndex)
    {
        if (isMixing || levelEnded) return;

        levelEnded = true;
        DisableCupButtons();

        // Iniciamos la corrutina para levantar el vaso y revelar el resultado.
        StartCoroutine(RevealCup(selectedIndex));
    }

    // === MEJORA: Corrutina para animar el levantamiento de los vasos ===
    IEnumerator RevealCup(int selectedIndex)
    {
        RectTransform selectedCup = activeCups[selectedIndex];
        Vector2 originalSelectedPos = selectedCup.anchoredPosition;
        Vector2 raisedSelectedPos = originalSelectedPos + new Vector2(0, 100f);

        // Buscamos cuál es el vaso que realmente tiene el caramelo.
        int finalCorrectIndex = -1;
        for (int i = 0; i < activeCups.Count; i++)
        {
            if (activeCups[i].childCount > 0) // El vaso con el caramelo tendrá un hijo.
            {
                finalCorrectIndex = i;
                break;
            }
        }

        // Hacemos visible el caramelo de nuevo, pero aún debajo del vaso.
        candy.gameObject.SetActive(true);

        // Levantamos el vaso que eligió el jugador.
        float time = 0;
        while (time < 0.25f)
        {
            selectedCup.anchoredPosition = Vector2.Lerp(originalSelectedPos, raisedSelectedPos, time / 0.25f);
            time += Time.deltaTime;
            yield return null;
        }
        selectedCup.anchoredPosition = raisedSelectedPos;

        yield return new WaitForSeconds(0.5f);

        // Si el jugador se equivocó, levantamos también el vaso correcto.
        if (selectedIndex != finalCorrectIndex)
        {
            RectTransform correctCup = activeCups[finalCorrectIndex];
            Vector2 originalCorrectPos = correctCup.anchoredPosition;
            Vector2 raisedCorrectPos = originalCorrectPos + new Vector2(0, 100f);

            time = 0;
            while (time < 0.25f)
            {
                correctCup.anchoredPosition = Vector2.Lerp(originalCorrectPos, raisedCorrectPos, time / 0.25f);
                time += Time.deltaTime;
                yield return null;
            }
            correctCup.anchoredPosition = raisedCorrectPos;
        }

        yield return new WaitForSeconds(1f);

        // Finalmente, mostramos el resultado.
        if (selectedIndex == finalCorrectIndex)
            HandleWin();
        else
            HandleLose();
    }


    void HandleWin()
    {
        resultText.text = $"¡Correcto! Nivel {currentLevel + 1} superado.";

        if (currentLevel >= levels.Length - 1)
        {
            resultText.text = "¡Felicidades! ¡Completaste todos los niveles!";
            restartGameButton.gameObject.SetActive(true);
        }
        else
        {
            nextLevelButton.gameObject.SetActive(true);
        }
        quitButton.gameObject.SetActive(true);
    }

    void HandleLose()
    {
        resultText.text = $"¡Incorrecto! Inténtalo de nuevo en el Nivel {currentLevel + 1}.";
        retryButton.gameObject.SetActive(true);
        restartGameButton.gameObject.SetActive(true); // Permitir reiniciar también al perder.
        quitButton.gameObject.SetActive(true);
    }

    void OnNextLevelClicked()
    {
        ConfigureLevel(currentLevel + 1);
    }

    void OnRetryClicked()
    {
        ConfigureLevel(currentLevel);
    }

    void OnRestartClicked()
    {
        ConfigureLevel(0);
    }

    void DisableCupButtons()
    {
        foreach (var cup in activeCups)
        {
            Button btn = cup.GetComponent<Button>();
            if (btn != null) btn.interactable = false;
        }
    }

    void ClearPreviousCups()
    {
        // Este bucle destruirá todos los vasos.
        // Por eso es CRÍTICO haber rescatado al caramelo antes.
        foreach (Transform child in cupsContainer)
        {
            Destroy(child.gameObject);
        }
        activeCups.Clear();
    }
}