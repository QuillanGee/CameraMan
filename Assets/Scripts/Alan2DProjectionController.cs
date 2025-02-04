using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alan2DProjectionController : MonoBehaviour
{
    [SerializeField] GameObject alan;
    [SerializeField] private ObjectProjection objectProjection;
    
    private void Start()
    {
        EventManager.instance.OnToggleFirstPerson += ProjectAlan2D;
    }
    
    // Start is called before the first frame update
    
    public void ProjectAlan2D()
    {
        //Moves to corresponding X position
        Vector3 newPositionX = alan.transform.position;
        newPositionX.x = transform.position.x;
        alan.transform.position = newPositionX;
        
        //Moves to corresponding X position
        Vector3 newPositionY = alan.transform.position;
        newPositionY.y = transform.position.y;
        alan.transform.position = newPositionY;
        
    }
    
}
