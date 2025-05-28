using UnityEngine;

public class GetMeshDimensions : MonoBehaviour
{
    void Start()
    {
        // Get the MeshRenderer or MeshFilter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            // Get the mesh from the MeshFilter
            Mesh mesh = meshFilter.mesh;

            // Get the bounds of the mesh (the bounding box)
            Vector3 meshSize = mesh.bounds.size;

            // The size of the mesh in the x, y, z axes
            float width = meshSize.x;
            float height = meshSize.y;
            float depth = meshSize.z;

            // Output the dimensions to the console
            Debug.Log("Mesh Dimensions: Width = " + width + ", Height = " + height + ", Depth = " + depth);
        }
        else
        {
            Debug.LogError("No MeshFilter found on this GameObject!");
        }
    }
}