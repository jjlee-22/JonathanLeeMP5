using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTRL : MonoBehaviour
{
    public bool enable = true;
    public bool is_pressed = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) == true)
        {
            is_pressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) == true)
        {
            is_pressed = false;
        }
        // Vertices toggle
        if (enable == true)
        {
            if (is_pressed == true)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach(Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
