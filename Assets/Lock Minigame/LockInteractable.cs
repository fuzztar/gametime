using UnityEngine;

public class LockInteractable : MonoBehaviour, IInteractable
{
    [Header("Lockpick Manager")]
    [SerializeField] private LockpickManager lockpickManager;


    [Header("Door")]
    [SerializeField] private DoorInteraction linkedDoor;


    [Header("UI")]
    [SerializeField] private ScrollingText scrollingText;


    private bool unlocked = false;



    public void Interact()
    {
        if (unlocked)
        {
            Debug.Log("Lock is already unlocked.");
            return;
        }


        if (PlayerAbilities.Instance == null)
        {
            Debug.LogWarning("No PlayerAbilities found!");
            return;
        }


        // Player does not have lockpicks yet
        if (!PlayerAbilities.Instance.CanLockpick)
        {
            Debug.Log("Player does not have lockpicks.");

            if (scrollingText != null)
            {
                scrollingText.itemInfo = new string[]
                {
                    "I need something to pick this lock."
                };

                scrollingText.gameObject.SetActive(true);
            }

            return;
        }


        // Player has lockpicks
        if (lockpickManager != null)
        {
            lockpickManager.StartLockpicking(this);
        }
        else
        {
            Debug.LogWarning("No LockpickManager assigned!");
        }
    }



    public void Unlock()
    {
        unlocked = true;

        Debug.Log("Lock unlocked!");


        if (linkedDoor != null)
        {
            linkedDoor.UnlockDoor();
        }
        else
        {
            Debug.LogWarning("No DoorInteraction assigned to lock!");
        }


        Destroy(gameObject);
    }
}