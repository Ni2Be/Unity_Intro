using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(L_System_Behaviour))]
public class L_System_Editor : Editor
{
    L_System_Behaviour      l_sys_behaviour_;
    L_System                l_sys_;
    Turtle_Renderer         turtle_renderer_;

    // Use this for initialization
    void Awake()
    {
        l_sys_behaviour_    = target as L_System_Behaviour;
        l_sys_              = l_sys_behaviour_.l_sys_;
        turtle_renderer_    = l_sys_behaviour_.turtle_renderer_;
    }

    public bool             l_sys_foldout_  = true;
    public bool             turtle_foldout_ = true;

    private string          new_rule_       = "Edit New Rule";

    public override void OnInspectorGUI()
    {
        
        l_sys_behaviour_    = target as L_System_Behaviour;
        l_sys_              = l_sys_behaviour_.l_sys_;
        turtle_renderer_    = l_sys_behaviour_.turtle_renderer_;


        EditorGUI.BeginChangeCheck();

        #region L-Sysem
        if (l_sys_foldout_ = EditorGUILayout.Foldout(l_sys_foldout_, "L-System"))
        {
            //Axiom
            l_sys_.axiom_           = EditorGUILayout.TextField("Axiom", l_sys_.axiom_);
            //Iteration
            l_sys_.iterations_      = EditorGUILayout.IntSlider("Iterations", l_sys_.iterations_, 0, 10);

            //Rules
            List<string>    keys    = l_sys_.rules_.Keys.ToList();

            foreach (string item in keys)
            {
                EditorGUILayout.BeginHorizontal();
                //Label pre
                EditorGUILayout.LabelField(item + " = ");
                //Label post
                string      rhs     = EditorGUILayout.TextField(l_sys_.rules_[item]);
                l_sys_.edit_rule(item, rhs);
                //del
                if(GUILayout.Button("X"))
                {
                    l_sys_.delete_rule_(item);
                }
                EditorGUILayout.EndHorizontal();
            }

            new_rule_               = EditorGUILayout.TextField(new_rule_);

            if (GUILayout.Button("Add Rule"))
            {
                l_sys_.add_rule(new_rule_);
                new_rule_           = "Edit New Rule";
            }
        }
        #endregion

        #region Turtle
        if (turtle_foldout_ = EditorGUILayout.Foldout(turtle_foldout_, "Turtle"))
        {
            turtle_renderer_.angle_     = EditorGUILayout.Slider("Angle", turtle_renderer_.angle_, 0.0f, 360.0f);
            turtle_renderer_.distance_  = EditorGUILayout.Slider("Distance", turtle_renderer_.distance_, 0.0f, 2.0f);
        }
        #endregion

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }
}
