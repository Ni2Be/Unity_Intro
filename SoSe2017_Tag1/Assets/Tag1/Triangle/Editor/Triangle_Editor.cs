using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Triangle))]
public class Triangle_Editor : Editor
{
    Triangle            triangle_;

    void Awake()
    {
        triangle_       = target as Triangle;
    }

    public void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck();
        triangle_.p0_   = Handles.FreeMoveHandle(triangle_.p0_, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereCap);
        triangle_.p1_   = Handles.FreeMoveHandle(triangle_.p1_, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereCap);
        triangle_.p2_   = Handles.FreeMoveHandle(triangle_.p2_, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereCap);
        triangle_.p3_   = Handles.FreeMoveHandle(triangle_.p3_, Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereCap);

        if (EditorGUI.EndChangeCheck())
            triangle_.CreateMesh();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
