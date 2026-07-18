using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;


    [Header("Dialogue Settings")]
    public string speakerName = "UNKNOWN";

    public List<DialogueLine> dialogueLines;


    [Header("Audio")]
    public AudioSource audioSource;


    private void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }


    public void StartDialogue()
    {
        StartCoroutine(PlayDialogue());
    }


    private IEnumerator PlayDialogue()
    {
        dialoguePanel.SetActive(true);

        speakerText.text = speakerName;

        dialogueText.text = "";


        foreach (DialogueLine line in dialogueLines)
        {
            dialogueText.text += "\n\n" + line.text;


            if (line.audioClip != null)
            {
                audioSource.clip = line.audioClip;
                audioSource.Play();


                while (audioSource.isPlaying)
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }


        dialoguePanel.SetActive(false);
    }
}