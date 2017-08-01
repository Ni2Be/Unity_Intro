using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public class Triangle : MonoBehaviour
{
    public Vector3  p0_       = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3  p1_       = new Vector3(0.0f, 0.0f, 1.0f);
    public Vector3  p2_       = new Vector3(1.0f, 0.0f, 0.0f);
    public Vector3  p3_       = new Vector3(1.0f, 0.0f, 1.0f);

    MeshFilter      mesh_filter_;
    MeshRenderer    mesh_renderer_;
    Mesh            mesh_;

    Material        material_;
    Texture         texture_;

    float           time_counter_;

    public void OnDrawGizmos()
    {
        //Dots
        Gizmos.color        = Color.green;
        Gizmos.DrawSphere   (transform.TransformPoint(p0_), 0.1f);
        Gizmos.color        = Color.blue;
        Gizmos.DrawSphere   (transform.TransformPoint(p1_), 0.1f);
        Gizmos.color        = Color.red;
        Gizmos.DrawSphere   (transform.TransformPoint(p2_), 0.1f);
        Gizmos.color        = Color.cyan;
        Gizmos.DrawSphere   (transform.TransformPoint(p3_), 0.1f);


        //Lines
        Gizmos.color        = Color.cyan;
        Gizmos.DrawLine     (transform.TransformPoint(p0_), transform.TransformPoint(p1_));
        Gizmos.DrawLine     (transform.TransformPoint(p1_), transform.TransformPoint(p2_));
        Gizmos.DrawLine     (transform.TransformPoint(p2_), transform.TransformPoint(p3_));
        Gizmos.DrawLine     (transform.TransformPoint(p2_), transform.TransformPoint(p0_));
        Gizmos.DrawLine     (transform.TransformPoint(p1_), transform.TransformPoint(p3_));
    }


    public void Start()
    {
        CreateMesh();
    }

    public void Reset()
    {
        CreateMesh();
    }


    public void CreateMesh()
    {
        //MeshFilter init
        mesh_filter_            = GetComponent<MeshFilter>();
        if (mesh_filter_ == null)
            mesh_filter_        = gameObject.AddComponent<MeshFilter>();

        //MeshRenderer init
        mesh_renderer_          = GetComponent<MeshRenderer>();
        if (mesh_renderer_ == null)
            mesh_renderer_      = gameObject.AddComponent<MeshRenderer>();

        //Mesh init
        mesh_                   = new Mesh();
        Vector3[]     vertecies = new Vector3[4];
        vertecies[0]            = p0_;
        vertecies[1]            = p1_;
        vertecies[2]            = p2_;
        vertecies[3]            = p3_;
        
        int[]        vert_index = new int[6];
        vert_index[0]           = 0;
        vert_index[1]           = 1;
        vert_index[2]           = 2;
        
        vert_index[3]           = 3;
        vert_index[4]           = 2;
        vert_index[5]           = 1;

        //Texture points
        Vector2[]           uvs = new Vector2[4];
        uvs[0]                  = new Vector2(0.0f, 0.0f);
        uvs[1]                  = new Vector2(0.0f, 1.0f);
        uvs[2]                  = new Vector2(1.0f, 0.0f);
        uvs[3]                  = new Vector2(1.0f, 1.0f);
        
        mesh_.vertices          = vertecies;
        mesh_.uv                = uvs;
        mesh_.triangles         = vert_index;

        //Material
        material_                       = new Material(Shader.Find("Standard"));
        texture_                        = Resources.Load("chess") as Texture2D;
        material_.mainTexture           = texture_;

        mesh_renderer_.sharedMaterial   = material_;

        mesh_filter_.sharedMesh         = mesh_;
    }


    void Update()
    {
        time_counter_ += Time.deltaTime;
    }

#if UNITY_EDITOR
    [MenuItem("GameObject/3D Object/Trinangle")]
    public static void CreateTrinangle()
    {
        GameObject go               = new GameObject("Triangle");
        go.AddComponent<Triangle>();

    }
#endif
}
