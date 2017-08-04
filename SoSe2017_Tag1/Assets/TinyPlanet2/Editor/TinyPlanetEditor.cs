using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TinyPlanet))]
public class TinyPlanetEditor : Editor
{
    TinyPlanet tinyPlanet;

    void Awake()
    {
        tinyPlanet = target as TinyPlanet;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        #region Parameters
        tinyPlanet.Width            = EditorGUILayout.Slider("Width", tinyPlanet.Width, 1f, 100f);
        tinyPlanet.Height           = EditorGUILayout.Slider("Height", tinyPlanet.Height, 1f, 100f);

        tinyPlanet.pointsWidth      = EditorGUILayout.IntSlider("pointsWidth", tinyPlanet.pointsWidth, 2, 100);
        tinyPlanet.pointsHeight     = EditorGUILayout.IntSlider("pointsWidth", tinyPlanet.pointsHeight, 2, 100);

        tinyPlanet.foldoutSphereT   = EditorGUILayout.Slider("Foldout Sphere t", tinyPlanet.foldoutSphereT, 0f, 1f);

        tinyPlanet.amplitude        = EditorGUILayout.Slider("Amplitude", tinyPlanet.amplitude, 0f, 4f);

        #endregion // Parameters

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Heightmap");
        tinyPlanet.heightmap = 
            EditorGUILayout.ObjectField(
                "Heightmap", 
                tinyPlanet.heightmap, 
                typeof(Texture2D),
                true) as Texture2D;

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Maintexture");
        tinyPlanet.mainTexture =
            EditorGUILayout.ObjectField(
                "MainTexture",
                tinyPlanet.mainTexture,
                typeof(Texture2D),
                true) as Texture2D;

        EditorGUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
            tinyPlanet.CreateMesh();
    }
}