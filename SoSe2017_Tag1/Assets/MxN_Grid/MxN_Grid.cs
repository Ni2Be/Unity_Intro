
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MxN_Grid : MonoBehaviour
{
    public float        height_        = 10.0f;
    public float        width_         = 10.0f;
    
    public int          width_points_count_  = 20;
    public int          height_points_count_ = 20;

    #region Mesh-Stuff
    Mesh            mesh_;
    MeshFilter      mesh_filter_;
    MeshRenderer    mesh_renderer_;
    #endregion


    #region unity methods

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion


    public void CreateMesh()
    {
        mesh_filter_        = GetComponent<MeshFilter>();
        mesh_renderer_      = GetComponent<MeshRenderer>();
        mesh_               = mesh_filter_.sharedMesh;

        if (mesh_ == null)
        {
            mesh_           = new Mesh();
            mesh_.name      = "MxN Grid";
        }

        List<Vector3>   vertices    = new List<Vector3>();
        List<Vector2>   uvs         = new List<Vector2>();

        //vertices / uvs
        float widht_step            = width_  / (width_points_count_  - 1);
        float height_step           = height_ / (height_points_count_ - 1);
        float widht_parameter       = 1.0f    / (width_points_count_  - 1);
        float height_parameter      = 1.0f    / (height_points_count_ - 1);

        for (int w = 0; w < width_points_count_; w ++)
        {
            for (int h = 0; h < height_points_count_; h ++)
            {
                vertices.Add(new Vector3(w * widht_step     , 0.0f                , h * height_step));
                uvs     .Add(new Vector2(w * widht_parameter, h * height_parameter));

            }
        }

        //triangles
        List<int> triangles = new List<int>();

        /* p01__________p11
         * |            |
         * |            |
         * |____________|
         * p00          p10
        */


        for (int w = 0; w < width_points_count_ - 1; w++)
        {
            for (int h = 0; h < height_points_count_ - 1; h++)
            {
                int p00 = h        * height_points_count_ + w    ;
                int p01 = h        * height_points_count_ + w + 1;
                int p10 = (h + 1)  * height_points_count_ + w    ;
                int p11 = (h + 1)  * height_points_count_ + w + 1;

                triangles.Add(p00);
                triangles.Add(p01);
                triangles.Add(p10);

                triangles.Add(p01);
                triangles.Add(p11);
                triangles.Add(p10);
            }
        }

        
        mesh_.Clear();

        mesh_.SetVertices       (vertices);
        mesh_.triangles         = triangles.ToArray();
        mesh_.uv                = uvs.ToArray();

        mesh_.RecalculateNormals();

        mesh_filter_.sharedMesh = mesh_;
    }




    public static Vector3 point_on_bilinear_surface(Vector3 p00, Vector3 p01, Vector3 p10, Vector3 p11, float u, float w)
    {
        Vector3 p0 = point_on_line(p00, p10, u);
        Vector3 p1 = point_on_line(p01, p11, u);
        return point_on_line(p0, p1, w);
    }

    public static Vector3 point_on_line(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }


}
