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
    [SerializeField] private AudioSource soundEffectAudioSource;



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

        StartCoroutine(
            PlayDialogue(
                speakerName,
                dialogueLines
            )
        );
    }



    private IEnumerator PlayDialogue(
        string speakerName,
        List<DialogueLine> dialogueLines
    )
    {
        dialoguePanel.SetActive(true);

        speakerText.text = speakerName;
        dialogueText.text = "";


        MusicManager.Instance?.LowerMusic();



        foreach (DialogueLine line in dialogueLines)
        {
            // Add text to transcript
            dialogueText.text += line.text + "\n\n";


            // Play optional sound effect first
            if (line.preDialogueSound != null)
            {
                if (soundEffectAudioSource != null)
                {
                    soundEffectAudioSource.PlayOneShot(
                        line.preDialogueSound
                    );
                }


                if (line.soundDelay > 0)
                {
                    yield return new WaitForSeconds(
                        line.soundDelay
                    );
                }
            }



            // Play voice line
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



        MusicManager.Instance?.RestoreMusic();


        dialoguePanel.SetActive(false);
    }
}