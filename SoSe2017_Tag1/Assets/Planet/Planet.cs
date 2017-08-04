using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Planet : MonoBehaviour
{
    public float        height_        = 10.0f;
    public float        width_         = 10.0f;
    
    public int          width_points_count_  = 20;
    public int          height_points_count_ = 20;

    public Vector3      center_;

    public float        fold_out_sphere_ = 1.0f;


    public float        time_counter_   = 0.0f;
    public float        amplitude_      = 1.0f;


    List<Vector3>   vertices_           = new List<Vector3>();
    List<Vector3>   sphere_vertices_    = new List<Vector3>();
    List<int>       triangles_          = new List<int>();
    List<Vector2>   uvs_                = new List<Vector2>();
    List<Vector3>   fold_out_vertices_  = new List<Vector3>();

    Vector3[]       vertices_with_height_;
    Vector3[]       sphere_vertices_with_height_;



    #region mesh stuff
    MeshFilter      mesh_filter_;
    MeshRenderer    mesh_renderer_;
    Mesh            mesh_;
    #endregion



    public Texture2D main_texture_;
    public Texture2D height_map_;

    public void Reset()
    {
        create_mesh();
    }


    private void setup_mesh()
    {
        mesh_filter_        = GetComponent<MeshFilter>();
        mesh_renderer_      = GetComponent<MeshRenderer>();
        mesh_               = mesh_filter_.sharedMesh;

        if (mesh_ == null)
            mesh_ = new Mesh();

        vertices_           .Clear();
        sphere_vertices_    .Clear();
        triangles_          .Clear();
        uvs_                .Clear();
        fold_out_vertices_  .Clear();
    }

    private void generate_uvs_and_vertices()
    {
        float widht_step            = width_  / (width_points_count_  - 1);
        float height_step           = height_ / (height_points_count_ - 1);
        float widht_step_sphere     = 360.0f  / (float) (width_points_count_ - 1);
        float height_step_sphere    = 180.0f  / (float) (height_points_count_ - 1);
        
        
        
        float widht_parameter       = 1.0f    / (width_points_count_  - 1);
        float height_parameter      = 1.0f    / (height_points_count_ - 1);

        for (int w = 0; w < width_points_count_; w ++)
        {
            for (int h = 0; h < height_points_count_; h ++)
            {
                //grid
                vertices_.Add(new Vector3(
                                        w * widht_step, 
                                        0.0f, 
                                        h * height_step));
                
                Vector3 sphere_point =  point_on_sphere(w * widht_step_sphere,
                                                        h * height_step_sphere,
                                                        height_ / 2,
                                                        center_);

                Vector3 rotated_sphere_point = rotate(
                                                      sphere_point,
                                                      center_,
                                                      new Vector3(90.0f, 0.0f, 0.0f));
                sphere_vertices_.Add(rotated_sphere_point);

                //texture uvs
                uvs_.Add(new Vector2(
                                    w * widht_parameter, 
                                    h * height_parameter));

            }
        }
    }

    private void generate_triangles()
    {
        /* p01__________p11
         * |            |
         * |            |
         * |____________|
         * p00          p10
        */


        for (int w = 0; w < width_points_count_ - 1; w++)
        {
            for (int h = 0; h < height_points_count_ - 1; h++)
            {
                int p00 = w        * height_points_count_ + h    ;
                int p01 = w        * height_points_count_ + h + 1;
                int p10 = (w + 1)  * height_points_count_ + h    ;
                int p11 = (w + 1)  * height_points_count_ + h + 1;

                triangles_.Add(p00);
                triangles_.Add(p01);
                triangles_.Add(p10);

                triangles_.Add(p01);
                triangles_.Add(p11);
                triangles_.Add(p10);
            }
        }
    }

    private void fold_out_vertices()
    {
        fold_out_vertices_ = new List<Vector3>(vertices_);
        for (int i = 0; i < vertices_.Count; i++)
        {
            fold_out_vertices_[i] = Vector3.Lerp(
                                            vertices_with_height_[i], 
                                            sphere_vertices_with_height_[i], 
                                            fold_out_sphere_);
        }
    }

    public void create_mesh()
    {

        setup_mesh();

        generate_uvs_and_vertices();

        generate_triangles();


        mesh_.Clear             ();
        mesh_.SetVertices       (vertices_);
        mesh_.triangles         = triangles_.ToArray();
        mesh_.uv                = uvs_.ToArray();


        apply_height_map();

        fold_out_vertices();

        mesh_.Clear();
        mesh_.SetVertices       (fold_out_vertices_);
        mesh_.triangles         = triangles_.ToArray();
        mesh_.uv                = uvs_.ToArray();

        mesh_.RecalculateNormals();

        mesh_filter_.sharedMesh = mesh_;

        apply_main_texture();
    }

    public Vector3 point_on_sphere(float lon, float lat, float radius, Vector3 center)
    {
        //longitude --> ost west
        //latitude  --> nord sued

        float lon_rad = lon * Mathf.Deg2Rad;
        float lat_rad = lat * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(lon_rad) * Mathf.Sin(lat_rad);
        float y = radius * Mathf.Sin(lon_rad) * Mathf.Sin(lat_rad);
        float z = radius * Mathf.Cos(lat_rad);

        return new Vector3(x, y, z) + center;
    }

    public Vector3 rotate(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3     direction   = point - pivot;
                    direction   = Quaternion.Euler(angles) * direction;
        return pivot + direction;


    }

    public void apply_main_texture()
    {
        mesh_renderer_                      = GetComponent<MeshRenderer>();
        if (mesh_renderer_.sharedMaterial == null)
            mesh_renderer_.sharedMaterial   = new Material(Shader.Find("Standard"));

        if (main_texture_ != null)
            mesh_renderer_.sharedMaterial.mainTexture = main_texture_;
    }

    public void apply_height_map()
    {
        if (height_map_ == null)
        {
            // workaround
            height_map_ = Resources.Load("heightmap") as Texture2D;
            //return;
        }


        mesh_filter_                            = GetComponent<MeshFilter>();
        
        Mesh mesh                               = mesh_filter_.sharedMesh;

        
        vertices_with_height_                   = vertices_.ToArray();
        sphere_vertices_with_height_            = sphere_vertices_.ToArray();

        Vector2[] uvs                           = mesh.uv;

        for (int i = 0; i < sphere_vertices_with_height_.Length; i++)
        {
            Vector2 uv                          = uvs[i];

            Color height_color                  = height_map_.GetPixelBilinear(uv.x, uv.y);
            float height                        = height_color.grayscale * amplitude_;

            vertices_with_height_[i]             = new Vector3 (
                                                            vertices_[i].x,
                                                            height,
                                                            vertices_[i].z);


            Vector3 position                 =  sphere_vertices_[i];
            Vector3 height_direction         =  (position - center_).normalized;
            sphere_vertices_with_height_[i]   += height_direction * height;
        }

        mesh_.vertices                          = sphere_vertices_with_height_;
    }

    #region unity methods
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time_counter_ += Time.deltaTime;
        transform.Rotate(Vector3.up * Time.deltaTime * 20.0f); ;
    }
    #endregion





    //NOISE
    public Texture2D    noise_texture_;
    public int          texture_width_           = 1024;
    public int          texture_height_          = 512;
    public float        noise_intensity_         = 10.0f;

    public Texture2D    grass_text_;
    public Texture2D    rock_text_;
    public Texture2D    water_text_;
    
    public float        water_cut_off_           = 0.3f;
    public float        grass_cut_off_           = 0.5f;


    public void create_noise_texture()
    {
        noise_texture_      = new Texture2D(texture_width_, texture_height_);
        noise_texture_.name = "Noise Texture";

        for (int w = 0; w < texture_width_; w++)
        {
            for (int h = 0; h < texture_height_; h++)
            {
                float gray_value = Mathf.PerlinNoise(
                                            (w * noise_intensity_) / texture_width_,
                                            (h * noise_intensity_) / texture_height_);

                noise_texture_.SetPixel(w, h, new Color(gray_value, gray_value, gray_value));
            }
        }
        noise_texture_.Apply();
    }

    public void create_height_map_and_main_texture()
    {
        if (noise_texture_ == null)
            return;

        height_map_         = new Texture2D(texture_width_, texture_height_);
        main_texture_       = new Texture2D(texture_width_, texture_height_);
        
        grass_text_         = Resources.Load("grass") as Texture2D;
        water_text_         = Resources.Load("water") as Texture2D;
        rock_text_          = Resources.Load("stone") as Texture2D;


        for (int w = 0; w < texture_width_; w++)
        {
            for (int h = 0; h < texture_height_; h++)
            {
                Color normal_sample     = noise_texture_.GetPixel(w, h);
                if (normal_sample.grayscale < water_cut_off_)
                    height_map_.SetPixel(w, h, new Color(water_cut_off_, water_cut_off_, water_cut_off_));
                else
                    height_map_.SetPixel(w, h, normal_sample);

                if (normal_sample.grayscale < water_cut_off_)
                    main_texture_.SetPixel(w, h, water_text_.GetPixel(w % water_text_.width, h % water_text_.height));
                else
                {
                    float lerp_parameter        = 1 - (normal_sample.grayscale - grass_cut_off_) / (1.0f - grass_cut_off_);
                    lerp_parameter              = lerp_parameter - 1;
                    Color blended_grass_hill    = Color.Lerp(
                                                        grass_text_ .GetPixel(w % grass_text_.width, h % grass_text_.height),
                                                        rock_text_  .GetPixel(w % rock_text_.width , h % rock_text_.height),
                                                        lerp_parameter);
                    main_texture_.SetPixel(w, h, blended_grass_hill);
                }
            }
        }
        main_texture_.Apply();
        height_map_.Apply();
    }



}
