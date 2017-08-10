using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain_Generator
{
    public Material     main_material_;
    Texture2D           height_map_;


    Material    snow_1_;
    Material    snow_2_;
    Material    ground_;
    Material    sand_;
    Material    savannah_;
    Material    woodland_;
    Material    mars_;
    Material    hill_;
    Material    gray_mountain_;
    Material    saturn_;
    Material    red_mountain_1_;
    Material    red_mountain_2_;
    Material    water_;
    Material    grass_;
    Material    red_sand_;

    public enum PLANET_TYPE : int // not used at the moment
    {
        TERRA,
        COMET,
        STAR
    }

    public float       temperature_        = 0.8f;
    public float       grass_begin_        = 0.09f;
    public float       hill_begin_         = 0.7f;
    public float       mountain_begin_     = 0.8f;
    public float       glacier_begin_      = 0.9f;

    public bool        is_terestrial_      = true;
    public float       smoothness_         = 0.05f;
    
    public void load_materials()
    {
        snow_1_         = Resources.Load("Materials/snow_1", typeof(Material)) as Material;
        snow_2_         = Resources.Load("Materials/snow_2", typeof(Material)) as Material;
        //ground_         = Resources.Load("Materials/ground", typeof(Material)) as Material;
        sand_           = Resources.Load("Materials/sand", typeof(Material)) as Material;
        //savannah_       = Resources.Load("Materials/savannah", typeof(Material)) as Material;
        woodland_       = Resources.Load("Materials/woodland", typeof(Material)) as Material;
        mars_           = Resources.Load("Materials/mars", typeof(Material)) as Material;
        hill_           = Resources.Load("Materials/hill", typeof(Material)) as Material;
        gray_mountain_  = Resources.Load("Materials/gray_mountain", typeof(Material)) as Material;
        saturn_         = Resources.Load("Materials/saturn", typeof(Material)) as Material;
        red_mountain_1_ = Resources.Load("Materials/red_mountain_1", typeof(Material)) as Material;
        //red_mountain_2_ = Resources.Load("Materials/red_mountain_2", typeof(Material)) as Material;
        //water_          = Resources.Load("Materials/water", typeof(Material)) as Material;
        grass_          = Resources.Load("Materials/grass", typeof(Material)) as Material;
        red_sand_       = Resources.Load("Materials/red_sand", typeof(Material)) as Material;
    }

    public Material generate_material(
                            Texture2D height_map, 
                            float temperature   ,
                            float grass_begin   ,
                            float hill_begin    ,
                            float mountain_begin,
                            float glacier_begin ,
                            PLANET_TYPE type    ,
                            float smoothness    )
    {
        load_materials();



        height_map_         = height_map;
        temperature_        = temperature;
        grass_begin_        = grass_begin;
        hill_begin_         = hill_begin;
        mountain_begin_     = mountain_begin;
        glacier_begin_      = glacier_begin;
        smoothness_         = smoothness;

        if (temperature > 0.33f)
        {
            woodland_       = grass_;
        }
        if (temperature > 0.66f)
        {
            woodland_       = red_sand_;
            sand_           = red_sand_;
            hill_           = mars_;
            gray_mountain_  = saturn_;
            snow_1_         = saturn_;
        }
        if (temperature == 1.0f)
        {
            snow_1_         = saturn_;
            gray_mountain_  = red_mountain_1_;
            hill_           = red_mountain_1_;
            woodland_       = mars_;
            sand_           = saturn_;
        }




        if (type == PLANET_TYPE.TERRA)
            is_terestrial_ = true;

        main_material_      = new Material(Shader.Find("Standard"));
        generate_texture();

        return main_material_;
    }

    private void generate_texture()
    {
        Texture2D main_diffuse              = new Texture2D(height_map_.width, height_map_.height);
        
        Texture2D main_ambient_occlusion    = new Texture2D(height_map_.width, height_map_.height);

        Texture2D main_normal               = new Texture2D(height_map_.width, height_map_.height);

        
        for (int h = 0; h < height_map_.height; h++)
        {
            for (int w = 0; w < height_map_.width; w++)
            {
                List<Color> new_pixel = new List<Color>();
                new_pixel = get_color(w, h);
                main_diffuse                        .SetPixel(w, h, new_pixel[0]);

                main_ambient_occlusion              .SetPixel(w, h, new_pixel[1]);

                main_normal                         .SetPixel(w, h, new_pixel[2]);

            }
        }
        main_diffuse            .Apply();
        main_ambient_occlusion  .Apply();
        main_normal             .Apply();
        main_material_.SetTexture("_MainTex"        , main_diffuse);
        main_material_.SetTexture("_OcclusionMap"   , main_ambient_occlusion);
        main_material_.SetTexture("_BumpMap"        , main_normal);
    }


    public List<Color> get_color(int  w, int h)
    {
        
        Texture2D diffuse;
        Texture2D ambient_occlusion;
        Texture2D normal;

        float height            = 1.0f - height_map_.GetPixel(w, h).grayscale;
        
        if (height > glacier_begin_)
        {
            diffuse             = snow_1_.GetTexture("_MainTex") as Texture2D;
            ambient_occlusion   = snow_1_.GetTexture("_OcclusionMap") as Texture2D;
            normal              = snow_1_.GetTexture("_BumpMap") as Texture2D;
        }
        else if (height > mountain_begin_)
        {
            diffuse             = gray_mountain_.GetTexture("_MainTex") as Texture2D;
            ambient_occlusion   = gray_mountain_.GetTexture("_OcclusionMap") as Texture2D;
            normal              = gray_mountain_.GetTexture("_BumpMap") as Texture2D;
        }
        else if (height > hill_begin_)
        {
            diffuse             = hill_.GetTexture("_MainTex") as Texture2D;
            ambient_occlusion   = hill_.GetTexture("_OcclusionMap") as Texture2D;
            normal              = hill_.GetTexture("_BumpMap") as Texture2D;
        }
        else if (height > grass_begin_)
        {
            diffuse             = woodland_.GetTexture("_MainTex") as Texture2D;
            ambient_occlusion   = woodland_.GetTexture("_OcclusionMap") as Texture2D;
            normal              = woodland_.GetTexture("_BumpMap") as Texture2D;
        }
        else
        {
            diffuse             = sand_.GetTexture("_MainTex") as Texture2D;
            ambient_occlusion   = sand_.GetTexture("_OcclusionMap") as Texture2D;
            normal              = sand_.GetTexture("_BumpMap") as Texture2D;
        }




        List<Color> new_pixels = new List<Color>();

        Color new_pixel     = new Color();

        if (is_terestrial_)
        {
            new_pixel       = diffuse.GetPixelBilinear(w / (float)height_map_.width, h / (float)height_map_.height);
            new_pixels      .Add(new_pixel);
            new_pixel       = ambient_occlusion.GetPixelBilinear(w / (float)height_map_.width, h / (float)height_map_.height);
            new_pixels      .Add(new_pixel);
            new_pixel       = normal.GetPixelBilinear(w / (float)height_map_.width, h / (float)height_map_.height);
            new_pixels      .Add(new_pixel);
        


            //north / south pole
            float north_pole_line = 1 - ((1 - temperature_) / 2);
            float south_pole_line = (1 - temperature_) / 2;
            
            float y_height        = h / (float)height_map_.height;
            if ((y_height > north_pole_line) || (y_height  < south_pole_line))
            {
                Texture2D snow_diffuse              = snow_2_.GetTexture("_MainTex") as Texture2D;
                Texture2D snow_ambient_occlusion    = snow_2_.GetTexture("_OcclusionMap") as Texture2D;
                Texture2D snow_normal               = snow_2_.GetTexture("_BumpMap") as Texture2D;
                //near pole full texture
                if ((y_height > north_pole_line + smoothness_) || (y_height < south_pole_line - smoothness_))
                {
                    new_pixels[0]       = snow_diffuse              .GetPixelBilinear(w / (float)height_map_.width, h / (float)height_map_.height);
                    new_pixels[1]       = snow_ambient_occlusion    .GetPixelBilinear(w / (float)height_map_.width, h / (float)height_map_.height);
                    new_pixels[2]       = snow_normal               .GetPixelBilinear(w / (float)height_map_.width, h / (float)height_map_.height);
                }
                else // interpolate north
                {
                    float pole_distance = (y_height > 0.5 ? (y_height - north_pole_line) : (south_pole_line - y_height));
                    float smooth_factor = pole_distance * (1.0f / smoothness_);
                    new_pixels[0]       = Color.Lerp(
                                                      new_pixels[0],
                                                      snow_diffuse.GetPixel(w % snow_diffuse.width, h % snow_diffuse.height),
                                                      smooth_factor);
                    new_pixels[1]       = Color.Lerp(
                                                      new_pixels[1],
                                                      snow_ambient_occlusion.GetPixel(w % snow_ambient_occlusion.width, h % snow_ambient_occlusion.height),
                                                      smooth_factor);
                    new_pixels[2]       = Color.Lerp(
                                                      new_pixels[2],
                                                      snow_normal.GetPixel(w % snow_normal.width, h % snow_normal.height),
                                                      smooth_factor);
                }
            }
        }
        return new_pixels;
    }
}
