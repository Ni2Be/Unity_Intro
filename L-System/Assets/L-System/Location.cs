using UnityEngine;
using System.Collections;

public class Location
{
    public Vector3      position_;
    public Quaternion   rotation_;

    public Location()
    {
        position_   = new Vector3(0.0f, 0.0f, 0.0f);
        rotation_   = Quaternion.identity;


    }

    public Location(Location other)
    {
        position_   = other.position_;
        rotation_   = other.rotation_;
    }

    public void set_values(Location other)
    {
        position_   = other.position_;
        rotation_   = other.rotation_;
    }

    public void turn_left(float angle)
    {
        rotation_ *= Quaternion.Euler(new Vector3( 0.0f  ,-angle, 0.0f));
    }                                                    
                                                         
    public void turn_right(float angle)                  
    {                                                    
        rotation_ *= Quaternion.Euler(new Vector3( 0.0f  , angle, 0.0f));
    }                                                           
                                                                
    public void pitch_up(float angle)                           
    {                                                           
        rotation_ *= Quaternion.Euler(new Vector3(-angle , 0.0f , 0.0f));
    }                                                           
                                                                
    public void pitch_down(float angle)                         
    {                                                           
        rotation_ *= Quaternion.Euler(new Vector3( angle , 0.0f , 0.0f));
                                                                
    }                                                           
                                                                
    public void roll_left(float angle)                          
    {                                                           
        rotation_ *= Quaternion.Euler(new Vector3( 0.0f  , 0.0f , -angle));
                                                                
    }                                                           
                                                                
    public void roll_right(float angle)                         
    {                                                           
        rotation_ *= Quaternion.Euler(new Vector3( 0.0f  , 0.0f ,  angle));
                                                                
    }                                                           
                                                                
    public void turn_around()                                   
    {                                                           
        rotation_ *= Quaternion.Euler(new Vector3( 180.0f, 0.0f , 0.0f));
    }

    public void move(float distance)
    {
        Vector3 target_forward  = (rotation_ * Vector3.forward).normalized;
        position_               += target_forward * distance;
    }
}
