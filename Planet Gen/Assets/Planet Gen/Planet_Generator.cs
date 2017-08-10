using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(Perlin_Noise))]
public class Planet_Generator : MonoBehaviour
{
    //Note: There are more member variables in the region Texturing

    //mesh
    List<Vector3>           vertices_               = new List<Vector3>();
    List<Vector3>           vertices_with_height_   = new List<Vector3>();
    List<int>               triangle_indices_       = new List<int>();
    List<Triangle>          triangles_              = new List<Triangle>();
    List<Vector2>           uvs_                    = new List<Vector2>();
    
    #region mesh
    MeshFilter      mesh_filter_;
    MeshRenderer    mesh_renderer_;
    Mesh            mesh_;
    #endregion

    //ocean submesh
    List<Vector3>           ocean_vertices_         = new List<Vector3>();
    List<int>               ocean_triangle_indices_ = new List<int>();
    List<Triangle>          ocean_triangles_        = new List<Triangle>();
    List<Vector2>           ocean_uvs_              = new List<Vector2>();

    //Height map
    public Texture2D        height_map_;
    public float            height_factor_          = 1.0f;
    public float            water_height_           = 0.08f;

    //Extern Components
    public Perlin_Noise      perlin_generator_;
    public Terrain_Generator terrain_generator_;

    //Animation
    List<Vector3>           ocean_vertices_copy_    = new List<Vector3>();
    public float            rotation_speed_         = 1.0f;
    public bool             is_running_             = false;
    public float            wave_amplitude_         = 0.01f;
    public int              wave_height_            = 20;
    public int              vertices_count_         = 0;
    public float            time_counter_           = 0.0f;

    //Progress
    public float            progress_               = 0.0f;

    //DEBUG HELPER
    public float    water_debug_height_     = 1.0f;
    //END DEBUG HELPER

    //water material (other materials in Terrain generator)
    Material        main_material_;
    Material        water_material_;

    //used in split_triangles(), defines how often the triangle will be split
    public int      split_count_            = 30;

    public Vector3  center_;
    public float    radius_                 = 1.0f;



    public void create_mesh()
    {
        //delete old stuff if there is any
        start_clean();

        //generate the ball
        generate_icosphere();

        //shape the ball
        if (!is_star_)
            apply_height_map();

        //generate submesh
        generate_ocean_sub_mesh();


        //Triangles to Indices
        triangles_to_indices(triangle_indices_, triangles_);

        //generate a not seeable ocean to avoid "to manny materials" errors
        if (ocean_vertices_.Count == 0)
            generate_little_ocean();

        //save for animation
        foreach (var vertex in ocean_vertices_)
            ocean_vertices_copy_.Add(new Vector3(vertex.x, vertex.y, vertex.z));
        
        //Add the ocean_sub_mesh
        vertices_count_     = vertices_.Count;
        triangles_to_indices(ocean_triangle_indices_, ocean_triangles_, vertices_count_); // with offset
        vertices_           .AddRange(ocean_vertices_);
        uvs_                .AddRange(ocean_uvs_);

        //set mesh parameters
        mesh_.Clear();
        mesh_.vertices          = vertices_.ToArray();
        
        mesh_.subMeshCount  = 2;
        mesh_.SetTriangles(triangle_indices_, 0);
        mesh_.SetTriangles(ocean_triangle_indices_, 1);

        mesh_.uv                = uvs_.ToArray();
        mesh_filter_.sharedMesh = mesh_;
        mesh_.RecalculateNormals();
    }

    #region Clean up
    public void start_clean()
    {
        mesh_filter_        = GetComponent<MeshFilter>();
        mesh_renderer_      = GetComponent<MeshRenderer>();
        mesh_               = mesh_filter_.sharedMesh;

        if (mesh_ == null)
            mesh_ = new Mesh();

        vertices_               .Clear();
        triangles_              .Clear();
        triangle_indices_       .Clear();
        uvs_                    .Clear();
        vertices_with_height_   .Clear();

        ocean_vertices_         .Clear();
        ocean_triangle_indices_ .Clear();
        ocean_triangles_        .Clear();
        ocean_uvs_              .Clear();

        ocean_vertices_copy_    .Clear();
    }
    #endregion

    #region generate_icosphere
    public void generate_icosphere()
    {
        var icosahedron_vertices    = new List<Vector3>();
        var icosahedron_triangles   = new List<Triangle>();
        generate_icosahedron(icosahedron_vertices, icosahedron_triangles);

        //splits triangles splitcount times
        split_triangles(icosahedron_vertices, icosahedron_triangles);

        //bulb out
        spherify();

        //uvs
        generate_uvs();
    }

    private void generate_icosahedron(List<Vector3> icosahedron_vertices, List<Triangle> icosahedron_triangles)
    {
        //golden ratio
        float phi = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        //Vertices
        icosahedron_vertices.Add(new Vector3(0.0f,  1.0f, phi));//0
        icosahedron_vertices.Add(new Vector3(0.0f, -1.0f, phi));//1
        icosahedron_vertices.Add(new Vector3(0.0f,  1.0f, -phi));//2
        icosahedron_vertices.Add(new Vector3(0.0f, -1.0f, -phi));//3

        icosahedron_vertices.Add(new Vector3( 1.0f,  phi, 0.0f));//4
        icosahedron_vertices.Add(new Vector3(-1.0f,  phi, 0.0f));//5
        icosahedron_vertices.Add(new Vector3( 1.0f, -phi, 0.0f));//6
        icosahedron_vertices.Add(new Vector3(-1.0f, -phi, 0.0f));//7

        icosahedron_vertices.Add(new Vector3( phi, 0.0f,  1.0f));//8
        icosahedron_vertices.Add(new Vector3( phi, 0.0f, -1.0f));//9
        icosahedron_vertices.Add(new Vector3(-phi, 0.0f,  1.0f));//10
        icosahedron_vertices.Add(new Vector3(-phi, 0.0f, -1.0f));//11

        //Triangles
        //around v10
        icosahedron_triangles.Add(new Triangle( 0,  5, 10));
        icosahedron_triangles.Add(new Triangle( 5, 11, 10));
        icosahedron_triangles.Add(new Triangle(11,  7, 10));
        icosahedron_triangles.Add(new Triangle( 7,  1, 10));
        icosahedron_triangles.Add(new Triangle( 1,  0, 10));

        //around v9
        icosahedron_triangles.Add(new Triangle(2, 4, 9));
        icosahedron_triangles.Add(new Triangle(4, 8, 9));
        icosahedron_triangles.Add(new Triangle(8, 6, 9));
        icosahedron_triangles.Add(new Triangle(6, 3, 9));
        icosahedron_triangles.Add(new Triangle(3, 2, 9));

        //top down v5/v4 to v7/v6
        icosahedron_triangles.Add(new Triangle( 5, 4,  2));
        icosahedron_triangles.Add(new Triangle( 5, 2, 11));
        icosahedron_triangles.Add(new Triangle(11, 2,  3));
        icosahedron_triangles.Add(new Triangle(11, 3,  7));
        icosahedron_triangles.Add(new Triangle( 7, 3,  6));

        //green
        icosahedron_triangles.Add(new Triangle(4, 5, 0));
        icosahedron_triangles.Add(new Triangle(4, 0, 8));
        icosahedron_triangles.Add(new Triangle(8, 0, 1));
        icosahedron_triangles.Add(new Triangle(8, 1, 6));
        icosahedron_triangles.Add(new Triangle(6, 1, 7));
    }

    //splits triangles in split_count_^2 triangles and save the vertices into vertices_
    //and the triangle indices into triangles_
    public void split_triangles(List<Vector3> icosahedron_vertices, List<Triangle> icosahedron_triangles)
    {
        foreach (var ico_triangle in icosahedron_triangles)
        {

            //Two arrays with points to interpolate the other points
            #region Points on outer Lines
            List<Vector3> p0_p1 = new List<Vector3>();
            List<Vector3> p2_p1 = new List<Vector3>();
            float side_length_factor = 1.0f / split_count_;

            //filling the arrays
            for (int i = 0; i < split_count_ + 1; i++)
            {
                float distance_factor = i * side_length_factor;
                p0_p1.Add(Vector3.Lerp(
                                icosahedron_vertices[ico_triangle.p0],
                                icosahedron_vertices[ico_triangle.p1],
                                distance_factor));

                p2_p1.Add(Vector3.Lerp(
                                icosahedron_vertices[ico_triangle.p2],
                                icosahedron_vertices[ico_triangle.p1],
                                distance_factor));
            }

            //Debug.Log("p0-p1: " + p0_p1.Count.ToString());
            //Debug.Log("split_count_: " + split_count_);
            #endregion

            //Vertices
            #region real vertex points
            List<List<Vector3>> new_vertices = new List<List<Vector3>>();

            for (int h = 0; h < split_count_ + 1; h++)
            {
                List<Vector3> new_vertices_line = new List<Vector3>();
                for (int w = 0; w < split_count_ + 1 - h; w++)
                {
                    //Vertex
                    float distance_factor = w * (1.0f / (split_count_ - h));
                    if (split_count_ - h == 0)
                        distance_factor = 0.0f;
                    new_vertices_line.Add(Vector3.Lerp(
                                              p0_p1[h],
                                              p2_p1[h],
                                              distance_factor));

                    //if (h == 2)
                    //{
                    //    Debug.Log(new_vertices_line[new_vertices_line.Count - 1]);
                    //    Debug.Log(p0_p1[h]);
                    //    Debug.Log(p2_p1[h]);
                    //    Debug.Log(distance_factor);
                    //}
                }
                new_vertices.Add(new_vertices_line);
            }
            //Put it in one List
            var new_vertices_list = new List<Vector3>();
            foreach (var vertieces_list in new_vertices)
            {
                new_vertices_list.AddRange(vertieces_list);
            }
            #endregion

            #region triangle indices
            //Triangle indices
            var new_triangles = new List<Triangle>();
            for (int counter = 0, h = 0, w = 0; h < new_vertices.Count - 1; h++)
            {
                for (w = 0; w < new_vertices[h].Count - 1; w++)
                {
                    //bottom
                    new_triangles.Add(
                                new Triangle(
                                        counter + w,
                                        new_vertices[h].Count + counter + w,
                                        counter + 1 + w));

                    //top
                    if (w < new_vertices[h].Count - 2)
                    {
                        new_triangles.Add(
                                    new Triangle(
                                            new_vertices[h].Count + counter + w,
                                            new_vertices[h].Count + counter + w + 1,
                                            counter + 1 + w));
                    }
                }
                counter += w + 1;
            }
            #endregion

            //Put it in the right global listspace
            for (int i = 0; i < new_triangles.Count; i++)
            {
                triangles_.Add(new Triangle(
                                    new_triangles[i].p0 + vertices_.Count,
                                    new_triangles[i].p1 + vertices_.Count,
                                    new_triangles[i].p2 + vertices_.Count));
            }
            vertices_.AddRange(new_vertices_list);
        }
    }

    public void spherify()
    {
        for (int i = 0; i < vertices_.Count; i++)
        {
            Vector3 direction = (vertices_[i] - center_).normalized;
            vertices_[i] = direction * radius_;
        }
    }

    private void generate_uvs()
    {
        for (int i = 0; i < vertices_.Count; i++)
        {
            uvs_.Add(vector_to_lon_lat(vertices_[i]));
        }

        uv_360deg_0deg_recalculation(vertices_, triangles_, uvs_);
    }

    //set the uvs where they jump from 1.0 to 0.0
    private void uv_360deg_0deg_recalculation(List<Vector3> vertices, List<Triangle> triangles, List<Vector2> uvs)
    {

        for (int i = 0; i < triangles.Count; i++)
        {
            //For triangles with two points at 360 / 0 deg
            if ((vertices[triangles[i].p0].z < 0.0f) && (vertices[triangles[i].p0].x <= 0.001f && vertices[triangles[i].p0].x >= -0.001f)
                && ((vertices[triangles[i].p1].x > 0.0f) || (vertices[triangles[i].p2].x > 0.0f)))
            {


                vertices.Add(new Vector3(
                                    vertices[triangles[i].p0].x,
                                    vertices[triangles[i].p0].y,
                                    vertices[triangles[i].p0].z));
                float y = uvs[triangles[i].p0].y;
                uvs.Add(new Vector2(0.0f, y));
                Triangle t = triangles[i];
                t.p0 = vertices.Count - 1;
                triangles[i] = t;
            }

            if ((vertices[triangles[i].p1].z < 0.0f) && (vertices[triangles[i].p1].x <= 0.001f && vertices[triangles[i].p1].x >= -0.001f)
                && ((vertices[triangles[i].p0].x > 0.0f) || (vertices[triangles[i].p2].x > 0.0f)))
            {
                vertices.Add(new Vector3(
                                    vertices[triangles[i].p1].x,
                                    vertices[triangles[i].p1].y,
                                    vertices[triangles[i].p1].z));
                float y = uvs[triangles[i].p1].y;
                uvs.Add(new Vector2(0.0f, y));
                Triangle t = triangles[i];
                t.p1 = vertices.Count - 1;
                triangles[i] = t;
            }

            if ((vertices[triangles[i].p2].z < 0.0f) && (vertices[triangles[i].p2].x <= 0.001f && vertices[triangles[i].p2].x >= -0.001f)
                && ((vertices[triangles[i].p1].x > 0.0f) || (vertices[triangles[i].p0].x > 0.0f)))
            {
                vertices.Add(new Vector3(
                                    vertices[triangles[i].p2].x,
                                    vertices[triangles[i].p2].y,
                                    vertices[triangles[i].p2].z));
                float y = uvs[triangles[i].p2].y;
                uvs.Add(new Vector2(0.0f, y));
                Triangle t = triangles[i];
                t.p2 = vertices.Count - 1;
                triangles[i] = t;
            }


            //for triangles with one point at 360 / 0 deg
            if ((vertices[triangles[i].p0].z < 0.0f) && ((vertices[triangles[i].p0].y >= 0.5f) || (vertices[triangles[i].p0].y <= -0.5f)))
            {
                if ((vertices[triangles[i].p0].x <= 0.001f && vertices[triangles[i].p0].x >= -0.001f)
                    && (((vertices[triangles[i].p1].x > 0.0f) && (vertices[triangles[i].p2].x < 0.0f))))
                {
                    vertices.Add(new Vector3(
                                        vertices[triangles[i].p1].x,
                                        vertices[triangles[i].p1].y,
                                        vertices[triangles[i].p1].z));
                    float x = uvs[triangles[i].p1].x;
                    float y = uvs[triangles[i].p1].y;
                    uvs.Add(new Vector2(x + 1.0f, y));
                    Triangle t = triangles[i];
                    t.p1 = vertices.Count - 1;
                    triangles[i] = t;

                    x = uvs[triangles[i].p0].x;
                    y = uvs[triangles[i].p0].y;

                    uvs[triangles[i].p0] = new Vector2(x + 1.0f, y);
                }

                if ((vertices[triangles[i].p2].x <= 0.001f && vertices[triangles[i].p2].x >= -0.001f)
                    && (((vertices[triangles[i].p0].x < 0.0f) && (vertices[triangles[i].p1].x > 0.0f))))
                {
                    vertices.Add(new Vector3(
                                        vertices[triangles[i].p1].x,
                                        vertices[triangles[i].p1].y,
                                        vertices[triangles[i].p1].z));
                    float x = uvs[triangles[i].p1].x;
                    float y = uvs[triangles[i].p1].y;
                    uvs.Add(new Vector2(x + 1.0f, y));
                    Triangle t = triangles[i];
                    t.p1 = vertices.Count - 1;
                    triangles[i] = t;

                    x = uvs[triangles[i].p2].x;
                    y = uvs[triangles[i].p2].y;

                    uvs[triangles[i].p2] = new Vector2(x + 1.0f, y);


                    //DEBUG
                    //Vector3 p0 = new Vector3(vertices_[triangles_[i].p0].x, vertices_[triangles_[i].p0].y + 1.0f, vertices_[triangles_[i].p0].z);
                    //vertices_[triangles_[i].p0] = p0;
                    //Vector3 p1 = new Vector3(vertices_[vertices_.Count - 1].x, vertices_[vertices_.Count - 1].y + 1.0f, vertices_[vertices_.Count - 1].z);
                    //vertices_[vertices_.Count - 1] = p1;
                    //Vector3 p2 = new Vector3(vertices_[triangles_[i].p2].x, vertices_[triangles_[i].p2].y + 1.0f, vertices_[triangles_[i].p2].z);
                    //vertices_[triangles_[i].p2] = p2;
                    //ENDDEBUG
                }
            }
        }
    }
    #endregion

    #region Sphere manipulation
    public void apply_height_map()
    {
        
        height_map_     = perlin_generator_.noise_map_;

        if (height_map_ == null)
        {
            height_map_ = Resources.Load("heightmap") as Texture2D;
        }

        for (int i = 0; i < vertices_.Count; i++)
        {
            Vector3 direction           = (vertices_[i] - center_).normalized;
            Vector3 delta_height_vector = direction * (height_factor_ * (1.0f - height_map_.GetPixelBilinear(uvs_[i].x, uvs_[i].y).grayscale));
            vertices_[i]                += delta_height_vector;
        }
    }


    public void generate_ocean_sub_mesh()
    {
        int vertex_counter      = 0;
        int triangle_counter    = 0;
        for (int i = 0; i < triangles_.Count; i++)
        {            
            //if one triangle point is under the sea, create a new triangle on sea lvl
            if (   (1.0f - height_map_.GetPixelBilinear(uvs_[triangles_[i].p0].x, uvs_[triangles_[i].p0].y).grayscale <= water_height_)
                || (1.0f - height_map_.GetPixelBilinear(uvs_[triangles_[i].p1].x, uvs_[triangles_[i].p1].y).grayscale <= water_height_)
                || (1.0f - height_map_.GetPixelBilinear(uvs_[triangles_[i].p2].x, uvs_[triangles_[i].p2].y).grayscale <= water_height_))
            {
                int off_set = 0;

                Vector3 direction   = (vertices_[triangles_[i].p0] - center_).normalized;
                Vector3 new_p0      = new Vector3();
                new_p0              = (direction * water_height_) * height_factor_;
                Vector3 radius      = direction * radius_;
                new_p0              += radius;
                new_p0              *= water_debug_height_;
                int p0_index        = 0;
                if (triangle_counter >= 1)
                {
                    if ((new_p0 == ocean_vertices_[vertex_counter - 3 - off_set])
                        && (uvs_[triangles_[i].p0] == ocean_uvs_[vertex_counter - 3 - off_set]))
                    { p0_index = vertex_counter - 3 - off_set;}
                    else if ((new_p0 == ocean_vertices_[vertex_counter - 2 - off_set])
                        && (uvs_[triangles_[i].p0] == ocean_uvs_[vertex_counter - 2 - off_set]))
                    { p0_index = vertex_counter - 2 - off_set; }
                    else if ((new_p0 == ocean_vertices_[vertex_counter - 1 - off_set])
                        && (uvs_[triangles_[i].p0] == ocean_uvs_[vertex_counter - 1 - off_set]))
                    { p0_index = vertex_counter - 1 - off_set; }
                    else
                    {
                        ocean_vertices_ .Add(new_p0);
                        ocean_uvs_      .Add(new Vector2(uvs_[triangles_[i].p0].x, uvs_[triangles_[i].p0].y));
                        p0_index        = vertex_counter++;
                        off_set++;
                    }
                }
                else
                {
                    ocean_vertices_ .Add(new_p0);
                    ocean_uvs_      .Add(new Vector2(uvs_[triangles_[i].p0].x, uvs_[triangles_[i].p0].y));
                    p0_index        = vertex_counter++;
                }

                direction           = (vertices_[triangles_[i].p1] - center_).normalized;
                Vector3 new_p1      = new Vector3();
                new_p1              = (direction * water_height_) * height_factor_;
                radius              = direction * radius_;
                new_p1              += radius;
                new_p1              *= water_debug_height_;
                int p1_index        = 0;
                
                if (triangle_counter >= 1)
                {
                    if ((new_p1 == ocean_vertices_[vertex_counter - 3 - off_set])
                        && (uvs_[triangles_[i].p1] == ocean_uvs_[vertex_counter - 3 - off_set]))
                    { p1_index = vertex_counter - 3 - off_set; }
                    else if ((new_p1 == ocean_vertices_[vertex_counter - 2 - off_set])
                        && (uvs_[triangles_[i].p1] == ocean_uvs_[vertex_counter - 2 - off_set]))
                    { p1_index = vertex_counter - 2 - off_set; }
                    else if ((new_p1 == ocean_vertices_[vertex_counter - 1 - off_set])
                        && (uvs_[triangles_[i].p1] == ocean_uvs_[vertex_counter - 1 - off_set]))
                    { p1_index = vertex_counter - 1 - off_set; }
                    else
                    {
                        ocean_vertices_ .Add(new_p1);
                        ocean_uvs_      .Add(new Vector2(uvs_[triangles_[i].p1].x, uvs_[triangles_[i].p1].y));
                        p1_index        = vertex_counter++;
                        off_set++;
                    }
                }
                else
                {
                    ocean_vertices_ .Add(new_p1);
                    ocean_uvs_      .Add(new Vector2(uvs_[triangles_[i].p1].x, uvs_[triangles_[i].p1].y));
                    p1_index        = vertex_counter++;
                }



                direction           = (vertices_[triangles_[i].p2] - center_).normalized;
                Vector3 new_p2      = new Vector3();
                new_p2              = (direction * water_height_) * height_factor_;
                radius              = direction * radius_;
                new_p2              += radius;
                new_p2              *= water_debug_height_;
                int p2_index        = 0;
                
                if (triangle_counter >= 1)
                {
                    if ((new_p2 == ocean_vertices_[vertex_counter - 3 - off_set])
                        && (uvs_[triangles_[i].p2] == ocean_uvs_[vertex_counter - 3 - off_set]))
                    { p2_index = vertex_counter - 3 - off_set; }
                    else if ((new_p2 == ocean_vertices_[vertex_counter - 2 - off_set])
                        && (uvs_[triangles_[i].p2] == ocean_uvs_[vertex_counter - 2 - off_set]))
                    { p2_index = vertex_counter - 2 - off_set; }
                    else if ((new_p2 == ocean_vertices_[vertex_counter - 1 - off_set])
                        && (uvs_[triangles_[i].p2] == ocean_uvs_[vertex_counter - 1 - off_set]))
                    { p2_index = vertex_counter - 1 - off_set; }
                    else
                    {
                        ocean_vertices_ .Add(new_p2);
                        ocean_uvs_      .Add(new Vector2(uvs_[triangles_[i].p2].x, uvs_[triangles_[i].p2].y));
                        p2_index        = vertex_counter++;
                        off_set++;
                    }
                }
                else
                {
                    ocean_vertices_ .Add(new_p2);
                    ocean_uvs_      .Add(new Vector2(uvs_[triangles_[i].p2].x, uvs_[triangles_[i].p2].y));
                    p2_index        = vertex_counter++;
                }

                
                ocean_triangles_.Add(new Triangle(p0_index, p1_index, p2_index));
                triangle_counter++;
            }
        }
    }
    #endregion

    #region Texturing
    Material    water_;
    Material    lava_;

    public float       temperature_        = 0.8f;
    public float       grass_begin_        = 0.09f;
    public float       hill_begin_         = 0.7f;
    public float       mountain_begin_     = 0.8f;
    public float       glacier_begin_      = 0.9f;
    
    public bool        is_terestrial_      = true;
    public bool        is_comet_           = true;
    public bool        is_star_            = false;

    public float       smoothness_         = 0.05f;
    
    public void load_materials()
    {
        water_          = Resources.Load("Materials/water", typeof(Material)) as Material;
        lava_           = Resources.Load("Materials/lava", typeof(Material)) as Material;
    }

    public void apply_textures()
    {
        load_materials();


        if (is_star_ || (temperature_ == 1.0f))
            water_material_ = lava_;
        else
            water_material_ = water_;


        terrain_generator_ = new Terrain_Generator();

        //set grass begin to the actual water height
        grass_begin_                = water_height_ + 0.02f;

        //generate new material from heightmap + parameters
        main_material_              = new Material(Shader.Find("Standard"));
        main_material_              = terrain_generator_.generate_material(
                                                                height_map_,
                                                                temperature_,
                                                                grass_begin_,
                                                                hill_begin_,
                                                                mountain_begin_,
                                                                glacier_begin_,
                                                                Terrain_Generator.PLANET_TYPE.TERRA,
                                                                smoothness_);

        main_material_.name         = "main_texture";
        water_material_.name        = "water_texture";
        Material[] materials        = new Material[2];

        //a star will use the water material (lava) also for its texture
        //only necessary if the waves animation goes throug the ground
        if (is_star_)
            materials[0] = water_material_;
        else
            materials[0] = main_material_;
        
        materials[1]                = water_material_;

        mesh_renderer_ = GetComponent<MeshRenderer>();

        mesh_renderer_.materials = materials;
        
    }
    #endregion

    #region Animation
    public void update_mesh()
    {
        calculate_wave();

        vertices_.RemoveRange(vertices_count_, ocean_vertices_.Count);
        vertices_.AddRange(ocean_vertices_);

        mesh_.vertices      = vertices_.ToArray();
    }
    public void calculate_wave()
    {

        for (int i = 0; i < ocean_vertices_.Count; i++)
        {
            Vector3 normal      = (ocean_vertices_copy_[i] - center_).normalized;
            Vector3 old_vertex  = ocean_vertices_copy_[i];
            
            float w             = ocean_uvs_[i].x;
            float h             = ocean_uvs_[i].y;
            
            //the waves will accure in a sin(x) like wave from left to right and a sin(x / 3) wave from top to bottom
            Vector3 amplitude   = normal * (( (float)Mathf.Sin(time_counter_ + w * 2 * Mathf.PI * wave_height_) 
                                            + (float)Mathf.Sin(time_counter_ + h * 2 * Mathf.PI * wave_height_) / 3) * wave_amplitude_);

            Vector3 new_vertex = old_vertex + amplitude;
            ocean_vertices_[i] = new_vertex;
        }
    }
    #endregion

    #region unity
    public void OnDrawGizmos()
    {
        //DEBUG
        //for (int i = 0; i < 6; i++)
        //{
        //    Gizmos.DrawSphere(vertices_[i], 0.1f);
        //}

        //foreach (var item in vertices_)
        //{
        //    //    if (item.z >= 0.0f)
        //    Gizmos.DrawSphere(item, 0.1f);
        //}
        //ENDDEBUG
    }

    public void Reset()
    {
        perlin_generator_ = GetComponent<Perlin_Noise>();
        if (perlin_generator_ == null)
            perlin_generator_ = new Perlin_Noise();
        perlin_generator_.generate_2d_noise();

        
        height_map_ = perlin_generator_.noise_map_;
        create_mesh();
    }

    // Use this for initialization
    void Start()
    {
        is_running_ = true;
        create_mesh();
    }

    // Update is called once per frame
    void Update()
    {
        //time_counter_ for waves
        time_counter_ += Time.deltaTime;
        transform.Rotate(Vector3.up * Time.deltaTime * rotation_speed_);

        update_mesh();
    }
    #endregion

    #region Helper
    public struct Triangle
    {
        public int p0;
        public int p1;
        public int p2;

        public Triangle(int p0, int p1, int p2)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
        }
    }

    
    public void triangles_to_indices(List<int> indices, List<Triangle> triagnles, int offset = 0)
    {
        foreach (var triangle in triagnles)
        {
            indices.Add(triangle.p0 + offset);
            indices.Add(triangle.p1 + offset);
            indices.Add(triangle.p2 + offset);
        }
    }


    public Vector2 vector_to_lon_lat(Vector3 vector)
    {
        Vector2 result      = new Vector2();
        Vector3 normal      = vector.normalized;


        float   longitude   = -(Mathf.Atan2(-normal.z, -normal.x)) - Mathf.PI / 2.0f;
        if (longitude < -Mathf.PI)
                longitude   += Mathf.PI * 2.0f;

        Vector3 p           = (new Vector3(normal.x, 0.0f, normal.z)).normalized;
        
        float   latitude    = Mathf.Acos(Vector3.Dot(p, normal));
        
        if (normal.y < 0.0f)
                latitude    *= -1.0f;

        if (float.IsNaN(latitude))
                latitude    = 0.0f;


        result.x            = 1.0f - (longitude + Mathf.PI) / (2 * Mathf.PI);
        result.y            = (latitude  + (Mathf.PI / 2)) / (Mathf.PI);
        
        return result;
    }

    public Texture2D generate_flat_height_map()
    {
        Texture2D height_map = new Texture2D(1000, 1000);
        Color white = Color.white;
        for (int i = 0; i < 1000; i++)
            for (int j = 0; j < 1000; j++)
                height_map_.SetPixel(i, j, white);
        return height_map;
    }

    public void generate_little_ocean()
    {

        ocean_vertices_.Add(new Vector3(0.01f, 0.01f, 0.01f));
        ocean_vertices_.Add(new Vector3(0.01f, 0.01f, 0.02f));
        ocean_vertices_.Add(new Vector3(0.01f, -0.01f, 0.01f));

        ocean_uvs_.Add(new Vector2(0.0f, 0.0f));
        ocean_uvs_.Add(new Vector2(0.0f, 0.0f));
        ocean_uvs_.Add(new Vector2(0.0f, 0.0f));

        ocean_triangles_.Add(new Triangle(0, 1, 2));
    }
    #endregion


    
    #region Generator
    public void generate_hot()
    {
        is_star_            = false;
        //generate mesh and texture
        is_terestrial_                          = true;
        perlin_generator_.octaves_count_        = UnityEngine.Random.Range(3, 8);
        perlin_generator_.granulation_height_   = UnityEngine.Random.Range(0.7f, 1.5f);
        perlin_generator_.exponential_mode_on_  = false;
        perlin_generator_.one_minus_abs_on_     = false;
        water_height_                           = UnityEngine.Random.Range(0.2f, 0.4f);
        height_factor_                          = UnityEngine.Random.Range(0.2f, 1.37f);
        temperature_                            = 1.0f;
        height_map_                             = perlin_generator_.generate_2d_noise();
        
        apply_textures();

        //set up animation
        wave_amplitude_     = UnityEngine.Random.Range(0.001f , 0.01f);
        wave_height_        = UnityEngine.Random.Range(1      , 20);

        create_mesh();
    }

    public void generate_terra()
    {
        is_star_            = false;
        //generate mesh and texture
        is_terestrial_                          = true;
        perlin_generator_.octaves_count_        = UnityEngine.Random.Range(6, 8);
        perlin_generator_.granulation_height_   = UnityEngine.Random.Range(0.7f, 1.2f);
        perlin_generator_.exponential_mode_on_  = true;
        perlin_generator_.one_minus_abs_on_     = true;
        water_height_                           = UnityEngine.Random.Range(0.125f, 0.2f);
        height_factor_                          = UnityEngine.Random.Range(0.125f, 0.27f);
        temperature_                            = UnityEngine.Random.Range(0.0f, 0.9f);
        
        height_map_                             = perlin_generator_.generate_2d_noise();
        apply_textures();
        //set up animation
        wave_amplitude_     = UnityEngine.Random.Range(0.01f , 0.02f);
        wave_height_        = UnityEngine.Random.Range(1      , 20);


        create_mesh();
    }

    public void generate_sun()
    {
        //generate mesh and texture
        is_star_        = true;
        height_map_     = generate_flat_height_map();
        apply_textures();
        
        //set up animation
        wave_amplitude_     = UnityEngine.Random.Range(0.002f , 0.05f);
        wave_height_        = UnityEngine.Random.Range(1      , 20);
        
        
        is_star_            = false;

        create_mesh();
    }
    #endregion
    
}
