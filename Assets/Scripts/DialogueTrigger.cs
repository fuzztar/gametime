using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private DialogueManager dialogueManager;


    [Header("Dialogue")]
    [SerializeField] private string speakerName = "UNKNOWN";
    [SerializeField] private List<DialogueLine> dialogueLines = new();


    [Header("Settings")]
    [SerializeField] private bool triggerOnce = true;


    [Header("Camera Focus")]
    [SerializeField] private bool useCameraFocus = false;
    [SerializeField] private Transform focusTarget;
    [SerializeField] private float focusSpeed = 3f;
    [SerializeField] private float focusHoldTime = 0.5f;


    [Header("Pre Camera Sound")]
    [SerializeField] private AudioSource soundEffectSource;
    [SerializeField] private AudioClip preCameraSound;
    [SerializeField] private float soundDelay = 0.5f;



    private bool triggered = false;



    private void OnTriggerEnter(Collider other)
    {
        if (triggered && triggerOnce)
            return;


        if (!other.CompareTag("Player"))
            return;


        Debug.Log("Dialogue Trigger activated!");


        triggered = true;



        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager not assigned!");
            return;
        }


        if (dialogueLines.Count == 0)
        {
            Debug.LogWarning("No dialogue lines assigned!");
            return;
        }



        if (useCameraFocus && focusTarget != null)
        {
            Debug.Log("Starting focused dialogue!");


            dialogueManager.StartDialogue(
                speakerName,
                dialogueLines,
                focusTarget,
                focusSpeed,
                focusHoldTime,
                soundEffectSource,
                preCameraSound,
                soundDelay
            );
        }
        else
        {
            Debug.Log("Starting normal dialogue!");


            dialogueManager.StartDialogue(
                speakerName,
                dialogueLines,
                null,
                focusSpeed,
                focusHoldTime,
                soundEffectSource,
                preCameraSound,
                soundDelay
            );
        }
    }
}