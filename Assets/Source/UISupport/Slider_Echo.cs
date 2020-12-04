using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_Echo : MonoBehaviour
{
    public Text value;

    // Update is called once per frame
    void Update()
    {
        var slider = GetComponent<Slider>();
        float v = slider.value;
        value.text = v.ToString();
    }
}
