using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "TextSentences", menuName = "Scriptable Objects/TextSentences")]
public class TextSentences : ScriptableObject
{
    [TextArea]
    [SerializeField] List<string> sentences = new List<string>();

    public string GetSentence()
    {
        int getRandomSentence= Random.Range(0,sentences.Count);
        Debug.Log(sentences[getRandomSentence]);
        return sentences[getRandomSentence];
       
    }


}

