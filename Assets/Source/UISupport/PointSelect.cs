using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSelect : MonoBehaviour
{
    public GameObject SelectPoint;
    bool mouse_is_pressed = false;
    float horizontal_dist = 0;
    float vertical_dist = 0;
    Vector3 pre_mouse_pos;
    GameObject selected_cylinder;
    GameObject selected_sphere;
    Color ori_color;
    void Start()
    {
        pre_mouse_pos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 current_mouse_pos = Input.mousePosition;
        horizontal_dist = current_mouse_pos.x - pre_mouse_pos.x;
        vertical_dist = current_mouse_pos.y - pre_mouse_pos.y;
        pre_mouse_pos = current_mouse_pos;
        if (Input.GetMouseButtonDown(0))
        {
            mouse_is_pressed = true;
        }
        else if (Input.GetMouseButtonUp(0)||GetComponent<CTRL>().is_pressed==false)
        {
            // Mouse release, exit transform in case there's a selected axis cylinder
            if (selected_cylinder != null)
            {
                selected_cylinder.GetComponent<Renderer>().material.color = ori_color;
                selected_cylinder = null;
            }
            mouse_is_pressed = false;
        }
        if(mouse_is_pressed==true)
        {
            // No selected axis cylinder but mouse is pressed, so treat it as a mouse click only
            if (selected_cylinder == null && CameraControll.is_alt_down==false)
            {
                ClickBehavior();
                mouse_is_pressed = false;
            }
        }
        // Has selected axis cylinder, so mouse is pressed and is being hold
        if (selected_cylinder != null)
        {
            if (selected_cylinder.name == "XAxis")
            {
                selected_sphere.transform.position += new Vector3(horizontal_dist*0.05f, 0, 0);
                
            }
            else if(selected_cylinder.name == "YAxis")
            {
                selected_sphere.transform.position += new Vector3(0, vertical_dist * 0.05f, 0);
            }
            else
            {
                selected_sphere.transform.position += new Vector3(0, 0, -horizontal_dist * 0.05f);
            }
            SelectPoint.transform.position = selected_sphere.transform.position;
        }
    }

    void ClickBehavior()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (GetComponent<CTRL>().is_pressed == true)
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Sphere" && selected_cylinder == null)
                {
                    SelectPoint.SetActive(true);
                    SelectPoint.transform.position = hit.transform.position;
                    selected_sphere = hit.collider.gameObject;
                    GetComponent<CTRL>().enable = false;
                }
                else if (hit.collider.gameObject.name == "XAxis" || hit.collider.gameObject.name == "YAxis" || hit.collider.gameObject.name == "ZAxis")
                {
                    selected_cylinder = hit.collider.gameObject;
                    ori_color = selected_cylinder.GetComponent<Renderer>().material.color;
                    selected_cylinder.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                }
            }
            else
            {
                SelectPoint.SetActive(false);
                GetComponent<CTRL>().enable = true;
            }
        }
            
    }
}
