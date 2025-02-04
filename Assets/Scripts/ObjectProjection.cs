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

    //for spawning new mesh
    public GameObject projectedMeshObject;
    private Mesh projectedMesh;
    private PolygonCollider2D polygonCollider;
    public Material projectedMaterial;
    public LayerMask groundLayer;
    
    //What z to project onto
    [SerializeField] Transform projectedWallTransform;

    void Awake()
    {
        // Get the Mesh Filter attached to this GameObject
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        EventManager.instance.OnToggleTwoD += UpdatePerception;
        EventManager.instance.OnToggleFirstPerson += DestroyProjectedMesh;
        EventManager.instance.OnInstantiateGamePlay += UpdatePerception;
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
                // Debug.Log("Vertex " + i + " world position: " + vertices[i]);
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
        
        //calculate center (used for moving it to origin and back)
        Vector3 centerOfProjection = AverageVector3Array(projectedVerticies);
        
        //MOVE CALCULATE PROJECTION TO ORIGIN (used for getting the gameObject anchor centered with the mesh)
        Vector3[] projectedVerticesAroundOrigin = TransformVerticesAroundOrigin(centerOfProjection, projectedVerticies);
        
        // Check if currMesh is null, if not destory currMesh (gameobject)
        DestroyProjectedMesh();
        
        //Create GameObject, create Mesh around Origin
        projectedMeshObject = new GameObject("ProjectedMesh");
        projectedMeshObject.layer = 9;
        projectedMesh = Create2DMesh(projectedVerticesAroundOrigin, mesh.triangles);
        
        //for collider
        polygonCollider = projectedMeshObject.AddComponent<PolygonCollider2D>();
        AddPolygonColliderFromProjectedVertices(projectedVerticesAroundOrigin,polygonCollider);
        projectedMeshObject.AddComponent<MeshFilter>().mesh = projectedMesh;
        projectedMeshObject.AddComponent<MeshRenderer>().material = projectedMaterial;
        
        //Calc Distance from Center of 3D mesh, Scale, transform GameObject back to original position
        float distanceToPlane = projectedWallTransform.position.z - transform.position.z;
        float scaleFactor =   2*(1.0f / Mathf.Max(1e-5f, Mathf.Abs(distanceToPlane))); // Avoid division by zero

        //Scale
        projectedMeshObject.transform.localScale *= scaleFactor;
        projectedMeshObject.transform.position = centerOfProjection;
        
        
    }

    private void DestroyProjectedMesh()
    {
        if (projectedMeshObject != null)
        {
            Destroy(projectedMeshObject);
        }
    }
    
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
    
    private void AddPolygonColliderFromProjectedVertices(Vector3[] projectedVertices, PolygonCollider2D polyCollider)
    {
        // Convert to Vector2 array first
        Vector2[] points2D = new Vector2[projectedVertices.Length];
        for (int i = 0; i < projectedVertices.Length; i++)
        {
            points2D[i] = new Vector2(projectedVertices[i].x, projectedVertices[i].y);
        }

        // Get the convex hull of the points
        Vector2[] convexHull = ComputeConvexHull(points2D);

        // Set the convex hull points as the collider path
        polyCollider.SetPath(0, convexHull);
    }

    // Graham Scan algorithm to compute convex hull
    private Vector2[] ComputeConvexHull(Vector2[] points)
    {
        if (points.Length < 3) return points;

        // Find point with lowest y-coordinate (and leftmost if tied)
        int lowestPoint = 0;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].y < points[lowestPoint].y ||
                (points[i].y == points[lowestPoint].y && points[i].x < points[lowestPoint].x))
            {
                lowestPoint = i;
            }
        }

        // Swap the lowest point to be first in array
        Vector2 temp = points[0];
        points[0] = points[lowestPoint];
        points[lowestPoint] = temp;

        // Sort points by polar angle with respect to base point
        Vector2 basePoint = points[0];
        System.Array.Sort(points, 1, points.Length - 1, new PolarAngleComparer(basePoint));

        // Build convex hull
        List<Vector2> hull = new List<Vector2>();
        hull.Add(points[0]);
        hull.Add(points[1]);

        for (int i = 2; i < points.Length; i++)
        {
            while (hull.Count >= 2 && !IsLeftTurn(hull[hull.Count - 2], hull[hull.Count - 1], points[i]))
            {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(points[i]);
        }

        return hull.ToArray();
    }

private class PolarAngleComparer : IComparer<Vector2>
{
    private Vector2 basePoint;

    public PolarAngleComparer(Vector2 basePoint)
    {
        this.basePoint = basePoint;
    }

    public int Compare(Vector2 a, Vector2 b)
    {
        float angleA = Mathf.Atan2(a.y - basePoint.y, a.x - basePoint.x);
        float angleB = Mathf.Atan2(b.y - basePoint.y, b.x - basePoint.x);
        
        if (angleA < angleB) return -1;
        if (angleA > angleB) return 1;
        
        // If angles are equal, put closer point first
        float distA = Vector2.SqrMagnitude(a - basePoint);
        float distB = Vector2.SqrMagnitude(b - basePoint);
        return distA.CompareTo(distB);
    }
}

    private bool IsLeftTurn(Vector2 a, Vector2 b, Vector2 c)
    {
        return ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0;
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
    

    public void PositionBlockToHoldPosition(Vector3 holdPosition)
    {
        projectedMeshObject.transform.position = holdPosition;
    }

    public void SetBlockParent(Transform parent)
    {
        projectedMeshObject.transform.SetParent(parent);
    }
}
