using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Mesh_Debugger))]
public class Mesh_Debugger_Editor : Editor
{
    Mesh_Debugger       mesh_debugger_;
    MeshFilter          mesh_filter_;
    Mesh                mesh_;

    #region Info-Toggles
    public static bool  move_vertecies_;

    public static bool  show_vertex_positions_;
    public static bool  show_vertex_indices_;
    public static bool  show_vertex_uvs_;
    public static bool  show_vertex_normals_;

    public static bool  show_triangle_numbers_;
    public static bool  show_triangle_indices_;
    public static bool  show_triangle_normals_;
    #endregion


    // Use this for initialization
    void Awake()
    {
        mesh_debugger_  = target as Mesh_Debugger;
        mesh_filter_    = mesh_debugger_.gameObject.GetComponent<MeshFilter>();
        mesh_           = mesh_filter_.sharedMesh;
    }

    // Update is called once per frame
    void OnSceneGUI()
    {
        if (mesh_ == null)
            return;

        if (move_vertecies_)
            move_vertecies();

        //Vertecies
        if (show_vertex_positions_)
            show_vertex_positions();
        if (show_vertex_indices_)
            show_vertex_indices();
        if (show_vertex_uvs_)
            show_vertex_uvs();
        if (show_vertex_normals_)
            show_vertex_normals();

        //Triangles
        if (show_triangle_numbers_)
            show_triangle_numbers();
        if (show_triangle_indices_)
            show_triangle_indices();
        if (show_triangle_normals_)
            show_triangle_normals();
    }


    private void move_vertecies()
    {
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < mesh_.vertexCount; i++)
        {
            mesh_.vertices[i] = Handles.FreeMoveHandle(mesh_.vertices[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereCap);
        }


        if (EditorGUI.EndChangeCheck())
            return;
    }

    private void show_vertex_positions()
    {
        for(int i = 0; i < mesh_.vertexCount; i++)
        {
            Handles.Label(  mesh_debugger_.transform.TransformPoint(mesh_.vertices[i]), 
                            mesh_.vertices[i].ToString());
        }
    }
    private void show_vertex_indices()
    {
        for (int i = 0; i < mesh_.vertexCount; i++)
        {
            Handles.Label(mesh_debugger_.transform.TransformPoint(mesh_.vertices[i]),
                            i.ToString());
        }
    }
    private void show_vertex_uvs()
    {
        for (int i = 0; i < mesh_.vertexCount; i++)
        {
            Handles.Label(mesh_debugger_.transform.TransformPoint(mesh_.vertices[i]),
                          mesh_.uv[i].ToString());
        }
    }
    private void show_vertex_normals()
    {
        for (int i = 0; i < mesh_.vertexCount; i++)
        {
            Handles.Label(mesh_debugger_.transform.TransformPoint(mesh_.vertices[i]),
                          mesh_.normals[i].ToString());

            Vector3 vertex_position = mesh_debugger_.transform.TransformPoint(mesh_.vertices[i]);

            Handles.color = new Color(mesh_.normals[i].x + 0.5f, mesh_.normals[i].y + 0.5f, mesh_.normals[i].z + 0.5f, 1);
            Handles.DrawLine( vertex_position, 
                              mesh_debugger_.transform.TransformPoint(mesh_.vertices[i] + mesh_.normals[i]));
        }
    }

    private void show_triangle_numbers()
    {
        for (int i = 0; i < mesh_.triangles.Length; i += 3)
        {
            Vector3 p0 = mesh_.vertices[mesh_.triangles[i]];
            Vector3 p1 = mesh_.vertices[mesh_.triangles[i + 1]];
            Vector3 p2 = mesh_.vertices[mesh_.triangles[i + 2]];

            Vector3 centroid = (p0 + p1 + p2) / 3.0f;

            Handles.Label(mesh_debugger_.transform.TransformPoint(centroid), (i / 3).ToString());


            //Handles.color = Color.green;
            //Handles.DrawSphere(mesh_debugger_.transform.TransformPoint(centroid), 0.1f);
        }
    }

    private void show_triangle_indices()
    {
        for (int i = 0; i < mesh_.triangles.Length; i += 3)
        {
            Vector3 p0 = mesh_.vertices[mesh_.triangles[i]];
            Vector3 p1 = mesh_.vertices[mesh_.triangles[i + 1]];
            Vector3 p2 = mesh_.vertices[mesh_.triangles[i + 2]];

            Vector3 centroid = (p0 + p1 + p2) / 3.0f;

            Vector3 p0_label = centroid + (p0 - centroid) * 0.75f;
            Vector3 p1_label = centroid + (p1 - centroid) * 0.75f;
            Vector3 p2_label = centroid + (p2 - centroid) * 0.75f;

            Handles.Label(mesh_debugger_.transform.TransformPoint(p0_label), mesh_.triangles[i]    .ToString());
            Handles.Label(mesh_debugger_.transform.TransformPoint(p1_label), mesh_.triangles[i + 1].ToString());
            Handles.Label(mesh_debugger_.transform.TransformPoint(p2_label), mesh_.triangles[i + 2].ToString());
        }
    }

    private void show_triangle_normals()
    {
        for (int i = 0; i < mesh_.triangles.Length; i += 3)
        {
            Vector3 p0 = mesh_.vertices[mesh_.triangles[i]];
            Vector3 p1 = mesh_.vertices[mesh_.triangles[i + 1]];
            Vector3 p2 = mesh_.vertices[mesh_.triangles[i + 2]];

            Vector3 centroid    = mesh_debugger_.transform.TransformPoint((p0 + p1 + p2) / 3.0f);
            Vector3 normal      = mesh_debugger_.transform.TransformDirection(calculate_normal(p0, p1, p2));

            Handles.color = new Color(
                                        normal.x + 0.5f, 
                                        normal.y + 0.5f, 
                                        normal.z + 0.5f, 
                                        1);
            Handles.Label(centroid, normal.ToString());
            Handles.DrawLine(centroid, centroid + normal.normalized);
        }
    }

    private Vector3 calculate_normal(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Vector3.Cross((p1 - p0), (p2 - p0));
    }

    public override void OnInspectorGUI()
    {
        move_vertecies_ = EditorGUILayout.ToggleLeft("Move Vertecies", move_vertecies_);

        show_vertex_positions_  = EditorGUILayout.ToggleLeft("Show Vertex Positions", show_vertex_positions_);
        show_vertex_indices_    = EditorGUILayout.ToggleLeft("Show Vertex Indices", show_vertex_indices_);
        show_vertex_uvs_        = EditorGUILayout.ToggleLeft("Show Vertex UVs", show_vertex_uvs_);
        show_vertex_normals_    = EditorGUILayout.ToggleLeft("Show Vertex Noramls", show_vertex_normals_);

        show_triangle_numbers_  = EditorGUILayout.ToggleLeft("Show Triangle Numbers", show_triangle_numbers_);
        show_triangle_indices_  = EditorGUILayout.ToggleLeft("Show Triangle Indices", show_triangle_indices_);
        show_triangle_normals_  = EditorGUILayout.ToggleLeft("Show Triangle Normals", show_triangle_normals_);
    }
}
