using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    private bool triggered = false;


    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;


        if (other.CompareTag("Player"))
        {
            triggered = true;

            dialogueManager.StartDialogue();
        }
    }
}