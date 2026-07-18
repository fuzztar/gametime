using System.Collections;
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


    [Header("Unlocks")]
    [SerializeField] private bool unlockLockpickBox = false;


    [Header("Sound Effect Before Dialogue")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip preDialogueSound;
    [SerializeField] private float delayBeforeDialogue = 1f;


    private bool triggered = false;



    private void OnTriggerEnter(Collider other)
    {
        if (triggered && triggerOnce)
            return;


        if (!other.CompareTag("Player"))
            return;


        triggered = true;


        StartCoroutine(PlaySequence());
    }



    private IEnumerator PlaySequence()
    {
        // Play optional sound effect
        if (audioSource != null && preDialogueSound != null)
        {
            audioSource.PlayOneShot(preDialogueSound);

            yield return new WaitForSeconds(delayBeforeDialogue);
        }


        // Start dialogue
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(
                speakerName,
                dialogueLines
            );
        }
        else
        {
            Debug.LogWarning("DialogueManager not assigned!");
        }


        // Unlock lockpick box if needed
        if (unlockLockpickBox)
        {
            if (PlayerAbilities.Instance != null)
            {
                PlayerAbilities.Instance.UnlockLockpickBox();
            }
            else
            {
                Debug.LogWarning("No PlayerAbilities found!");
            }
        }
    }
}