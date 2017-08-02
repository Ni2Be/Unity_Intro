using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Line_Coords))]
public class Line_Coords_Editor : Editor
{
    Line_Coords line_coords_;

    // Use this for initialization
    void Awake()
    {
        line_coords_ = target as Line_Coords;
    }

    // Update is called once per frame
    void OnSceneGUI()
    {
        line_coords_.p0 = Handles.FreeMoveHandle(
                                            line_coords_.p0,
                                            Quaternion.identity,
                                            0.1f,
                                            Vector3.zero,
                                            Handles.SphereCap);

        line_coords_.p1 = Handles.FreeMoveHandle(
                                            line_coords_.p1,
                                            Quaternion.identity,
                                            0.1f,
                                            Vector3.zero,
                                            Handles.SphereCap);
    }
}
