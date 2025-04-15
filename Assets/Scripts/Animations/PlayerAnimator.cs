using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public float speedThreshold = 0.1f;

    void Update()
    {
        // Get player input speed
        float speed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        // Set Speed parameter
        animator.SetFloat("Speed", speed);

        // Jump input (example with spacebar)
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            animator.SetBool("IsJumping", true);
        }

        // Reset jumping (when grounded again)
        if (controller.isGrounded && animator.GetBool("IsJumping"))
        {
            animator.SetBool("IsJumping", false);
        }
    }
}