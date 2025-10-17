using UnityEngine;
using UnityEngine.UI;

public class Card_Memory : MonoBehaviour
{
    [SerializeField] private Image IconImage;
    public Sprite iconSprite;
    public Sprite hiddenIconSprite;
    public bool isSelect = false;

    public CardsController controller;

    public void OnCardClick()
    {
        controller.SetSelected(this);
    }

    public void SetIconSprite(Sprite sp)
    {
        iconSprite = sp;
    }

    public void Show()
    {
        IconImage.sprite = iconSprite;
        isSelect = true;
    }

    public void Hide()
    {
        IconImage.sprite = hiddenIconSprite;
        isSelect = false;
    }
}
