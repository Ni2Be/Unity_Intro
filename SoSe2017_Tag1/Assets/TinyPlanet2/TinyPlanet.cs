using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TinyPlanet : MonoBehaviour
{
    #region Mesh Stuff
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;
    #endregion

    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> sphereVertices = new List<Vector3>();
    List<Vector3> foldoutVertices = new List<Vector3>();

    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    public float Width = 10f;
    public float Height = 10f;

    public int pointsWidth = 10;
    public int pointsHeight = 10;

    public float foldoutSphereT;
    public Vector3 center;

    public Texture2D mainTexture;
    public Texture2D heightmap;



    public void Reset()
    {
        CreateMesh();
    }

    public void CreateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = meshFilter.sharedMesh;

        if (mesh == null)
            mesh = new Mesh();

        vertices.Clear();
        sphereVertices.Clear();
        foldoutVertices.Clear();
        uvs.Clear();
        triangles.Clear();

        // parameter berechnen
        float widthStep = Width / (float)(pointsWidth - 1f);
        float heightStep = Height / (float)(pointsHeight - 1f);
        float widthStepSphere = 360f / (float)(pointsWidth - 1f);
        float heightStepSphere = 180f / (float)(pointsHeight - 1f); ;

        float widthParameter = 1f / (float)(pointsWidth - 1f);
        float heightParameter = 1f / (float)(pointsHeight - 1f); ;

        for (int u = 0; u < pointsWidth; u++)
        {
            for (int w = 0; w < pointsHeight; w++)
            {
                // vertices berechnen
                vertices.Add(new Vector3(
                    u * widthStep,
                    0f,
                    w * heightStep));

                Vector3 spherePoint = PointOnSphere(
                        u * widthStepSphere,
                        w * heightStepSphere,
                        Height / 2f,
                        center);

                Vector3 rotatedSpherePoint =
                    RotateAroundPivot(
                        spherePoint,
                        center,
                        new Vector3(90f, 0f, 0f));

                sphereVertices.Add(rotatedSpherePoint);

                // uvs berechnen
                uvs.Add(new Vector2(u * widthParameter, w * heightParameter));
            }
        }

        // triangles berechnen
        for (int u = 0; u < pointsWidth - 1; u++)
        {
            for (int w = 0; w < pointsHeight - 1; w++)
            {
                int P00 = u * pointsHeight + w;
                int P01 = u * pointsHeight + (w + 1);
                int P10 = (u + 1) * pointsHeight + w;
                int P11 = (u + 1) * pointsHeight + (w + 1);

                triangles.Add(P00);
                triangles.Add(P01);
                triangles.Add(P10);

                triangles.Add(P10);
                triangles.Add(P01);
                triangles.Add(P11);
            }
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        ApplyHeightMap();

        foldoutVertices = new List<Vector3>(vertices);
        for (int i = 0; i < vertices.Count; i++)
        {
            foldoutVertices[i] =
                Vector3.Lerp(
                    verticesWithHeight[i],
                    sphereVerticesWithHeight[i],
                    foldoutSphereT);
        }

        mesh.Clear();
        mesh.SetVertices(foldoutVertices);
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;

        ApplyMainTexture();
    }

    public Vector3 PointOnSphere(float lon, float lat, float radius, Vector3 center)
    {
        // longitude --> ost-west
        // latitude  --> nord-süd

        float lonRad = lon * Mathf.Deg2Rad;
        float latRad = lat * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(lonRad) * Mathf.Sin(latRad);
        float y = radius * Mathf.Sin(lonRad) * Mathf.Sin(latRad);
        float z = radius * Mathf.Cos(latRad);

        return center + new Vector3(x, y, z);
    }

    public Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 direction = point - pivot;
        direction = Quaternion.Euler(angles) * direction;
        Vector3 newPoint = pivot + direction;
        return newPoint;
    }

    public void ApplyMainTexture()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.sharedMaterial == null)
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        if (mainTexture != null)
            meshRenderer.sharedMaterial.mainTexture = mainTexture;
    }

    public float amplitude = 1f;
    Vector3[] verticesWithHeight;
    Vector3[] sphereVerticesWithHeight;

    public void ApplyHeightMap()
    {
        if (heightmap == null)
        {
            // workaround
            heightmap = Resources.Load("heightmap") as Texture2D;
            //return;
        }
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh;

        verticesWithHeight = vertices.ToArray();
        sphereVerticesWithHeight = sphereVertices.ToArray();
        Vector2[] uvs = mesh.uv;

        for (int i = 0; i < verticesWithHeight.Length; i++)
        {
            Vector2 uv = uvs[i];
            Color heightColor = heightmap.GetPixelBilinear(uv.x, uv.y);
            float height = heightColor.grayscale * amplitude;

            verticesWithHeight[i] =
                new Vector3(
                    vertices[i].x,
                    height,
                    vertices[i].z);

            Vector3 position = sphereVertices[i];
            Vector3 heightDirection = (position - center).normalized;
            sphereVerticesWithHeight[i] += heightDirection * height;
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
