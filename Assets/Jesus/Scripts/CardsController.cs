using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CardsController : MonoBehaviour
{
    private MemoryGameManager gameManager;

    [SerializeField] Card_Memory cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] Sprite[] sprites;

    private List<Sprite> spritePairs = new List<Sprite>();

    private Card_Memory firstSelected;
    private Card_Memory secondSelected;
    private bool canSelect = true;

    public List<Card_Memory> cards = new List<Card_Memory>();

    private void Start()
    {
        PrepareSprites();
        CreateCards();
        StartCoroutine(ShowAllThenHide());
        gameManager = Object.FindFirstObjectByType<MemoryGameManager>();

    }

    IEnumerator ShowAllThenHide()
    {
        foreach (Transform child in gridTransform)
        {
            Card_Memory card = child.GetComponent<Card_Memory>();
            card.Show();
        }

        yield return new WaitForSeconds(2f);

        foreach (Transform child in gridTransform)
        {
            Card_Memory card = child.GetComponent<Card_Memory>();
            card.Hide();
        }

        canSelect = true;
    }


    private void PrepareSprites()
    {
        spritePairs.Clear();

        for (int i = 0; i < sprites.Length; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }

        ShuffleSprites(spritePairs);
    }

    void CreateCards()
    {
        foreach (Sprite sp in spritePairs)
        {
            Card_Memory card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(sp);
            card.controller = this;
            card.Hide();

            cards.Add(card);
        }

        Debug.Log($"Total de cartas generadas: {cards.Count}");
    }


    public void SetSelected(Card_Memory card)
    {
        if (!canSelect || card.isSelect) return;

        card.Show();

        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else
        {
            secondSelected = card;
            canSelect = false;
            StartCoroutine(CheckMatching());
        }
    }

    IEnumerator CheckMatching()
    {
        yield return new WaitForSeconds(0.5f);

        if (firstSelected.iconSprite == secondSelected.iconSprite)
        {
            gameManager.OnPairMatched();

            firstSelected.GetComponent<UnityEngine.UI.Button>().interactable = false;
            secondSelected.GetComponent<UnityEngine.UI.Button>().interactable = false;

            firstSelected.isSelect = true;
            secondSelected.isSelect = true;
        }
        else
        {
            gameManager.OnPairMistake();

            firstSelected.Hide();
            secondSelected.Hide();

            firstSelected.isSelect = false;
            secondSelected.isSelect = false;
        }

        firstSelected = null;
        secondSelected = null;
        canSelect = true;

        yield break;
    }

    void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }
}
