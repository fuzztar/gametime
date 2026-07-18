using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 2f;

    public bool isOpen = false;


    [Header("Lock Settings")]
    public bool locked = true;


    [Header("Door Object")]
    public Transform door;



    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;



    [Header("Locked Door Dialogue")]
    private TextInteractConditional textInteract;
    [SerializeField] private string[] text = new string[] { "The door is locked." };

    [Header("Unlocked Door Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string speakerName = "UNKNOWN";
    [SerializeField] private List<DialogueLine> doorDialogue = new();

    [SerializeField] private bool playDialogueOnlyOnce = true;

    private bool dialoguePlayed = false;



    [Header("Camera Focus")]
    [SerializeField] private bool useCameraFocus = false;
    [SerializeField] private Transform focusTarget;
    [SerializeField] private float focusSpeed = 3f;
    [SerializeField] private float focusHoldTime = 0.5f;



    private Quaternion closedRotation;
    private Quaternion openRotation;

    private Coroutine currentCoroutine;



    private void Start()
    {
        textInteract = GetComponent<TextInteractConditional>();
        closedRotation = door.rotation;

        openRotation = Quaternion.Euler(
            door.eulerAngles + new Vector3(0, openAngle, 0)
        );
    }



    private IEnumerator ToggleDoor()
    {
        bool opening = !isOpen;

        Quaternion targetRotation =
            opening ? openRotation : closedRotation;


        isOpen = opening;


        // Play door sound
        if (audioSource != null)
        {
            if (opening && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }
            else if (!opening && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
            }
        }



        while (Quaternion.Angle(door.rotation, targetRotation) > 0.01f)
        {
            door.rotation = Quaternion.Lerp(
                door.rotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );

            yield return null;
        }


        door.rotation = targetRotation;
    }





    public void Interact()
    {
        // Locked door
        if (locked)
        {

            textInteract.text = text;
            textInteract.StartText();

            return;
        } else
        {
            // Play unlocked door dialogue
            if (dialogueManager != null &&
                doorDialogue.Count > 0 &&
                (!dialoguePlayed || !playDialogueOnlyOnce))
            {
                dialoguePlayed = true;


                if (useCameraFocus &&
                    focusTarget != null)
                {
                    dialogueManager.StartDialogue(
                        speakerName,
                        doorDialogue,
                        focusTarget,
                        focusSpeed,
                        focusHoldTime
                    );
                }
                else
                {
                    dialogueManager.StartDialogue(
                        speakerName,
                        doorDialogue
                    );
                }
            }
        }




        // Open / close door
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }


        currentCoroutine = StartCoroutine(
            ToggleDoor()
        );
    }




    public void UnlockDoor()
    {
        locked = false;

        Debug.Log("Door unlocked!");
    }
}