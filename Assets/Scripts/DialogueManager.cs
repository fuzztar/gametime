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


    [Header("Player Control")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLook;


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
        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }


        // Optional: disable looking
        if (mouseLook != null)
        {
            mouseLook.canLook = false;
        }



        dialoguePanel.SetActive(true);

        speakerText.text = speakerName;
        dialogueText.text = "";


        MusicManager.Instance?.LowerMusic();



        foreach (DialogueLine line in dialogueLines)
        {
            dialogueText.text += line.text + "\n\n";


            // Optional sound before voice line
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



            // Voice line
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



        // Restore player control
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }


        if (mouseLook != null)
        {
            mouseLook.canLook = true;
        }



        MusicManager.Instance?.RestoreMusic();


        dialoguePanel.SetActive(false);
    }
}