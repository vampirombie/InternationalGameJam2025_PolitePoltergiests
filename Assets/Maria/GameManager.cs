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
    void Start()
    {

    }
                
    void Update()
    {

    }
     public void SelectedLevel()
    {
        if (sentenceDifficulty == 1)
        {
            easyLevel.GetSentence();

        }
        else if (sentenceDifficulty == 2)
        {
            mediumLevel.GetSentence();
        }
        else if (sentenceDifficulty == 3)
        {
            hardLevel.GetSentence();
        }
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
