using UnityEngine;

public class ComputerInteractable : MonoBehaviour, IInteractable
{
    public bool hacked = false;

    public DoorInteraction linkedDoor;


    public void Interact()
    {
        if (hacked)
        {
            Debug.Log("Computer already hacked!");
            return;
        }


        FindFirstObjectByType<HackingManager>()
            .StartHacking(this);
    }



    public void HackComplete()
    {
        hacked = true;


        Debug.Log(
            "Computer successfully hacked!"
        );


        if (linkedDoor != null)
        {
            linkedDoor.UnlockDoor();
        }
    }
}