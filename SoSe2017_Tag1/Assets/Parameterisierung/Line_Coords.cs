using UnityEngine;
using System.Collections;

public class Line_Coords : MonoBehaviour
{
    public Vector3  p0 = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3  p1 = new Vector3(1.0f, 1.0f, 1.0f);

    [Range(-1, 2)]
    public float    t = 0.5f;



    void OnDrawGizmos()
    {
        //axis
        Gizmos.color = Color.gray;
        Gizmos.DrawRay(p0, (p0 - p1) * 1000);
        Gizmos.DrawRay(p1, (p1 - p0) * 1000);

        //Spheres
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(p0, 0.1f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(p1, 0.1f);

        //line
        Gizmos.color = Color.white;
        Gizmos.DrawLine(p0, p1);

        //point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(point_on_line(p0, p1, t), 0.05f);
    }

    public static Vector3 point_on_line(Vector3 p0, Vector3 p1, float t)
    {
        return ( 1 - t ) * p0 + t * p1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
