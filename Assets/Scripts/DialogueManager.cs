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
        float focusHoldTime = 0.5f,
        AudioSource preCameraSoundSource = null,
        AudioClip preCameraSound = null,
        float soundDelay = 0.5f
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
                focusHoldTime,
                preCameraSoundSource,
                preCameraSound,
                soundDelay
            )
        );
    }






    private IEnumerator PlayDialogue(
        string speakerName,
        List<DialogueLine> dialogueLines,
        Transform focusTarget,
        float focusSpeed,
        float focusHoldTime,
        AudioSource preCameraSoundSource,
        AudioClip preCameraSound,
        float soundDelay
    )
    {
        Debug.Log("PlayDialogue started!");



        // Disable player controls
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }


        if (mouseLook != null)
        {
            mouseLook.canLook = false;
        }





        // Play sound BEFORE camera movement
        if (preCameraSound != null &&
            preCameraSoundSource != null)
        {
            preCameraSoundSource.PlayOneShot(
                preCameraSound
            );


            if (soundDelay > 0)
            {
                yield return new WaitForSeconds(
                    soundDelay
                );
            }
        }






        // Camera focus
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






        // Open dialogue box
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }



        if (speakerText != null)
        {
            speakerText.text = speakerName;
        }


        if (dialogueText != null)
        {
            dialogueText.text = "";
        }





        MusicManager.Instance?.LowerMusic();






        foreach (DialogueLine line in dialogueLines)
        {
            if (dialogueText != null)
            {
                dialogueText.text += line.text + "\n\n";
            }



            // Sound before individual line
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