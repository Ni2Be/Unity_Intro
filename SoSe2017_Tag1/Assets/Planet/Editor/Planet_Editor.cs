using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Planet))]
public class Planet_Editor : Editor
{
    Planet planet_;


    void Awake()
    {
        planet_ = target as Planet;
    }


    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        planet_.width_ = EditorGUILayout.Slider("Width", planet_.width_, 1.0f, 100.0f);
        planet_.height_ = EditorGUILayout.Slider("Height", planet_.height_, 1.0f, 100.0f);

        planet_.width_points_count_ = EditorGUILayout.IntSlider("Number of Points Width", planet_.width_points_count_, 2, 100);
        planet_.height_points_count_ = EditorGUILayout.IntSlider("Number of Points Height", planet_.height_points_count_, 2, 100);

        planet_.fold_out_sphere_ = EditorGUILayout.Slider("Fold out Sphere", planet_.fold_out_sphere_, 0.0f, 1.0f);

        planet_.amplitude_ = EditorGUILayout.Slider("Amplitude", planet_.amplitude_, 0.0f, 1.0f);




        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Heightmap");
        planet_.height_map_ = EditorGUILayout.ObjectField(
                                                            "Heightmap",
                                                            planet_.height_map_,
                                                            typeof(Texture2D),
                                                            true)
                                                            as Texture2D;
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Maintexture");
        planet_.main_texture_ = EditorGUILayout.ObjectField(
                                                            "Main Texture",
                                                            planet_.main_texture_,
                                                            typeof(Texture2D),
                                                            true)
                                                            as Texture2D;
        EditorGUILayout.EndVertical();


        planet_.noise_intensity_ = EditorGUILayout.Slider("Noise intensity", planet_.noise_intensity_, 0.0f, 50.0f);

        if (GUILayout.Button("Generate"))
        {
            planet_.create_noise_texture();
            planet_.create_height_map_and_main_texture();
        }
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Noise");
        planet_.noise_texture_ = EditorGUILayout.ObjectField(
                                                            "Noise",
                                                            planet_.noise_texture_,
                                                            typeof(Texture2D),
                                                            true)
                                                            as Texture2D;
        EditorGUILayout.EndVertical();

        planet_.create_mesh();
        
    }



    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //}
}
