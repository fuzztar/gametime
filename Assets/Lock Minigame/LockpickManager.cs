using UnityEngine;

public class LockpickManager : MonoBehaviour
{
    public GameObject lockpickCanvas;
    public LockpickMinigame minigame;

    public PlayerMovement playerMovement;
    public PlayerInteract playerInteract;

    private LockInteractable currentLock;

    public void StartLockpicking(LockInteractable lockToPick)
    {
        currentLock = lockToPick;

        // Disable player movement
        playerMovement.enabled = false;
        playerInteract.canInteract = false;

        lockpickCanvas.SetActive(true);

        minigame.StartMinigame();

        Debug.Log("Lockpicking started!");
    }


    public void CompleteLock()
    {
        currentLock.Unlock();

        StopLockpicking();
    }


    public void StopLockpicking()
    {
        Debug.Log("Stopping lockpick UI!");

        minigame.StopMinigame();

        Debug.Log("Disabling canvas: " + lockpickCanvas.name);

        lockpickCanvas.SetActive(false);

        playerMovement.enabled = true;
        playerInteract.canInteract = true;
    }
}