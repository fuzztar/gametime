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


    private bool triggered = false;


    private void OnTriggerEnter(Collider other)
    {
        if (triggered && triggerOnce)
            return;


        if (!other.CompareTag("Player"))
            return;


        triggered = true;


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


        if (unlockLockpickBox)
        {
            if (PlayerAbilities.Instance != null)
            {
                PlayerAbilities.Instance.UnlockLockpickBox();
            }
            else
            {
                Debug.LogWarning(
                    "No PlayerAbilities found!"
                );
            }
        }
    }
}