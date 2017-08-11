using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))] //only for debug
public class Perlin_Noise : MonoBehaviour
{
    public int         octaves_count_       = 8;
    public float       granulation_height_  = 1.0f;
    
    public bool        exponential_mode_on_ = true;
    public bool        one_minus_abs_on_    = false;


    public int         height_         = 1000;
    public int         width_          = 1000;

    public Texture2D   noise_map_;

    public Texture2D generate_2d_noise()
    {
        noise_map_              = new Texture2D(height_, width_);


        List<Texture2D> octaves = new List<Texture2D>();


        //fill octaves with smoothed noise
        for (int i = 0; i < octaves_count_; i++)
        {
            int random_map_h        = (height_ / (int)Math.Pow(2, i)) + 2;
            int random_map_w        = (width_  / (int)Math.Pow(2, i)) + 2;
            Texture2D random_map    = new Texture2D(random_map_w, random_map_h);

            fill_with_noise(random_map, random_map_w, random_map_h);
            
            smoothing(random_map, random_map_w, random_map_h);

            octaves.Add(random_map);
        }



        //calculate the final result from the octaves
        for (int h = 0; h < height_; h++)
        {
            for (int w = 0; w < width_; w++)
            {
                float gray_scale = 0.0f;

                //set each pixel to a partial amount of each octaves pixel
                for (int i = 0; i < octaves.Count; i++)
                {
                    if (!exponential_mode_on_)
                        gray_scale += (octaves[octaves.Count - 1 - i].GetPixelBilinear( 
                                                                                (w + 1) / (float)width_, 
                                                                                (h + 1) / (float)height_).grayscale 
                                                                                                                / ((octaves_count_* i + 1) * granulation_height_));
                    else
                        gray_scale += (octaves[octaves.Count - 1 - i].GetPixelBilinear(
                                                                                (w + 1) / (float)width_, 
                                                                                (h + 1) / (float)height_).grayscale 
                                                                                                                / ((float)Math.Pow(2, i) * granulation_height_));
                }
                Color pixel_color = new Color(gray_scale, gray_scale, gray_scale);
                noise_map_.SetPixel(w, h, pixel_color);
            }
        }


        //will amplify the value of the noise
        if (one_minus_abs_on_)
        {
            for (int h = 0; h < height_; h++)
            {
                for (int w = 0; w < width_; w++)
                {
                    float gray_scale = Mathf.Pow(noise_map_.GetPixel(w, h).grayscale, 8);
                    Color pixel_color = new Color(gray_scale, gray_scale, gray_scale);
                    noise_map_.SetPixel(w, h, pixel_color);
                }
            }
        }

        noise_map_.Apply();

        return noise_map_;
    }

    public void smoothing(Texture2D noise_map, int width, int height)
    {
        for (int h = 1; h < height - 1; h++)
        {
            for (int w = 1; w < width - 1; w++)
            {
                float gray_scale = (  noise_map.GetPixel(w, h).grayscale 
                                    / 4
                                    + (noise_map.GetPixel(w - 1, h    ).grayscale
                                     + noise_map.GetPixel(w + 1, h    ).grayscale
                                     + noise_map.GetPixel(w    , h - 1).grayscale
                                     + noise_map.GetPixel(w    , h + 1).grayscale)
                                    / 8
                                    + (noise_map.GetPixel(w - 1, h - 1).grayscale
                                     + noise_map.GetPixel(w - 1, h + 1).grayscale
                                     + noise_map.GetPixel(w + 1, h - 1).grayscale
                                     + noise_map.GetPixel(w + 1, h + 1).grayscale)
                                    / 16);
                Color pixel_color = new Color(gray_scale, gray_scale, gray_scale);
                noise_map.SetPixel(w, h, pixel_color);
            }
        }
        noise_map.Apply();
    }

    public void fill_with_noise(Texture2D noise_map, int width, int height)
    {
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                float   gray_scale      = UnityEngine.Random.Range(0.0f, 1.0f);
                Color   pixel_color     = new Color(gray_scale, gray_scale, gray_scale);
                noise_map.SetPixel(w, h, pixel_color);
            }
        }
        noise_map.Apply();
    }

    public void generate_mesh()
    {
        Vector3 p0 = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 p1 = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 p2 = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 p3 = new Vector3(1.0f, 0.0f, 1.0f);

        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);
        vertices.Add(p3);

        List<int> indices = new List<int>();

        indices.Add(0);
        indices.Add(1);
        indices.Add(2);

        indices.Add(2);
        indices.Add(1);
        indices.Add(3);

        List<Vector2> uvs = new List<Vector2>();
        uvs.Add(new Vector2(0.0f, 0.0f));
        uvs.Add(new Vector2(0.0f, 1.0f));
        uvs.Add(new Vector2(1.0f, 0.0f));
        uvs.Add(new Vector2(1.0f, 1.0f));


        Mesh mesh;
        MeshFilter mesh_filter;
        mesh_filter = GetComponent<MeshFilter>();
        mesh = mesh_filter.sharedMesh;

        if (mesh == null)
            mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.uv = uvs.ToArray();

        mesh_filter.mesh = mesh;


        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        if (mesh_renderer.sharedMaterial == null)
            mesh_renderer.sharedMaterial = new Material(Shader.Find("Standard"));


        if (noise_map_ != null)
            mesh_renderer.sharedMaterial.mainTexture = noise_map_;
    }

    #region Unity
    public void Reset()
    {
        generate_mesh();
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
