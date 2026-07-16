using UnityEngine;

public class LockInteractable : MonoBehaviour
{
    public DoorInteraction connectedDoor;

    public void Interact()
    {
        FindObjectOfType<LockpickManager>().StartLockpicking(this);
    }

    public void Unlock()
    {
        // Unlock the door
        if (connectedDoor != null)
        {
            connectedDoor.UnlockDoor();
        }

        // Remove the lock
        Destroy(gameObject);
    }
}