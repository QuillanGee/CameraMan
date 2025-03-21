using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;  // Need this for scene management

public class Alan2D : MonoBehaviour
{
    private Transform projectedWallTransform;
    [SerializeField] Transform Alan;
    [SerializeField] private GameObject AlanMesh;
    private Vector3 alanDefaultScale;
    [SerializeField] Transform resetPosition;
    private float scaleFactor = 4f;
    [SerializeField] private Transform zAxisFor2Dlevel;
    float minScale = 0.5f;  // Example minimum scale
    float maxScale = 1.5f;  // Example maximum scale
    
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
        EventManager.instance.OnToggleTwoD += ProjectAlanToMoveAlan2D;
        EventManager.instance.OnResetAlan2D += ResetPosition;
        
        EventManager.instance.OnPostToggleFirstPerson += HideAlan2D;
        EventManager.instance.OnPostToggleTwoD += ShowAlan2D;

        EventManager.instance.OnLoadScene += SetZAxisFor2D;
        EventManager.instance.OnLoadScene += AttachResetPosition;
        EventManager.instance.OnResetAlan2D += ResetPosition;
    }

    private void SetZAxisFor2D() 
    {
        zAxisFor2Dlevel = GameObject.FindWithTag("zAxisFor2DLevel").transform;
        if (zAxisFor2Dlevel != null)
        {
            print("Couldn't find zAxis");
        }
    }
    
    private void AttachResetPosition()
    {
        resetPosition = GameObject.FindWithTag("ResetPosition2D").transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Platform"))
        // {
        //     transform.SetParent(collision.transform);
        // }

        if (collision.gameObject.CompareTag("BouncePad"))
        {
            rb.velocity = new Vector2(rb.velocity.x, bouncePadForce);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Platform"))
        // {
        //     transform.SetParent(null);
        // }
    }
    
    private void ProjectAlanToMoveAlan2D()
    {
        ScaleAlan();
        //Moves to corresponding X position
        Vector3 newPosition = new Vector3(Alan.position.x, Alan.position.y, zAxisFor2Dlevel.position.z);
        transform.position = newPosition;
    }
    
    private void ScaleAlan()
    {
        float distanceToPlane = StaticProjectedWallTransform.ProjectedWallTransform.position.z - Alan.transform.position.z;
        float computedScaleFactor = scaleFactor * (1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane)));
        int direction = transform.localScale.x > 0 ? 1 : -1;
        Vector3 theScale = alanDefaultScale * computedScaleFactor;
        theScale.x = Mathf.Clamp(theScale.x, minScale, maxScale);
        theScale.y = Mathf.Clamp(theScale.y, minScale, maxScale);
        theScale.z = Mathf.Clamp(theScale.z, minScale, maxScale);
        theScale.x *= direction;
        transform.localScale = theScale;
    }

    private void ResetPosition()
    {
        transform.position = resetPosition.position;
    }
    
    private void HideAlan2D()
    {
        if (AlanMesh.activeInHierarchy)
        {
            AlanMesh.SetActive(false);
        }
    }

    private void ShowAlan2D()
    {
        if (!AlanMesh.activeInHierarchy)
        {
            AlanMesh.SetActive(true);
        }
    }
}
