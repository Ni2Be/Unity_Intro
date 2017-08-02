using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(MxN_Grid))]
public class MxN_Grid_Editor : Editor
{
    MxN_Grid        mxn_grid_;

    // Use this for initialization
    void Awake()
    {
        mxn_grid_ = target as MxN_Grid;
    }
    
    // Update is called once per frame
    void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        mxn_grid_.width_  = EditorGUILayout.Slider("Width" , mxn_grid_.width_ , 1.0f, 100.0f);
        mxn_grid_.height_ = EditorGUILayout.Slider("Height", mxn_grid_.height_, 1.0f, 100.0f);

        mxn_grid_.width_points_count_  = EditorGUILayout.IntSlider("Number of Points Width" , mxn_grid_.width_points_count_ , 2, 100);
        mxn_grid_.height_points_count_ = EditorGUILayout.IntSlider("Number of Points Height", mxn_grid_.height_points_count_, 2, 100);

        if (EditorGUI.EndChangeCheck())
            mxn_grid_.CreateMesh();
    }
}
