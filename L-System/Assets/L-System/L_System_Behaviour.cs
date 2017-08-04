using UnityEngine;
using System.Collections;

public class L_System_Behaviour : MonoBehaviour 
{
    public L_System         l_sys_              = new L_System();
    public Turtle_Renderer  turtle_renderer_    = new Turtle_Renderer();

    public void OnDrawGizmos()
    {
        string      generated_string    = l_sys_.generate();
        
        turtle_renderer_.process        (generated_string);
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
