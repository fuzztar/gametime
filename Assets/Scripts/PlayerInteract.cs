using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;

    public bool canInteract = true;

    void Update()
    {
        // Draw interaction ray
        Debug.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * interactDistance,
            Color.red
        );

        // Stop interaction while lockpicking
        if (!canInteract)
            return;

        // Check if player presses E
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }


    void TryInteract()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            LockInteractable lockObject =
                hit.collider.GetComponent<LockInteractable>();

            if (lockObject != null)
            {
                Debug.Log("Lock found!");
                lockObject.Interact();
            }
            else
            {
                Debug.Log("Object is not a lock");
            }
        }
        else
        {
            Debug.Log("Ray hit nothing");
        }
    }
}