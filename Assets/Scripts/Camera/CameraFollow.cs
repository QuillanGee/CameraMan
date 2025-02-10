using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform alanTransform;
    [SerializeField] private Transform leftMax;
    [SerializeField] private Transform rightMax;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - alanTransform.position;
    }
    
    void Update()
    {
        StartCoroutine(DelayFollow());
    }

    // Update is called once per frame
    private IEnumerator DelayFollow()
    {
        float alanX = alanTransform.position.x;
        float offsetX = offset.x;
        float clampedX = Mathf.Clamp(offsetX + alanX , rightMax.position.x, leftMax.position.x);
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(clampedX,transform.position.y,transform.position.z);
    }
}
