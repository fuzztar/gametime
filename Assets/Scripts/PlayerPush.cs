using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushForce = 50f;
    public float maxPushSpeed = 3f;

    private CharacterController controller;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;


        // Ignore objects without rigidbodies
        if (rb == null || rb.isKinematic)
            return;


        // Prevent pushing objects downward
        if (hit.moveDirection.y < -0.3f)
            return;


        // Get horizontal push direction
        Vector3 pushDirection = new Vector3(
            hit.moveDirection.x,
            0,
            hit.moveDirection.z
        );


        // Smooth push force
        rb.AddForce(
     pushDirection * pushForce,
     ForceMode.Impulse
 );


        // Limit maximum speed
        if (rb.linearVelocity.magnitude > maxPushSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * maxPushSpeed;
        }
    }
}