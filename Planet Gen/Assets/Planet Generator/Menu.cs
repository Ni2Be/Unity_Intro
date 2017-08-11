using UnityEngine;



public class Menu : MonoBehaviour
{

    public bool is_running = true;
    void Update()
    {
        if (Input.GetKey("escape") || !is_running)
            Application.Quit();
    }

    public void quit()
    {
        is_running = false;
    }
}