using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushForce = 4f;
    public float maxPushSpeed = 3f;
    public float acceleration = 8f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        // Ignore objects without rigidbodies or kinematic rigidbodies
        if (rb == null || rb.isKinematic)
            return;

        // Don't push downward
        if (hit.moveDirection.y < -0.3f)
            return;

        // Only push objects on the horizontal plane
        Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z).normalized;

        if (pushDirection.sqrMagnitude < 0.01f)
            return;

        // Desired horizontal velocity
        Vector3 desiredVelocity = pushDirection * maxPushSpeed;

        // Preserve the object's existing vertical movement
        desiredVelocity.y = rb.linearVelocity.y;

        // Smoothly move toward the desired velocity
        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            desiredVelocity,
            acceleration * Time.fixedDeltaTime
        );
    }
}
