using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] TextSentences TextSentences;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public  void CallSentence()
    {
        string text = TextSentences.GetSentence();
        print(text);
    }
}
