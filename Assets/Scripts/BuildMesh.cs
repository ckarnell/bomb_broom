using UnityEngine;
using System.Collections;

public class BuildMesh : MonoBehaviour {
    // Values for building the triangle.
    // Sides of triangle are length 1, center is (0,0,0).
    float adj = 0.5F; // Length of half of one side.
    float opp = 0.288675134595F; // Distance from the center to the middle of a side.
    float hyp = 0.5773503F; // Distance from the center to a vertex.

    public float[] getCoords(float offset)
    {
        float[] lens = new float[3];
        float new_adj = adj - offset;
        lens[0] = new_adj;
        float new_opp = (opp * new_adj) / adj;
        lens[1] = new_opp;
        float new_hyp = (hyp * new_adj) / adj;
        lens[2] = new_hyp;
        return lens;
    }

    // Use this for initialization
    void Start() {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;
        
        float smaller_tri_offset = 0.1F;
        float[] smaller_verts = getCoords(smaller_tri_offset);
        float new_adj = smaller_verts[0];
        float new_opp = smaller_verts[1];
        float new_hyp = smaller_verts[2];
        Debug.Log(new_hyp);
        float z_offset = -0.05F;

        // Vertices
        Vector3[] vertices = new Vector3[]
        {
            // Back triangle
            new Vector3(0, hyp, 0), // Top back, 0
            new Vector3(adj, -opp, 0), // Bottom right back, 1
            new Vector3(-adj, -opp, 0), // Bottom left back, 2
            

            // Front triangle
            new Vector3(0, new_hyp, z_offset), // Top front, 3
            new Vector3(new_adj, -new_opp, z_offset), // Bottom right front, 4
            new Vector3(-new_adj, -new_opp, z_offset), // Bottom left front, 5

            // Top right connecting side
            new Vector3(0, hyp, 0), // 6
            new Vector3(adj, -opp, 0), // 7
            new Vector3(0, new_hyp, z_offset), // 8
            new Vector3(new_adj, -new_opp, z_offset), // 9

            // Bottom connecting side
            new Vector3(adj, -opp, 0), // 10
            new Vector3(-adj, -opp, 0), // 11
            new Vector3(new_adj, -new_opp, z_offset), // 12
            new Vector3(-new_adj, -new_opp, z_offset), // 13

            // Left connecting side
            new Vector3(-adj, -opp, 0), // 14
            new Vector3(0, hyp, 0), // 15
            new Vector3(-new_adj, -new_opp, z_offset), // 16
            new Vector3(0, new_hyp, z_offset) // 17
        };

        // Triangles // 3 points, clockwise, determines which side is visible
        int[] triangles = new int[]
        {
            // Bigger triangle behind
            0,2,1,

            // Smaller triangle front
            3,4,5,

            // Top right connecting side
            6,7,8, // First triangle
            8,7,9, // Second triangle

            // Bottom connecting side
            10,11,12, // First triangle
            12,11,13, // Second triangle

            // Upper left connecting side
            14,15,16, // First triangle
            16,15,17  // Second triangle
        };

        // UVs
        Vector2[] uvs = new Vector2[]
        {
            // Bigger back triangle
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),

            // Smaller front triangle
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),

            // Upper right connecting side
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            // Bottom connecting side
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            // Upper left connecting side
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0)
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.Optimize();
        mesh.RecalculateNormals();

        MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        meshc.sharedMesh = mesh;
	}

}
