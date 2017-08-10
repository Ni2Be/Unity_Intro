using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Perlin_Noise))]
public class Perlin_Noise_Editor : Editor
{
    Perlin_Noise perlin_noise_;

    void Awake()
    {
        perlin_noise_ = target as Perlin_Noise;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        perlin_noise_.octaves_count_        = EditorGUILayout.IntSlider("Split Count", perlin_noise_.octaves_count_, 1, 10);
        perlin_noise_.granulation_height_   = EditorGUILayout.Slider("Graqnulation Height", perlin_noise_.granulation_height_, 0.5f, 2.0f);
        perlin_noise_.exponential_mode_on_  = EditorGUILayout.Toggle("Exponetial Mode", perlin_noise_.exponential_mode_on_);
        perlin_noise_.one_minus_abs_on_     = EditorGUILayout.Toggle("One minus Abs Mode", perlin_noise_.one_minus_abs_on_);

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Noise");
        perlin_noise_.noise_map_            = EditorGUILayout.ObjectField(
                                                            "Heightmap",
                                                            perlin_noise_.noise_map_,
                                                            typeof(Texture2D),
                                                            true)
                                                            as Texture2D;
        EditorGUILayout.EndVertical();


        //ONLY FOR DEBUG
        //if (GUILayout.Button("Generate"))
        //{
        //    perlin_noise_.generate_2d_noise();
        //}

        //if (GUILayout.Button("Apply"))
        //{
        //    perlin_noise_.generate_mesh();
        //}

        //if (GUILayout.Button("Gen / Apply"))
        //{
        //    perlin_noise_.generate_2d_noise();
        //    perlin_noise_.generate_mesh();
        //}
        //END ONLY FOR DEBUG

        EditorGUI.EndChangeCheck();
    }
}
