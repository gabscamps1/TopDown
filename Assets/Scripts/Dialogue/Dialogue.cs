


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SentenceData
{
    public string characterName; // Nome específico para a sentença
    [TextArea(3, 10)]
    public string sentence;
    public Sprite sentenceImage;

    public Sprite backgroundImage;
}

[System.Serializable]
public class Dialogue
{
    public string name;
    //public Sprite npcImage;
    public List<SentenceData> sentences;
}

[System.Serializable]
public class DialogosPorFlag
{
    public string flag;
    public Dialogue dialogueData;
}

