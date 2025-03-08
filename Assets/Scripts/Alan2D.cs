using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;  // Need this for scene management

public class Alan2D : MonoBehaviour
{
    [SerializeField] Transform projectedWallTransform;
    [SerializeField] Transform Alan;
    private Vector3 alanDefaultScale;
    private Vector3 startingPosition;
    private float initialDistanceFromWall;
    private float scaleFactor = 1f;
    
    private Rigidbody2D rb;
    
    private float bouncePadForce = 2.5f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        alanDefaultScale = transform.localScale;
        startingPosition = transform.position;
        initialDistanceFromWall = Mathf.Abs(Alan.transform.position.z - projectedWallTransform.position.z);
        
        EventManager.instance.OnToggleTwoD += ProjectAlanToMoveAlan2D;
        EventManager.instance.OnResetAlan2D += ResetPosition;
        EventManager.instance.OnPostToggleFirstPerson += HideAlan2D;
        EventManager.instance.OnPostToggleTwoD += ShowAlan2D;
        gameObject.SetActive(false);

    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            //to make sure rb doesn't go to sleep when character is staying still
            rb.WakeUp();
            if (Input.GetKeyDown(KeyCode.W))
            {
                
                // check if current scene is Level Demo
                if (SceneManager.GetActiveScene().name == "Level Demo Modified")
                {
                    // Load the next scene
                    SceneManager.LoadScene("MainMenu");
                }
                else if (SceneManager.GetActiveScene().name == "Level BouncePad")
                {
                    // Load the next scene
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    SceneManager.LoadScene("MainMenu");
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

        if (collision.gameObject.CompareTag("BouncePad"))
        {
            rb.velocity = new Vector2(rb.velocity.x, bouncePadForce);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
        }
    }
    
    private void ProjectAlanToMoveAlan2D()
    {
        //gets 2D Alan's direction
        int direction = transform.localScale.x > 0 ? 1 : -1;
        
        //Scales based on distance from InvisaWall
        float distanceToPlane = projectedWallTransform.position.z - Alan.position.z;
        float computedScaleFactor =  scaleFactor * initialDistanceFromWall*(1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane))); // Avoid division by zero
        Vector3 theScale = alanDefaultScale * computedScaleFactor;
        theScale.x *= direction;
        transform.localScale = theScale;
        
        //Moves to corresponding X position
        Vector3 newXPosition = transform.position;
        newXPosition.x = Alan.position.x;
        transform.position = newXPosition;
        
        //Moves to corresponding Y position
        Vector3 newYPosition = transform.position;
        newYPosition.y = Alan.position.y;
        transform.position = newYPosition;
        
        //parent the capsule to 2D Alan
    }
    

    private void ResetPosition()
    {
        transform.position = startingPosition;
    }
    
    private void HideAlan2D()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void ShowAlan2D()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }
}
