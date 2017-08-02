using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(House))]
public class HouseEditor : Editor
{
    House house;

    void Awake()
    {
        this.house = target as House;
    }
      
    public void OnSceneGUI()
    {
        
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    
        EditorGUI.BeginChangeCheck();
        house.height_ = EditorGUILayout.Slider("height", house.height_, 1f, 10f);
        house.width_ = EditorGUILayout.Slider("width", house.width_, 1f, 10f);
        house.depth_ = EditorGUILayout.Slider("depth", house.depth_, 1f, 10f);

        house.wall_height_percentage_ = EditorGUILayout.Slider("WallHeight", house.wall_height_percentage_, 0f, 1f);

        if (EditorGUI.EndChangeCheck())
            house.CreateMesh();
    }

}
