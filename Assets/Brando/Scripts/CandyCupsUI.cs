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

    void Start()
    {
        startButton.onClick.AddListener(StartChoosingCup);
        resultText.text = "Haz clic en un vaso para esconder el caramelo.";
    }

    public void OnCupClicked(int index)
    {
        if (!isChoosing || isMixing) return;

        correctCup = index;
        candy.SetParent(cups[index]);
        candy.anchoredPosition = Vector2.zero;

        resultText.text = "¡Caramelo escondido! Mezclando...";
        isChoosing = false;

        StartCoroutine(MixCups());
    }

    void StartChoosingCup()
    {
        if (isMixing) return;
        resultText.text = "Elige un vaso para esconder el caramelo.";
        isChoosing = true;
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
        foreach (RectTransform cup in cups)
        {
            Button btn = cup.GetComponent<Button>();
            if (btn != null)
                btn.interactable = true;
        }
    }

    public void OnCupSelected(int index)
    {
        if (isMixing || isChoosing) return;

        candy.SetParent(cups[correctCup]);
        candy.anchoredPosition = Vector2.zero;

        if (index == correctCup)
            resultText.text = "¡Adivinaste!  Ganaste 5× caramelos.";
        else
            resultText.text = "Fallaste  El caramelo estaba en otro vaso.";

        candy.SetParent(transform);
        candy.anchoredPosition = Vector2.zero;
    }
}
