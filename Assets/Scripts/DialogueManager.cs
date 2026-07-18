using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Audio")]
    [SerializeField] private AudioSource dialogueAudioSource;


    private void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }


    public void StartDialogue(string speakerName, List<DialogueLine> dialogueLines)
    {
        StopAllCoroutines();
        StartCoroutine(PlayDialogue(speakerName, dialogueLines));
    }


    private IEnumerator PlayDialogue(
        string speakerName,
        List<DialogueLine> dialogueLines
    )
    {
        dialoguePanel.SetActive(true);

        speakerText.text = speakerName;
        dialogueText.text = "";

        // Lower background music while dialogue is playing
        MusicManager.Instance?.LowerMusic();

        foreach (DialogueLine line in dialogueLines)
        {
            dialogueText.text += line.text + "\n\n";

            if (line.audioClip != null)
            {
                dialogueAudioSource.clip = line.audioClip;
                dialogueAudioSource.Play();

                while (dialogueAudioSource.isPlaying)
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }

        // Restore music volume
        MusicManager.Instance?.RestoreMusic();

        dialoguePanel.SetActive(false);
    }
}