using UnityEngine;

public class PushPuzzleGoal : MonoBehaviour
{
    public DoorInteraction linkedDoor;


    [Header("Visual Feedback")]
    public Renderer zoneRenderer;
    public Material inactiveMaterial;
    public Material completedMaterial;


    [Header("Box Settings")]
    public Transform snapPoint;
    public Rigidbody pushableObject;


    private bool completed = false;


    void Start()
    {
        if (zoneRenderer != null)
        {
            zoneRenderer.material = inactiveMaterial;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (completed)
            return;


        if (other.CompareTag("Pushable"))
        {
            completed = true;


            Debug.Log("Push puzzle completed!");


            // Change zone color
            if (zoneRenderer != null)
            {
                zoneRenderer.material = completedMaterial;
            }


            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Stop movement
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;


                // Freeze physics
                rb.isKinematic = true;


                // Snap into place
                if (snapPoint != null)
                {
                    Vector3 newPosition = snapPoint.position;

                    // Preserve the object's current height above the ground
                    newPosition.y = other.transform.position.y;

                    other.transform.position = newPosition;
                    other.transform.rotation = snapPoint.rotation;
                }
            }


            if (linkedDoor != null)
            {
                linkedDoor.UnlockDoor();
            }
        }
    }
}