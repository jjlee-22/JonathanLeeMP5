using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControll : MonoBehaviour
{
    public Slider X_Slider;
    public Slider Y_Slider;
    public Slider Z_Slider;
    public GameObject lookAtPoint;
    public float zoom_sensitivity;
    public float move_sensitivity;
    Vector3 original_lookAt;
    Vector3 original_camera_position;
    Vector3 offset;
    Vector3 current_mouse;
    public static bool is_alt_down = false;
    bool is_rightpress = false;
    bool is_leftpress = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 lookAtPosition = lookAtPoint.transform.position;
        //Vector3 camera_location = this.transform.position;
        update_lookAt(lookAtPosition);
        original_lookAt = lookAtPosition;
        original_camera_position = transform.localPosition;
        current_mouse = Input.mousePosition;
        zoom_sensitivity = 10;
        move_sensitivity = 5;
        offset = lookAtPoint.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var x = X_Slider.GetComponent<Slider>();
        var y = Y_Slider.GetComponent<Slider>();
        var z = Z_Slider.GetComponent<Slider>();
        Vector3 translate_value = new Vector3(x.value, y.value, z.value);
        // Continuously apply slider values on the lookatpoint
        lookAtPoint.transform.position = original_lookAt + translate_value;
        update_lookAt(lookAtPoint.transform.position);
        //lookAtPoint.transform.position = original_lookAt;
        // Handle key press
        handle_alt();
        handle_rightClick();
        handle_leftClick();
        Vector3 mouse_move_direction = mouse_movement();
        if (is_alt_down == true) // alt is pressed
        {
            handle_wheelscroll();
            if(is_leftpress == true)
            {
                handle_tumble(mouse_move_direction);
            }
            else if (is_rightpress == true)
            {
                handle_track(mouse_move_direction);
            }
        }
        /*Vector3 rot = this.transform.rotation.eulerAngles;
        Quaternion q = new Quaternion();
        q.eulerAngles = rot + new Vector3(5, 0, 0) * Time.deltaTime;
        this.transform.rotation= q;*/
    }

    public void setOriginalLookAt(Vector3 p)
    {
        original_lookAt = p;
    }

    public void resetCamera()
    {
        transform.localPosition = original_camera_position;
    }

    void update_lookAt(Vector3 lookAtPosition)
    {
        /*Vector3 V = lookAtPosition - cameraPosition;
        this.transform.rotation = Quaternion.FromToRotation(Vector3.forward, V);
        Vector3 rot = this.transform.rotation.eulerAngles;
        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(rot.x, rot.y, 0);
        this.transform.rotation = q;*/
        Vector3 V = lookAtPosition - this.transform.localPosition;
        Vector3 W = Vector3.Cross(-V, Vector3.up);
        Vector3 U = Vector3.Cross(W, -V);
        //transform.localRotation = Quaternion.LookRotation(V, U);
        this.transform.localRotation = Quaternion.FromToRotation(Vector3.up, U);
        Quaternion alignU = Quaternion.FromToRotation(transform.forward, V);
        this.transform.localRotation = alignU * this.transform.localRotation;
    }

    void handle_wheelscroll()
    {
        this.GetComponent<Camera>().fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoom_sensitivity;
    }
    void handle_track(Vector3 d)
    {
        float dist = dis(transform.localPosition, lookAtPoint.transform.localPosition);
        Vector3 dir = lookAtPoint.transform.localPosition - transform.localPosition;
        Matrix4x4 r = Matrix4x4.TRS(transform.right * d.x * 0.5f + new Vector3(0, d.y, 0) * 0.5f, Quaternion.identity, Vector3.one);
        transform.localPosition = r.MultiplyPoint(transform.localPosition);
        //transform.localPosition += transform.right*d.x + new Vector3(0,d.y,0);
        original_lookAt = transform.localPosition + dist * dir.normalized;
    }

    void handle_tumble(Vector3 d)
    {
        Quaternion q1 = Quaternion.AngleAxis(d.x*move_sensitivity*10/60f, transform.up);
        Quaternion q2 = Quaternion.AngleAxis(-d.y*move_sensitivity*10/60f, transform.right);
        Matrix4x4 r1 = Matrix4x4.TRS(Vector3.zero, q1, Vector3.one);
        Matrix4x4 r2 = Matrix4x4.TRS(Vector3.zero, q2, Vector3.one);
        Matrix4x4 invP = Matrix4x4.TRS(-lookAtPoint.transform.localPosition, Quaternion.identity, Vector3.one);
        Matrix4x4 r = invP.inverse * r2 * r1 * invP;
        Vector3 newCameraPos = r.MultiplyPoint(transform.localPosition);
        transform.localPosition = newCameraPos;
        update_lookAt(lookAtPoint.transform.localPosition);
    }
    void handle_rightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            is_rightpress = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            is_rightpress = false;
        }
    }

    void handle_leftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            is_leftpress = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            is_leftpress = false;
        }
    }
    void handle_alt()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            is_alt_down = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            is_alt_down = false;
        }
    }
    Vector3 mouse_movement() // return mouse move direction
    {
        //Debug.Log(Input.mousePosition);
        Vector3 direction = Input.mousePosition - current_mouse;
        current_mouse = Input.mousePosition;
        return direction.normalized;
    }

    float dis(Vector3 start, Vector3 end)
    {
        float x_sqr = (end.x - start.x) * (end.x - start.x);
        float y_sqr = (end.y - start.y) * (end.y - start.y);
        float z_sqr = (end.z - start.z) * (end.z - start.z);
        return Mathf.Sqrt(x_sqr + y_sqr + z_sqr);
    }


}
