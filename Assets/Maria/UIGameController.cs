using UnityEngine;

public class UIGameController : MonoBehaviour
{
    [SerializeField] GameObject typeMinigame;
    [SerializeField] GameObject cupMinigame;
    [SerializeField] GameObject cardMinigame;
    [SerializeField] GameObject PanelCameramove;

    private int currentDoor = 0;

    private void OnEnable()
    {
        PlayerHands.OnDoorTouched += HandleDoorTouched;
        PlayerHands.OnDoorExit += HandleDoorExit;
    }

    private void OnDisable()
    {
        PlayerHands.OnDoorTouched -= HandleDoorTouched;
        PlayerHands.OnDoorExit -= HandleDoorExit;
    }

    void Start()
    {
        typeMinigame.SetActive(false);
        cupMinigame.SetActive(false);
        cardMinigame.SetActive(false);
    }

    void Update()
    {
        if (currentDoor != 0 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateMinigame(currentDoor);
            PanelCameramove.SetActive(false);
            // Ya no borramos currentDoor aquí
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
