using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;


public class ObjectProjection : MonoBehaviour
{
   //for current Mesh
   private MeshFilter meshFilter;
   private Mesh mesh;
   private Vector3[] vertices;
   private Bounds bounds;
   private Vector3 boundsCenter;
   private MeshRenderer meshRenderer;

   //for parent Mesh
   private MeshRenderer parentMeshRenderer;

   //for spawning new mesh
   public GameObject projectedMeshObject;
   private Mesh projectedMesh;
   private PolygonCollider2D polygonCollider;
   public Material projectedMaterial;
   public LayerMask groundLayer;
   
   public float minScale = 0.5f;  // Example minimum scale
   public float maxScale = 3f;  // Example maximum scale
   
   //for Door if they have one
   [SerializeField] private GameObject correspondingDoor;
   private Door door;

   private bool isHoldingBlock = false;
   private Transform myParent;
   private Vector3 relativePositionHold;
   private Quaternion relativeRotationHold;
   private Vector3 relativeScaleHold;

   private Rigidbody2D rb2D;
   private Pickable2DObject pickable2DObject; 

   void Awake()
   {
       // Get the Mesh Filter attached to this GameObject
       meshFilter = GetComponent<MeshFilter>();
       meshRenderer = GetComponent<MeshRenderer>();
       parentMeshRenderer = GetComponentInParent<MeshRenderer>();
   }


   void Start()
   {
       EventManager.instance.OnToggleTwoD += UpdatePerception;
       EventManager.instance.OnPostToggleFirstPerson += ToggleFirstPersonFunctions;
       EventManager.instance.OnHoldingBlock += SetHoldingBlockTrue;
       EventManager.instance.OnNotHoldingBlock += SetHoldingBlockFalse;
       // EventManager.instance.OnPostToggleFirstPerson += ShowObject;
       // EventManager.instance.OnPostToggleTwoD += HideObject;
       door = GetComponent<Door>();
       myParent = transform.parent;
   }

   private void OnDestroy()
   {
       EventManager.instance.OnToggleTwoD -= UpdatePerception;
       EventManager.instance.OnPostToggleFirstPerson -= DestroyProjectedMesh;
       EventManager.instance.OnHoldingBlock -= SetHoldingBlockTrue;
       EventManager.instance.OnNotHoldingBlock -= SetHoldingBlockFalse;
       // EventManager.instance.OnPostToggleFirstPerson -= ShowObject;
       // EventManager.instance.OnPostToggleTwoD -= HideObject;
   }


   private void GetMeshData()
   {
       if (meshFilter != null)
       {
           mesh = meshFilter.mesh;
           vertices = mesh.vertices;  
           bounds = mesh.bounds;
           boundsCenter = bounds.center;
           // Convert local verticies to world
           for (int i = 0; i < vertices.Length; i++)
           {
               vertices[i] = transform.TransformPoint(vertices[i]);
           }
       }
       else
       {
           Debug.LogError("No MeshFilter found on the GameObject!");
       }
   }
  
   private void UpdatePerception()
   {
       GetMeshData();

       // CALC PROJECTION
       Vector3[] projectedVerticies = ProjectVerticesTo2DAlgorithm(vertices);

       // Calculate center (used for moving it to origin and back)
       Vector3 centerOfProjection = AverageVector3Array(projectedVerticies);

       // MOVE CALCULATE PROJECTION TO ORIGIN (used for getting the gameObject anchor centered with the mesh)
       Vector3[] projectedVerticesAroundOrigin = TransformVerticesAroundOrigin(centerOfProjection, projectedVerticies);

       // Create GameObject, create Mesh around Origin
       projectedMeshObject = new GameObject("ProjectedMesh");
       projectedMeshObject.layer = 9;
       projectedMesh = Create2DMesh(projectedVerticesAroundOrigin, mesh.triangles);
       
       // For collider
       polygonCollider = projectedMeshObject.AddComponent<PolygonCollider2D>();
       pickable2DObject = projectedMeshObject.AddComponent<Pickable2DObject>();
       // rb2D = projectedMeshObject.AddComponent<Rigidbody2D>();
       CreateColliderFromTriangles(projectedMesh,polygonCollider);
       if (gameObject.CompareTag("whiteboard"))
       {
           projectedMeshObject.tag = "whiteboard";
       }
       
       if (gameObject.CompareTag("Stairs") || gameObject.CompareTag("Walls") || gameObject.CompareTag("Door"))
       {
           polygonCollider.isTrigger = true;
           
           if (gameObject.CompareTag("Stairs"))
           {
               projectedMeshObject.tag = "Stairs";
           }

           if (gameObject.CompareTag("Door"))
           {
               projectedMeshObject.layer = 0;
               projectedMeshObject.tag = "Door";
               projectedMeshObject.AddComponent<Door2D>().SetCorrepondingDoorTransform(correspondingDoor);
               projectedMeshObject.GetComponent<Door2D>().SetDoor(door);
           }
       }
       
       // projectedMeshObject.AddComponent<MeshFilter>().mesh = projectedMesh;
       // projectedMeshObject.AddComponent<MeshVisualizer>();
       // projectedMeshObject.AddComponent<MeshRenderer>().material = projectedMaterial;
       
       projectedMeshObject.transform.position = new Vector3(centerOfProjection.x, centerOfProjection.y, StaticZAxisFor2DLevel.currentZAxis.position.z);
       SetThisParentToProjectedMeshObject();
   }

   // private void HideObject()
   // {
   //     parentMeshRenderer.enabled = false;
   // }
   //
   // private void ShowObject()
   // {
   //     parentMeshRenderer.enabled = true;
   // }

   private void ToggleFirstPersonFunctions()
   {
       PutBackToParent();
       DestroyProjectedMesh();
   }

   private void DestroyProjectedMesh()
   {
       if (projectedMeshObject != null)
       {
           Destroy(projectedMeshObject);
       }
   }
  
   //gets the orthographic projection
   private Vector3[] ProjectVerticesTo2DAlgorithm(Vector3[] currVertices)
   {
       Vector3[] projectedVerticies = new Vector3[currVertices.Length];


       // Matrix for orthographic projection onto the XY-plane
       Matrix4x4 projectionMatrix = Matrix4x4.identity;
       projectionMatrix.m22 = 0f; // Set Z value to zero (flatten the Z axis)


       // Apply the matrix transformation to each vertex
       for (int i = 0; i < currVertices.Length; i++)
       {
           //for orthographic projection
           projectedVerticies[i] = projectionMatrix.MultiplyPoint3x4(currVertices[i]);
       }


       return projectedVerticies;
   }
  
  
   private Mesh Create2DMesh(Vector3[] vertices, int[] triangles)
   {
       Mesh newMesh = new Mesh();
       newMesh.vertices = vertices;
       newMesh.triangles = triangles;


       // Recalculate normals and bounds for the new mesh
       newMesh.RecalculateNormals();
       newMesh.RecalculateBounds();


       return newMesh;
   }

   private void CreateOwnPolygonCollider(Vector3[] projectedVertices, PolygonCollider2D polyCollider)
   {
       Vector2[] points = new Vector2[projectedMesh.vertexCount];
       for (int i = 0; i < projectedMesh.vertexCount; i++)
       {
           Vector3 vertex = projectedMesh.vertices[i];
           points[i] = new Vector2(vertex.x, vertex.y); // Assuming your mesh is flat on XY plane
       }

       polyCollider.pathCount = 1;
       polyCollider.SetPath(0, points);
   }
   
   private void CreateColliderFromTriangles(Mesh myMesh, PolygonCollider2D polyCollider)
   {
       Vector3[] mYvertices = myMesh.vertices;
       int[] triangles = myMesh.triangles;
    
       List<Vector2> outlinePoints = new List<Vector2>();

       // Loop through each triangle in the mesh
       for (int i = 0; i < triangles.Length; i += 3)
       {
           // Get the indices of the three vertices that form a triangle
           int index0 = triangles[i];
           int index1 = triangles[i + 1];
           int index2 = triangles[i + 2];

           // Add the triangle edges to the list
           AddEdgeToOutline(mYvertices[index0], mYvertices[index1], outlinePoints);
           AddEdgeToOutline(mYvertices[index1], mYvertices[index2], outlinePoints);
           AddEdgeToOutline(mYvertices[index2], mYvertices[index0], outlinePoints);
       }

       // Set the path for the collider (ensure it's a closed loop)
       polyCollider.pathCount = 1;
       polyCollider.SetPath(0, outlinePoints.ToArray());
   }

   private void AddEdgeToOutline(Vector3 start, Vector3 end, List<Vector2> outlinePoints)
   {
       // Check if the edge already exists in the outline
       // (This is simplified; in practice, you should compare both directions of the edge)
       if (!outlinePoints.Contains(new Vector2(start.x, start.y)) && !outlinePoints.Contains(new Vector2(end.x, end.y)))
       {
           outlinePoints.Add(new Vector2(start.x, start.y));
           outlinePoints.Add(new Vector2(end.x, end.y));
       }
   }
  
   private static Vector3 AverageVector3Array(Vector3[] vectors)
   {
       if (vectors == null || vectors.Length == 0)
       {
           return Vector3.zero; // Return zero vector if the input array is null or empty
       }


       Vector3 sum = Vector3.zero;


       // Sum all vectors
       for (int i = 0; i < vectors.Length; i++)
       {
           sum += vectors[i];
       }


       // Calculate the average
       Vector3 average = sum / vectors.Length;
       return average;
   }
  
   private Vector3[] TransformVerticesAroundOrigin(Vector3 distFromOrigin, Vector3[] vertices)
   {
       Vector3[] transformedVertices = new Vector3[vertices.Length];


       for (int i = 0; i < vertices.Length; i++)
       {
           transformedVertices[i] = vertices[i] - distFromOrigin;
       }


       return transformedVertices;
   }
   
   private void SetHoldingBlockTrue()
   {
       isHoldingBlock = true;
   }
   private void SetHoldingBlockFalse()
   {
       isHoldingBlock = false;
   }
   
   private void PutBackToParent()
   {
       // if (projectedMeshObject.CompareTag("whiteboard"))
       // {
       //     transform.position = new Vector3(transform.position.x, transform.position.y, myParent.position.z);
       // }
       // else
       // {
           // transform.position = myParent.transform.position;
           // transform.rotation = myParent.transform.rotation;
       // }
       // transform.rotation = myParent.rotation;
       transform.SetParent(myParent);
       transform.localPosition = relativePositionHold;
       transform.localRotation = relativeRotationHold;
       transform.localScale = relativeScaleHold;
   }

   private void SetThisParentToProjectedMeshObject()
   {
       relativePositionHold = transform.localPosition;
       relativeRotationHold = transform.localRotation;
       relativeScaleHold = transform.localScale;
       Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, StaticZAxisFor2DLevel.currentZAxis.position.z);
       transform.position = newPosition;
       transform.SetParent(projectedMeshObject.transform);
   }

   //USED IN ALAN

   public void AttachBlockToHoldPosition(Transform holdPosition)
   {
       pickable2DObject.Pickup(holdPosition);
   }
}
