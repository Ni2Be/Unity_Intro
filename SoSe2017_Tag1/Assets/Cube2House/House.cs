using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public class House : MonoBehaviour
{
    #region Parameter des einfachen Hauses
    public float height_                    = 3;
    public float width_                     = 3;
    public float depth_                     = 5;

    public float wall_height_percentage_    = 0.7f; //percentage of the height of the wall from the whole height
    #endregion // Parameter des einfachen Hauses

    // Die benötigten Positionen
    public Vector3 p0, p1, p2, p3, p4, p5, p6, p7, p8, p9;

    #region Mesh und Komponenten
    public Mesh         mesh_;
    public MeshFilter   mesh_filter_;
    public MeshRenderer mesh_renderer_;
    #endregion // Mesh und Komponenten

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void Reset()
    {
        CreateMesh();
    }
    public void OnDrawGizmos()
    {

        //Gizmos.color = Color.cyan;
        //Gizmos.DrawSphere(transform.TransformPoint(p0), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p1), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p2), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p3), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p4), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p5), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p6), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p7), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p8), 0.1f);
        //Gizmos.DrawSphere(transform.TransformPoint(p9), 0.1f);
    }

    public void CreateMesh()
    {
        mesh_filter_              = GetComponent<MeshFilter>();
        if (mesh_filter_ == null)
            mesh_filter_          = this.gameObject.AddComponent<MeshFilter>();

        mesh_renderer_ = GetComponent<MeshRenderer>();
        if (mesh_renderer_ == null)
            mesh_renderer_        = this.gameObject.AddComponent<MeshRenderer>();

        this.mesh_               = mesh_filter_.sharedMesh;

        if (this.mesh_ == null)
            this.mesh_           = new Mesh();

        // Positionen berechnen
        float wall_h = height_ * wall_height_percentage_;
        //front
        p0 = new Vector3( 0.0f           , 0.0f     , 0.0f);
        p1 = new Vector3( 0.0f           , wall_h   , 0.0f);
        p2 = new Vector3( width_         , 0.0f     , 0.0f);
        p3 = new Vector3( width_         , wall_h   , 0.0f);
        p4 = new Vector3( width_ / 2.0f  , height_  , 0.0f);
        //back
        p5 = new Vector3( width_         , 0.0f     , depth_);
        p6 = new Vector3( width_         , wall_h   , depth_);
        p7 = new Vector3( 0.0f           , 0.0f     , depth_);
        p8 = new Vector3( 0.0f           , wall_h   , depth_);
        p9 = new Vector3( width_ / 2.0f  , height_  , depth_);


        // Triangle Arrays for submeshes           
        Vector3[] vertices      = new Vector3[26];
        //front / back
        vertices[0]             = p0;
        vertices[1]             = p1;
        vertices[2]             = p2;
        vertices[3]             = p3;
        vertices[4]             = p4;

        vertices[5]             = p5;
        vertices[6]             = p6;
        vertices[7]             = p7;
        vertices[8]             = p8;
        vertices[9]             = p9;
        //side
        vertices[10]            = p0;
        vertices[11]            = p1;
        vertices[12]            = p8;
        vertices[13]            = p7;
        vertices[14]            = p2;
        vertices[15]            = p3;
        vertices[16]            = p6;
        vertices[17]            = p5;
        //roof
        vertices[18]            = p1;
        vertices[19]            = p4;
        vertices[20]            = p9;
        vertices[21]            = p8;
        vertices[22]            = p3;
        vertices[23]            = p4;           
        vertices[24]            = p9;
        vertices[25]            = p6;



        Vector2[] uvs = new Vector2[vertices.Length];

        //front
        float front_top         = height_;
        float front_wall_top    = height_ * wall_height_percentage_;
        float front_middle      = width_ / 2;
        float front_right       = width_;

        float uv_front_top      = front_top;
        float uv_front_wall_top = front_wall_top;
        float uv_front_middle   = front_middle;
        float uv_front_right    = front_right;

        uvs[0]             = new Vector2( 0.0f              , 0.0f              );
        uvs[1]             = new Vector2( 0.0f              , uv_front_wall_top );
        uvs[2]             = new Vector2( uv_front_right    , 0.0f              );
        uvs[3]             = new Vector2( uv_front_right    , uv_front_wall_top );
        uvs[4]             = new Vector2( uv_front_middle   , uv_front_top      );
        
        uvs[5]             = new Vector2(0.0f              , 0.0f              );
        uvs[6]             = new Vector2(0.0f              , uv_front_wall_top );
        uvs[7]             = new Vector2(uv_front_right    , 0.0f              );
        uvs[8]             = new Vector2(uv_front_right    , uv_front_wall_top );
        uvs[9]             = new Vector2(uv_front_middle   , uv_front_top      );

        //side
        float side_height       = height_ * wall_height_percentage_;
        float side_width        = width_;

        float uv_side_top       = side_height;
        float uv_side_right     = side_width;

        uvs[10]            = new Vector2( 0.0f              , 0.0f              );
        uvs[11]            = new Vector2( 0.0f              , uv_side_top       );
        uvs[12]            = new Vector2( uv_side_right     , uv_side_top       );
        uvs[13]            = new Vector2( uv_side_right     , 0.0f              );
        uvs[14]            = new Vector2( 0.0f              , 0.0f              );
        uvs[15]            = new Vector2( 0.0f              , uv_side_top       );
        uvs[16]            = new Vector2( uv_side_right     , uv_side_top       );
        uvs[17]            = new Vector2( uv_side_right     , 0.0f              );
        
        //roof
        double roof_height      = Math.Sqrt(Math.Pow(width_ / 2, 2) + Math.Pow(height_ - (wall_height_percentage_ * height_), 2));
        double roof_width       = depth_;

        float uv_roof_top       = (float)roof_height;
        float uv_roof_right     = (float)roof_width;

        uvs[18]            = new Vector2( 0.0f              , 0.0f          );
        uvs[19]            = new Vector2( 0.0f              , uv_roof_top   );
        uvs[20]            = new Vector2( uv_roof_right     , uv_roof_top   );
        uvs[21]            = new Vector2( uv_roof_right     , 0.0f          );
        uvs[22]            = new Vector2( 0.0f              , 0.0f          );
        uvs[23]            = new Vector2( 0.0f              , uv_roof_top   );           
        uvs[24]            = new Vector2( uv_roof_right     , uv_roof_top   );
        uvs[25]            = new Vector2( uv_roof_right     , 0.0f          );




        // Arrays für die Submeshes
        //front
        int[] fronts            = new int[18];
        fronts[0]               = 0;
        fronts[1]               = 1;
        fronts[2]               = 2;
        fronts[3]               = 2;
        fronts[4]               = 1;
        fronts[5]               = 3;
        fronts[6]               = 1;
        fronts[7]               = 4;
        fronts[8]               = 3;

        fronts[9]               = 6;
        fronts[10]              = 7;
        fronts[11]              = 5;
        fronts[12]              = 6;
        fronts[13]              = 8;
        fronts[14]              = 7;
        fronts[15]              = 6;
        fronts[16]              = 9;
        fronts[17]              = 8;


        //side
        int[] sides             = new int[12];
        sides[0]               = 10;
        sides[1]               = 12;
        sides[2]               = 11;
        sides[3]               = 10;
        sides[4]               = 13;
        sides[5]               = 12;

        sides[6]               = 14;
        sides[7]               = 15;
        sides[8]               = 17;
        sides[9]               = 15;
        sides[10]              = 16;
        sides[11]              = 17;
        

        //roof
        int[] roof              = new int[12];
        roof[0]               = 18;
        roof[1]               = 20;
        roof[2]               = 19;
        roof[3]               = 18;
        roof[4]               = 21;
        roof[5]               = 20;
        
        roof[6]               = 22;
        roof[7]               = 23;
        roof[8]               = 25;
        roof[9]               = 23;
        roof[10]              = 24;
        roof[11]              = 25;
        

        
        mesh_.Clear();
        mesh_.vertices           = vertices;

        // Submesh-Triangles zuweisen
        mesh_.subMeshCount = 3;
        mesh_.SetTriangles(sides , 0);
        mesh_.SetTriangles(fronts, 1);
        mesh_.SetTriangles(roof  , 2);

        mesh_.uv = uvs;

        mesh_.RecalculateNormals();
        mesh_filter_.mesh = mesh_;
       
        //Material
        Material material_brick         = new Material(Shader.Find("Diffuse"));
        Material material_brickWhite    = new Material(Shader.Find("Diffuse"));
        Material material_roof          = new Material(Shader.Find("Diffuse"));
        Texture texture_1               = Resources.Load("brick") as Texture2D;
        Texture texture_2               = Resources.Load("brickWhite") as Texture2D;
        Texture texture_3               = Resources.Load("roof") as Texture2D;

        material_brick.mainTexture      = texture_1;
        material_brickWhite.mainTexture = texture_2;
        material_roof.mainTexture       = texture_3;
        
        Material[] materials            = new Material[3];
        materials[0]                    = material_brick;
        materials[1]                    = material_brickWhite;
        materials[2]                    = material_roof;

        mesh_renderer_.materials        = materials;

    }

#if UNITY_EDITOR
    [MenuItem("GameObject/Primitives/SimpleHouse", false, 50)]
    public static House CreateHouse()
    {
        GameObject gO = new GameObject("SimpleHouse");
        House house = gO.AddComponent<House>();

        house.CreateMesh();

        return house;
    }
#endif
}