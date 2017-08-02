using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class House : MonoBehaviour
{
    #region Parameter des einfachen Hauses
    public float Height = 3;
    public float Width = 3;
    public float Depth = 5;

    public float wallHeightP = 0.7f;
    #endregion // Parameter des einfachen Hauses

    // Die benötigten Positionen
    public Vector3 P0, P1, P2, P3, P4, P5,P6,P7,P8,P9;

    #region Mesh und Komponenten
    public Mesh mesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    #endregion // Mesh und Komponenten

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void CreateMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = this.gameObject.AddComponent<MeshFilter>();

        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = this.gameObject.AddComponent<MeshRenderer>();

        this.mesh = meshFilter.sharedMesh;

        if (this.mesh == null)
            this.mesh = new Mesh();

        // Positionen berechnen
        // TODO: Berechne die benötigten Positionen 
        //P0 =  ;
        //P1 = 
        //P2 = 
        //P3 = 
        //P4 = 

        //P5 = 
        //P6 = 
        //P7 = 
        //P8 = 
        //P9 = 
                

        // Triangle Arrays for submeshes
        Vector3[] vertices = new Vector3[26];
        Vector2[] uv = new Vector2[vertices.Length];

        // Arrays für die Submeshes
        // TODO: ein Array pro submesh
        
        // TODO: Vertices anlegen, UV-Koordinaten berechnen
        
        
        
        mesh.Clear();
        mesh.vertices = vertices;

        // TODO: Submesh-Triangles zuweisen
        mesh.subMeshCount = 3;
        //mesh.SetTriangles(, 0);
        //mesh.SetTriangles(, 1);
        //mesh.SetTriangles(, 2);

        mesh.uv = uv;

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        // TODO: Texturen laden

        // TODO: Material-Array anlegen

        // TODO: Materialien erstellen

        // TODO: Den Materialien die Textur zuweisen

        // TODO: Dem MeshRenderer das MaterialArray übergeben

    }

#if UNITY_EDITOR
    [MenuItem("GameObject/Primitives/SimpleHouse", false, 50)]
    public static House CreateHouse()
    {
        GameObject gO = new GameObject("SimpleHouse");
        House house = gO.AddComponent<House>();

        house.CreateMesh();

        return house;
    }
#endif
}