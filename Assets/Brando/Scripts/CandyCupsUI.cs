using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CandyCupsUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public RectTransform[] cups;
    public RectTransform candy;
    public Button startButton;
    public TMP_Text resultText;

    [Header("Configuración")]
    public float moveSpeed = 500f;
    public int mixCount = 5;

    private int correctCup = -1;
    private bool isMixing = false;
    private bool isChoosing = false;

    private Vector2 candyStartPos;

    void Start()
    {
        // Guardamos la posición inicial del caramelo
        candyStartPos = candy.anchoredPosition;

        startButton.onClick.AddListener(StartChoosingCup);
        resultText.text = "Haz clic en un vaso para esconder el caramelo.";
    }

    public void OnCupClicked(int index)
    {
        if (!isChoosing || isMixing) return;

        correctCup = index;
        isChoosing = false;

        DisableCupButtons();

        resultText.text = "¡Caramelo escondido! Mezclando...";

        StartCoroutine(MoveCandyToCup(index));
    }

    void StartChoosingCup()
    {
        if (isMixing) return;
        resultText.text = "Elige un vaso para esconder el caramelo.";
        isChoosing = true;

        for (int i = 0; i < cups.Length; i++)
        {
            Button btn = cups[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = true;
                btn.onClick.RemoveAllListeners();
                int index = i;
                btn.onClick.AddListener(() => OnCupClicked(index));
            }
        }
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
            int b = Random.Range(0, cups.Length);
            if (a == b) continue;

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
        }

        isMixing = false;
        resultText.text = "¿Dónde está el caramelo?";
        EnableCupSelection();
    }

    void EnableCupSelection()
    {
        for (int i = 0; i < cups.Length; i++)
        {
            Button btn = cups[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = true;
                btn.onClick.RemoveAllListeners();
                int index = i;
                btn.onClick.AddListener(() => OnCupSelected(index));
                btn.onClick.AddListener(() => DisableCupButtons());
            }
        }
    }

    void DisableCupButtons()
    {
        for (int i = 0; i < cups.Length; i++)
        {
            Button btn = cups[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.interactable = false;
                btn.onClick.RemoveAllListeners();
            }
        }
    }

    public void OnCupSelected(int index)
    {
        if (isMixing || isChoosing) return;

        // Mostrar el caramelo en el vaso correcto
        candy.SetParent(cups[correctCup]);
        candy.anchoredPosition = Vector2.zero;

        if (index == correctCup)
            resultText.text = "¡Adivinaste! Ganaste 5× caramelos.";
        else
            resultText.text = "Fallaste. El caramelo estaba en otro vaso.";

        // Devolver el caramelo a su lugar original
        StartCoroutine(MoveCandyBack());
    }

    IEnumerator MoveCandyBack()
    {
        // Sacamos el caramelo del vaso
        candy.SetParent(transform);

        Vector2 start = candy.anchoredPosition;
        Vector2 end = candyStartPos;

        float duration = 0.5f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            candy.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        candy.anchoredPosition = end;
    }
}
