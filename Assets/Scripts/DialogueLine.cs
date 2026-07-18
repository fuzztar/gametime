using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string text;

    [Header("Voice Line")]
    public AudioClip audioClip;


    [Header("Optional Sound Effect Before Voice")]
    public AudioClip preDialogueSound;

    public float soundDelay = 0f;
}