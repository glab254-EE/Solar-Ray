using UnityEngine;

public class PlayerMovementInvoker
{
    private Rigidbody rb;
    internal void MovePlayer(Vector3 force)
    {
        float velocityY = rb.linearVelocity.y;
        Vector3 followingForce = Vector3.up * velocityY + force;
        rb.linearVelocity = followingForce;
    }
    internal void JumpPlayer(float force)
    {
        rb.AddForce(Vector3.up*force, ForceMode.VelocityChange);
    }
    public PlayerMovementInvoker(Rigidbody playerRigidBody)
    {
        rb = playerRigidBody;
    }
}
