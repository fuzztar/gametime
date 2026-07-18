using UnityEngine;

public class LockpickManager : MonoBehaviour
{
    [Header("Lockpick UI")]
    public GameObject lockpickCanvas;
    public GameObject lockpickInstructions;


    [Header("Minigame")]
    public LockpickMinigame minigame;


    [Header("Player References")]
    public PlayerMovement playerMovement;
    public PlayerInteract playerInteract;


    [Header("Completion Sound")]
    [SerializeField] private AudioSource completionAudioSource;
    [SerializeField] private AudioClip completionSound;


    private LockInteractable currentLock;



    public void StartLockpicking(LockInteractable lockToPick)
    {
        currentLock = lockToPick;


        playerMovement.enabled = false;
        playerInteract.canInteract = false;


        lockpickCanvas.SetActive(true);


        if (lockpickInstructions != null)
        {
            lockpickInstructions.SetActive(true);
        }


        ObjectHighlighter highlighter =
            FindFirstObjectByType<ObjectHighlighter>();

        if (highlighter != null)
        {
            highlighter.uiOpen = true;
        }


        minigame.StartMinigame();


        Debug.Log("Lockpicking started!");
    }



    public void CompleteLock()
    {
        Debug.Log("Lockpick completed!");


        // Play completion sound
        if (completionAudioSource != null &&
            completionSound != null)
        {
            completionAudioSource.PlayOneShot(
                completionSound
            );
        }


        if (currentLock != null)
        {
            currentLock.Unlock();
        }


        StopLockpicking();
    }



    public void StopLockpicking()
    {
        Debug.Log("Stopping lockpick UI!");


        if (minigame != null)
        {
            minigame.StopMinigame();
        }


        if (lockpickCanvas != null)
        {
            lockpickCanvas.SetActive(false);
        }


        if (lockpickInstructions != null)
        {
            lockpickInstructions.SetActive(false);
        }


        ObjectHighlighter highlighter =
            FindFirstObjectByType<ObjectHighlighter>();

        if (highlighter != null)
        {
            highlighter.uiOpen = false;
        }


        playerMovement.enabled = true;
        playerInteract.canInteract = true;
    }
}