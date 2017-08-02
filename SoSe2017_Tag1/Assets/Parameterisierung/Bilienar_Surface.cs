using UnityEngine;
using System.Collections;


public class Bilienar_Surface : MonoBehaviour
{
    public Vector3 p00 = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 p01 = new Vector3(0.0f, 0.0f, 1.0f);
    public Vector3 p10 = new Vector3(1.0f, 0.0f, 0.0f);
    public Vector3 p11 = new Vector3(1.0f, 0.0f, 1.0f);

    [Range(0.0f, 1.0f)]
    public float u = 0.5f;
    [Range(0.0f, 1.0f)]
    public float w = 0.5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(p00, 0.1f);
        Gizmos.DrawSphere(p01, 0.1f);
        Gizmos.DrawSphere(p10, 0.1f);
        Gizmos.DrawSphere(p11, 0.1f);


        Gizmos.color = Color.grey;
        Gizmos.DrawLine(p00, p01);
        Gizmos.DrawLine(p00, p10);
        Gizmos.DrawLine(p10, p11);
        Gizmos.DrawLine(p01, p11);


        Gizmos.color = Color.grey;
        for (float x = -1.0f; x < 2.0f; x += 0.01f)
        {
            for (float y = -1.0f; y < 2.0f; y += 0.01f)
            {
                Gizmos.DrawCube(point_on_bilinear_surface(p00, p01, p10, p11, x, y), new Vector3(0.01f, 0.01f, 0.01f));
            }
        }

        Gizmos.color = Color.cyan;
        for (float x = 0.0f; x < 1.0f; x += 0.01f)
        {
            for (float y = 0.0f; y < 1.0f; y += 0.01f)
            {
                Gizmos.DrawCube(point_on_bilinear_surface(p00, p01, p10, p11, x, y), new Vector3(0.01f, 0.01f, 0.01f));
            }
        }



        Gizmos.color = Color.red;
        Gizmos.DrawSphere(point_on_bilinear_surface(p00, p01, p10, p11, u, w), 0.1f);


    }

    public static Vector3 point_on_bilinear_surface(Vector3 p00, Vector3 p01, Vector3 p10, Vector3 p11, float u, float w)
    {
        Vector3 p0 = point_on_line(p00, p10, u);
        Vector3 p1 = point_on_line(p01, p11, u);
        return point_on_line(p0, p1, w);
    }

    public static Vector3 point_on_line(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
