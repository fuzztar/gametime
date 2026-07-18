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



    public void StartDialogue(
        string speakerName,
        List<DialogueLine> dialogueLines,
        Transform focusTarget = null,
        float focusSpeed = 3f,
        float focusHoldTime = 0.5f
    )
    {
        Debug.Log("DialogueManager received dialogue!");

        StopAllCoroutines();

        StartCoroutine(
            PlayDialogue(
                speakerName,
                dialogueLines,
                focusTarget,
                focusSpeed,
                focusHoldTime
            )
        );
    }





    private IEnumerator PlayDialogue(
        string speakerName,
        List<DialogueLine> dialogueLines,
        Transform focusTarget,
        float focusSpeed,
        float focusHoldTime
    )
    {
        Debug.Log("PlayDialogue started!");



        // Disable player control
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }


        if (mouseLook != null)
        {
            mouseLook.canLook = false;
        }



        // Focus camera if enabled
        if (focusTarget != null &&
            CameraFocusManager.Instance != null)
        {
            yield return StartCoroutine(
                CameraFocusManager.Instance.FocusOnTarget(
                    focusTarget,
                    focusSpeed,
                    focusHoldTime
                )
            );
        }



        // Open dialogue UI
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            Debug.Log("Dialogue panel opened!");
        }
        else
        {
            Debug.LogWarning("Dialogue Panel is not assigned!");
        }



        if (speakerText != null)
        {
            speakerText.text = speakerName;
        }


        if (dialogueText != null)
        {
            dialogueText.text = "";
        }



        // Lower music
        MusicManager.Instance?.LowerMusic();



        foreach (DialogueLine line in dialogueLines)
        {
            Debug.Log("Playing line: " + line.text);



            // Add text
            if (dialogueText != null)
            {
                dialogueText.text += line.text + "\n\n";
            }



            // Play optional sound effect before voice line
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



        // Return camera
        if (focusTarget != null &&
            CameraFocusManager.Instance != null)
        {
            yield return StartCoroutine(
                CameraFocusManager.Instance.ReturnControl(
                    focusSpeed
                )
            );
        }
        else
        {
            if (mouseLook != null)
            {
                mouseLook.canLook = true;
            }
        }



        // Restore player
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }



        MusicManager.Instance?.RestoreMusic();



        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }


        Debug.Log("Dialogue finished!");
    }
}