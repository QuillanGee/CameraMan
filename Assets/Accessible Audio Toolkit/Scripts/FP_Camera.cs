using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FP_Camera : MonoBehaviour
{
    public Camera FPCamera;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float moveSpeed;


    public bool move = false;
    float h;
    float v;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        h = horizontalSpeed * Input.GetAxis("Mouse X");
        v = verticalSpeed * Input.GetAxis("Mouse Y");

        transform.Rotate(0, h, 0);
        FPCamera.transform.Rotate(-v, 0, 0);
        move = false;

        if (Input.GetKey(KeyCode.W)){
            move = true;
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.S)) {
            move = true;
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.A)){
            move = true;
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
        else if (Input.GetKey(KeyCode.D)){
            move = true;
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
    }
}
