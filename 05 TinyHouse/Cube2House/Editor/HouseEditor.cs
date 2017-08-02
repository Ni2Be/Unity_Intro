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
        house.Height = EditorGUILayout.Slider("height", house.Height, 1f, 10f);
        house.Width = EditorGUILayout.Slider("width", house.Width, 1f, 10f);
        house.Depth = EditorGUILayout.Slider("depth", house.Depth, 1f, 10f);

        house.wallHeightP = EditorGUILayout.Slider("WallHeight", house.wallHeightP, 0f, 1f);

        if (EditorGUI.EndChangeCheck())
            house.CreateMesh();
    }

}
