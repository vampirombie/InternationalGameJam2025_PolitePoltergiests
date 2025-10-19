using UnityEngine;

public class GameManager : SingletonPersistent<GameManager>
{
   
    [Header("Sentences")]
    int sentenceDifficulty = 1;
    [SerializeField] TextSentences easyLevel;
    [SerializeField] TextSentences mediumLevel;
    [SerializeField] TextSentences hardLevel;

    int candiesQuantity = 0;
    [Header("Cups")]
    int cupsDifficulty = 1;

    [Header("Cards")]
    int cardsDifficulty = 1;
    string chosenSentence;
    void Start()
    {

    }
                
    void Update()
    {

    }
     public string GetSentenceByDifficulty(int currentLevel)
    {
        switch (currentLevel)
        {
            case 1:
               chosenSentence=  easyLevel.GetSentence();
                break;
            case 2:
                chosenSentence=  mediumLevel.GetSentence();
                break;
            case 3:
                chosenSentence = hardLevel.GetSentence();
                break;
            default:
                chosenSentence = easyLevel.GetSentence();
                break;
        }
        return chosenSentence;
        

    }
    
    public void ReduceCandies(int betCandies)
    {
        candiesQuantity -= betCandies;
    }
    public void AddCandies(int betCandies)
    {
        candiesQuantity += betCandies;
    }
    
    /*public int Level()
    {
        return 
    }*/
}
