using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;

public class LockpickBox : MonoBehaviour, IInteractable

{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string speakerName = "UNKNOWN";
    [SerializeField] private List<DialogueLine> dialogueLines = new();

    [SerializeField] private DialogueTrigger dialogueTrigger;

    private TextInteractConditional textInteract;

    private bool collected = false;
    public bool IsAvailable
    {
        get
        {
            return dialogueTrigger.triggered;
        }
    }

    void Start()
    {
        textInteract = GetComponent<TextInteractConditional>();
    }

    public void Interact()
    {
        if (collected)
            return;


        // Box is not available yet
        if (!IsAvailable)
        {
            Debug.Log(
                "Player does not know about the lockpicks yet."
            );

            return;
        }

        textInteract.StartText();

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        Debug.Log("Started waiting");
        yield return new WaitWhile(() => textInteract.textBoxOn);
        Debug.Log("Finished waiting");

        collected = true;


        // Give player permanent lockpicking ability
        PlayerAbilities.Instance.UnlockLockpicking();


        // Play pickup dialogue
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(
                speakerName,
                dialogueLines
            );
        }
        else
        {
            Debug.LogWarning(
                "DialogueManager not assigned on LockpickBox!"
            );
        }


        Destroy(gameObject, 0.2f);
    }
}