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


    [Header("Locked Message")]
    public ScrollingText scrollingText;


    [Header("Opening Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string speakerName = "UNKNOWN";
    [SerializeField] private List<DialogueLine> openDialogue = new();



    private Quaternion closedRotation;
    private Quaternion openRotation;

    private Coroutine currentCoroutine;

    private bool playedOpenDialogue = false;



    private void Start()
    {
        closedRotation = door.rotation;

        openRotation = Quaternion.Euler(
            door.eulerAngles + new Vector3(0, openAngle, 0)
        );
    }



    private IEnumerator ToggleDoor()
    {
        Quaternion targetRotation =
            isOpen ? closedRotation : openRotation;


        isOpen = !isOpen;


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


        // Play dialogue after door finishes opening
        if (isOpen && !playedOpenDialogue)
        {
            playedOpenDialogue = true;

            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue(
                    speakerName,
                    openDialogue
                );
            }
        }
    }



    public void Interact()
    {
        if (locked)
        {
            Debug.Log("Door is locked!");

            if (scrollingText != null)
            {
                scrollingText.itemInfo = new string[]
                {
                    "The door is locked."
                };

                scrollingText.gameObject.SetActive(true);
            }

            return;
        }



        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }


        currentCoroutine = StartCoroutine(ToggleDoor());
    }



    public void UnlockDoor()
    {
        locked = false;

        Debug.Log("Door unlocked!");
    }
}