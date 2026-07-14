using UnityEngine;

public class LockInteractable : MonoBehaviour
{
    public void Interact()
    {
        FindObjectOfType<LockpickManager>().StartLockpicking(this);
    }

    public void Unlock()
    {
        Destroy(gameObject);
    }
}