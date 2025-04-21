using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Need this for scene management

public class TwoDCharacterMovement : MonoBehaviour {

    public float speed = 3f;
    private float JumpHeight = 6f;
    private bool isGrounded = true;
    [SerializeField] private Transform groundCheckPosition;
    private float groundCheckRadius = 0.2f;
    private float minGroundCheckRadius = 0.1f;
    private float maxGroundCheckRadius = 0.3f;
    private bool isOnStairs = false;  // Whether the character is on stairs
    private bool isClimbing = false;  // Whether the character is climbing the stairs
    private Collider2D currentStairCollider;  // The current staircase collider

    
    [SerializeField] private LayerMask focusedObjects;

    private Rigidbody2D rb;
    
    [SerializeField] private Animator currentAnimator;
    private RuntimeAnimatorController alanAnimatorController;
    [SerializeField] private AnimatorOverrideController holdingOverrideController;
    private bool facingRight = false;
    
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    private string currentState;
    private const string PLAYER_IDLE = "Idle";
    private const string PLAYER_RUN = "Run";
    private const string PLAYER_JUMP = "Jump";
    
    private bool enterdoor = false;
    private bool overlappedDoor = false;
    [SerializeField] private Collider2D playerCollider;
    private Collider2D doorCollider;
    private Door2D otherDoor;

    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        // currentAnimator = GetComponentInChildren<Animator>();
        alanAnimatorController = currentAnimator.runtimeAnimatorController;
        
        EventManager.instance.OnHoldingBlock += SwitchToHoldBlockAnimationController;
        EventManager.instance.OnNotHoldingBlock += SwitchToAlanAnimationController;
        EventManager.instance.OnPauseGamePlay += HandlePause;
        
    }

    void Update() {
        if (isClimbing)
            return; // Disable other controls while climbing

        if (enterdoor)
            return;
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * -speed, rb.velocity.y);
        
        groundCheckRadius = Mathf.Clamp(0.2f * transform.localScale.y, minGroundCheckRadius, maxGroundCheckRadius);
        isGrounded = Physics2D.OverlapCircle(groundCheckPosition.position,groundCheckRadius, focusedObjects);
        
        // Set animation move state (you can adjust this for different animations)
        if (moveHorizontal != 0)
        {
            // Check if the player is moving right and is not already facing right
            if (moveHorizontal > 0 && !facingRight)
            {
                // Walk right
                facingRight = true; 
                Flip();
            }
            // Check if the player is moving left and is not already facing left
            else if (moveHorizontal < 0 && facingRight)
            {
                // Walk left
                facingRight = false;
                Flip();
            }
        }
        // else
        // {
        //     StandStraight();
        // }


        if (isGrounded)
        {
            if (moveHorizontal != 0)
            {
                currentAnimator.SetInteger("AnimInt", 1);
            }
            else
            {
                currentAnimator.SetInteger("AnimInt", 0);
            }
        }
        else
        {
            if (rb.velocity.y > 0.1)
            {
                currentAnimator.SetInteger("AnimInt", 2);
            }
            else
            {
                currentAnimator.SetInteger("AnimInt", 0);
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
        
        if (isOnStairs && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(ClimbStairs());
        }

        if ((overlappedDoor) && Input.GetKeyDown(KeyCode.W))
        {
            TryEnterDoor();
        }
    }
    void Flip()
    {
        Vector3 myRotation = transform.rotation.eulerAngles;
        myRotation.y += 180f;
        transform.rotation = Quaternion.Euler(myRotation);
    }
    
    // void StandStraight()
    // {
    //     Vector3 myRotation = new Vector3(0f, 0f, 0f);
    //     transform.rotation = Quaternion.Euler(myRotation);
    // }
    
    private IEnumerator ClimbStairs()
    {
        isClimbing = true;

        // Lock player controls during climbing
        float startY = transform.position.y;
        float startZ = transform.position.z;
        float endY = startY + currentStairCollider.bounds.size.y;  // Use collider height for the stair height
        float climbDuration = 2f;  // Time to reach the top of the stairs

        float timeElapsed = 0f;

        // Disable horizontal and jumping control during climbing
        float startX = transform.position.x;

        while (timeElapsed < climbDuration)
        {
            transform.position = new Vector3(startX, Mathf.Lerp(startY, endY, timeElapsed / climbDuration), startZ);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    
        // transform.position = new Vector3(startX, endY, startZ); // Ensure we reach the exact end position

        // Allow controls again after climbing
        isClimbing = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
        {
            isOnStairs = true; // Start detecting 'W' key to climb
            currentStairCollider = other;  // Store reference to the current staircase collider
        }

        if (other.CompareTag("Door"))  // Check if the other object is the player
        {
            doorCollider = other;
            overlappedDoor = true;
            otherDoor = other.gameObject.GetComponent<Door2D>();
        }
    }
    
    // Trigger when the character leaves the stairs area
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
        {
            isOnStairs = false; // Stop detecting 'W' key
            currentStairCollider = null;  // Remove reference when leaving
        }
        if (other.CompareTag("Door"))  // Check if the other object is the player
        {
            overlappedDoor = false;
            doorCollider = null;
        }
    }

    private void TryEnterDoor()
    {
        // Check if the player is too large to enter the door
        if (IsPlayerTooBigForDoor())
        {
            Debug.Log("Player is too big to enter the door!");
        }
        else
        {
            StartCoroutine(EnterDoor());
        }
    }

    private IEnumerator EnterDoor()
    {
        enterdoor = true;
        otherDoor.TriggerDoorAnimation();
        yield return new WaitForSeconds(0.5f);
        otherDoor.TriggerCorrespondingDoorAnimation();
        transform.position = new Vector3(otherDoor.getCorrespondingDoorTransform().transform.position.x, otherDoor.getCorrespondingDoorTransform().transform.position.y, transform.position.z);
        enterdoor = false;
    }
    
    bool IsPlayerTooBigForDoor()
    {
        // Get the size of both the player's and the door's colliders
        Vector2 playerSize = playerCollider.bounds.size;
        Vector2 doorSize = doorCollider.bounds.size;

        // Compare the player's size to the door's size
        if (playerSize.x > doorSize.x || playerSize.y > doorSize.y)
        {
            return true;  // Player is too big for the door
        }

        return false;  // Player can enter
    }
    
    

    private void SwitchToHoldBlockAnimationController()
    {
        currentAnimator.runtimeAnimatorController = holdingOverrideController;
    }

    private void SwitchToAlanAnimationController()
    {
        currentAnimator.runtimeAnimatorController = alanAnimatorController;
    }
    
    void OnDrawGizmos()
    {
        if (groundCheckPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
        }
    }
    
    
    
    private void HandlePause(object sender, bool isPaused)
    {
        if (isPaused)
        {
            rb.gravityScale = 0;  // Disable gravity
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;  // Freeze all position axes
        }
        else
        {
            rb.gravityScale = 2;  // Re-enable gravity
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}