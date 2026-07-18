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

    [SerializeField] private TextInteractConditional textInteract;

    private bool collected = false;
    public bool IsAvailable
    {
        get
        {
            return PlayerAbilities.Instance != null &&
                   PlayerAbilities.Instance.HasHeardLockpickHint;
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


        if (PlayerAbilities.Instance == null)
        {
            Debug.LogWarning("No PlayerAbilities found!");
            return;
        }


        // Box is not available yet
        if (!PlayerAbilities.Instance.HasHeardLockpickHint)
        {
            Debug.Log(
                "Player does not know about the lockpicks yet."
            );

            return;
        }

        textInteract.StartText();

        while (textInteract.textBoxOn)
        {
            StartCoroutine(WaitFor(0.1f));
        }

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

    IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}