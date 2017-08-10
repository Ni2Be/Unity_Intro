using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Planet_Generator))]
public class Planet_Gen_Editor : Editor
{
    public bool hand_adjust_foldout_    = true;
    public bool auto_gen_foldout_       = true;
    public bool animation_foldout_      = true;
    public bool debug_foldout_          = false;

    Planet_Generator planet_;

    void Awake()
    {
        planet_ = target as Planet_Generator;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        if (hand_adjust_foldout_ = EditorGUILayout.Foldout(hand_adjust_foldout_, "Hand adjusting"))
        {
            planet_.split_count_            = EditorGUILayout.IntSlider("Split Count", planet_.split_count_, 1, 35);
            planet_.height_factor_          = EditorGUILayout.Slider("Height Factor", planet_.height_factor_, 0.0f, 1.5f);
            planet_.water_height_           = EditorGUILayout.Slider("Water height", planet_.water_height_, 0.02f, 1.0f);

            if (planet_.perlin_generator_ != null)
            {

                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Noise");
            
                
                planet_.temperature_                    = EditorGUILayout.Slider("Temperature", planet_.temperature_, 0.0f, 1.0f);

                planet_.perlin_generator_.noise_map_    = EditorGUILayout.ObjectField(
                                                                "Heightmap",
                                                                planet_.perlin_generator_.noise_map_,
                                                                typeof(Texture2D),
                                                                true)
                                                                as Texture2D;

                EditorGUILayout.EndVertical();
            }
            if (GUILayout.Button("Generate"))
            {
                if (planet_.is_star_)
                {
                    planet_.height_map_ = planet_.generate_flat_height_map();
                }
                else
                    planet_.height_map_ = planet_.perlin_generator_.generate_2d_noise();
            }

            if (GUILayout.Button("Apply"))
            {
                planet_.apply_textures();
            }


        }
        if (animation_foldout_ = EditorGUILayout.Foldout(animation_foldout_, "Animation"))
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Animation");
            planet_.wave_amplitude_         = EditorGUILayout.Slider("Wave amplitude", planet_.wave_amplitude_, 0.0f, 0.05f);
            planet_.wave_height_            = EditorGUILayout.IntSlider("Wave height", planet_.wave_height_, 0, 20);
            planet_.rotation_speed_         = EditorGUILayout.Slider("rotation speed", planet_.rotation_speed_, 0.0f, 100.0f);
            EditorGUILayout.EndVertical();
        }

        //
        if (auto_gen_foldout_ = EditorGUILayout.Foldout(auto_gen_foldout_, "Auto gen"))
        {
            if (GUILayout.Button("Generate Sun"))
            {
                planet_.generate_sun();
                return;
            }
            if (GUILayout.Button("Generate Terra"))
            {
                planet_.generate_terra();
                return;
            }
            if (GUILayout.Button("Generate Hot"))
            {
                planet_.generate_hot();
                return;
            }
        }
        //DEBUG HELPER
        if (debug_foldout_ = EditorGUILayout.Foldout(debug_foldout_, "Debug"))
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Debug");
            planet_.water_debug_height_     = EditorGUILayout.Slider("Debug water height", planet_.water_debug_height_, 1.0f, 2.0f);
            EditorGUILayout.EndVertical();
        }
        //END DEBUG HELPER

        if (EditorGUI.EndChangeCheck())
            planet_.create_mesh();
    }
}
