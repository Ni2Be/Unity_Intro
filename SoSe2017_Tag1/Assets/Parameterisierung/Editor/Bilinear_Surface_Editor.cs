using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Bilienar_Surface))]
public class Bilinear_Surface_Editor : Editor
{
    Bilienar_Surface bilinear_surface_;

    // Use this for initialization
    void Awake()
    {
        bilinear_surface_ = target as Bilienar_Surface;
    }

    // Update is called once per frame
    void OnSceneGUI()
    {
        bilinear_surface_.p00 = Handles.FreeMoveHandle(
                                            bilinear_surface_.p00,
                                            Quaternion.identity,
                                            0.1f,
                                            Vector3.zero,
                                            Handles.SphereCap);

        bilinear_surface_.p01 = Handles.FreeMoveHandle(
                                            bilinear_surface_.p01,
                                            Quaternion.identity,
                                            0.1f,
                                            Vector3.zero,
                                            Handles.SphereCap);

        bilinear_surface_.p10 = Handles.FreeMoveHandle(
                                            bilinear_surface_.p10,
                                            Quaternion.identity,
                                            0.1f,
                                            Vector3.zero,
                                            Handles.SphereCap);

        bilinear_surface_.p11 = Handles.FreeMoveHandle(
                                            bilinear_surface_.p11,
                                            Quaternion.identity,
                                            0.1f,
                                            Vector3.zero,
                                            Handles.SphereCap);
    }
}
