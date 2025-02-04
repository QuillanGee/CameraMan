using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Need this for scene management

public class TwoDCharacterMovement : MonoBehaviour {

    public float speed = 3f;
    private float JumpHeight = 5f;
    private bool isGrounded = true;
    [SerializeField] private Transform groundCheckPosition;
    private float groundCheckRadius = 0.4f;
    public LayerMask groundLayer;
    

    private Rigidbody2D rb;
    
    private Animator currentAnimator;
    private RuntimeAnimatorController alanAnimatorController;
    [SerializeField] private AnimatorOverrideController holdingOverrideController;
    private bool facingRight = false;
    
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private string currentState;
    private const string PLAYER_IDLE = "Idle";
    private const string PLAYER_RUN = "Run";
    private const string PLAYER_JUMP = "Jump";



    void Start() {
        rb = GetComponent<Rigidbody2D>();
        currentAnimator = GetComponentInChildren<Animator>();
        alanAnimatorController = currentAnimator.runtimeAnimatorController;
        
        EventManager.instance.OnHoldingBlock += SwitchToHoldBlockAnimationController;
        EventManager.instance.OnNotHoldingBlock += SwitchToAlanAnimationController;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            //to make sure rb doesn't go to sleep when character is staying still
            rb.WakeUp();
            if (Input.GetKeyDown(KeyCode.W))
            {
                // Get the current scene's build index
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

                // Calculate the next scene index
                int nextSceneIndex = currentSceneIndex + 1;

                // Check if the next scene index is within the range of available scenes
                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    // Load the next scene
                    SceneManager.LoadScene(nextSceneIndex);
                }
            }
        }
    }

    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * -speed, rb.velocity.y);
        
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position,groundCheckRadius, groundLayer);

        // Set animation move state (you can adjust this for different animations)
        if (moveHorizontal != 0)
        {
            // Check if the player is moving right and is not already facing right
            if (moveHorizontal > 0 && !facingRight)
            {
                // Walk right
                facingRight = true;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            // Check if the player is moving left and is not already facing left
            else if (moveHorizontal < 0 && facingRight)
            {
                // Walk left
                facingRight = false;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }


        if (isGrounded)
        {
            if (moveHorizontal != 0)
            {
                currentAnimator.SetInteger("moveState", 1);
            }
            else
            {
                currentAnimator.SetInteger("moveState", 0);
            }
        }
        else
        {
            if (rb.velocity.y > 0.1)
            {
                currentAnimator.SetInteger("moveState", 2);
            }
            else
            {
                currentAnimator.SetInteger("moveState", 3);
            }
        }
       
        
        //FOR DELAYED JUMPING
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f) 
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpHeight);
        }

        if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        //allows us to press jump before hitting the ground
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        //allows us to press jump after leaving the groud
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    void Flip()
    {
        // Flip the character by changing its x-scale
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void SwitchToHoldBlockAnimationController()
    {
        currentAnimator.runtimeAnimatorController = holdingOverrideController;
    }

    private void SwitchToAlanAnimationController()
    {
        currentAnimator.runtimeAnimatorController = alanAnimatorController;
    }
}