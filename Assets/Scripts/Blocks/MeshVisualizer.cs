using UnityEngine;

public class MeshVisualizer : MonoBehaviour
{
    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void OnDrawGizmos()
    {
        // Ensure we have a mesh to draw
        if (mesh == null) return;

        // Get the vertices and triangles from the mesh
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Set the Gizmo color for drawing
        Gizmos.color = Color.green;

        // Loop through each triangle in the mesh
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // Get the indices of the three vertices that form a triangle
            int index0 = triangles[i];
            int index1 = triangles[i + 1];
            int index2 = triangles[i + 2];

            // Get the actual positions of the vertices in world space (considering the object's transform)
            Vector3 vertex0 = transform.TransformPoint(vertices[index0]);
            Vector3 vertex1 = transform.TransformPoint(vertices[index1]);
            Vector3 vertex2 = transform.TransformPoint(vertices[index2]);

            // Draw lines between the three vertices of the triangle
            Gizmos.DrawLine(vertex0, vertex1);
            Gizmos.DrawLine(vertex1, vertex2);
            Gizmos.DrawLine(vertex2, vertex0);
        }
    }
}

