using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIGameController : MonoBehaviour
{
    [SerializeField] GameObject typeMinigame;
    [SerializeField] GameObject cupMinigame;
    [SerializeField] GameObject cardMinigame;
    [SerializeField] GameObject PanelCameramove;
    [SerializeField] GameObject pressPanel;
    [SerializeField] GameObject GameOver;

    [SerializeField] TextMeshProUGUI candies;

    private int currentDoor = 0;

    private void OnEnable()
    {
        PlayerHands.OnDoorTouched += HandleDoorTouched;
        PlayerHands.OnDoorExit += HandleDoorExit;
        PlayerHands.OnDoorEnter += ShowPressPanel;
        PlayerHands.OnDoorExit += HidePressPanel;
    }

    private void OnDisable()
    {
        PlayerHands.OnDoorTouched -= HandleDoorTouched;
        PlayerHands.OnDoorExit -= HandleDoorExit;
        PlayerHands.OnDoorEnter -= ShowPressPanel;
        PlayerHands.OnDoorExit -= HidePressPanel;
    }

    void Start()
    {
        typeMinigame.SetActive(false);
        cupMinigame.SetActive(false);
        cardMinigame.SetActive(false);
        pressPanel.SetActive(false);
    }
   public  void ShowPressPanel()
    {
        pressPanel.SetActive(true);
    }
    void HidePressPanel()
    {
        pressPanel.SetActive(false);
    }
    void Update()
    {
        candies.text = " Candies: " + GameManager.Instance.candiesQuantity.ToString();
        if (currentDoor != 0 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateMinigame(currentDoor);
            PanelCameramove.SetActive(false);
  
        }
    }

    void HandleDoorTouched(int doorNumber)
    {
        currentDoor = doorNumber;
        Debug.Log($"Estás frente a la puerta {doorNumber}. Presiona E para entrar.");
    }

    void HandleDoorExit()
    {
        currentDoor = 0;
        Debug.Log("Te alejaste de la puerta.");
    }
    
    void ActivateMinigame(int doorNumber)
    {
        switch (doorNumber)
        {
            case 1:
                typeMinigame.SetActive(true);
                typeMinigame.GetComponent<TypingChallenge>().StartGame();  
                Debug.Log("Minijuego de escribir activado.");
                break;
            case 2:
                cupMinigame.SetActive(true);
                Debug.Log("Minijuego de copas activado.");
                break;
            case 3:
                cardMinigame.SetActive(true);
                Debug.Log("Minijuego de cartas activado.");
                break;
        }
    }
}
