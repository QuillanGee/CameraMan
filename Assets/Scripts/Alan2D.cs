using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Need this for scene management

public class Alan2D : MonoBehaviour
{
    [SerializeField] Transform projectedWallTransform;
    [SerializeField] GameObject Alan;
    [SerializeField] private Transform holdPosition2D;
    private Vector3 alanDefaultScale;
    private Vector3 startingPosition;
    private ObjectProjection currentHeldObjectProjection;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        alanDefaultScale = transform.localScale;
        startingPosition = transform.position;
        
        EventManager.instance.OnToggleTwoD += ProjectAlanToMoveAlan2D;
        EventManager.instance.OnHoldingBlock += SetObjectionProjectionInstance;
        EventManager.instance.OnHoldingBlock += AttachBlockToAlan2D;
        EventManager.instance.OnResetAlan2D += ResetPosition;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }
    
    public void ProjectAlanToMoveAlan2D()
    {
        //gets 2D Alan's direction
        int direction = transform.localScale.x > 0 ? 1 : -1;
        
        //Scales based on distance from InvisaWall
        float distanceToPlane = projectedWallTransform.position.z - Alan.transform.position.z;
        float scaleFactor =  3*(1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane))); // Avoid division by zero
        Vector3 theScale = alanDefaultScale * scaleFactor;
        theScale.x *= direction;
        transform.localScale = theScale;
        
        //Moves to corresponding X position
        Vector3 newXPosition = transform.position;
        newXPosition.x = Alan.transform.position.x;
        transform.position = newXPosition;
        
        //Moves to corresponding Y position
        Vector3 newYPosition = transform.position;
        newYPosition.y = Alan.transform.position.y;
        transform.position = newYPosition;
    }

    private void SetObjectionProjectionInstance()
    {
        currentHeldObjectProjection = GetComponentInChildren<ObjectProjection>();
    }
    
    private void AttachBlockToAlan2D()
    {
        // Determine the direction the character is facing
        float direction = Mathf.Sign(transform.localScale.x);

        // Calculate the new position of the block
        Vector3 newPosition = transform.position + new Vector3(direction * 0.7f, 1f, 0f);
        
        
        currentHeldObjectProjection.PositionBlockToHoldPosition(holdPosition2D.position);
        currentHeldObjectProjection.SetBlockParent(transform);
    }

    private void ResetPosition()
    {
        transform.position = startingPosition;
    }
}
