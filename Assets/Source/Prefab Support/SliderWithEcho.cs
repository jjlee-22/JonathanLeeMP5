using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderWithEcho : MonoBehaviour
{
    public Slider TheSlider;
    public Text TheEcho;
    public Text TheLabel;
    private SliderWithEcho.SliderCallbackDelegate mCallBack;

    private void Start() => TheSlider.onValueChanged.AddListener((SliderValueChange));

    public void SetSliderListener(SliderWithEcho.SliderCallbackDelegate listener) => mCallBack = listener;

    private void SliderValueChange(float v)
    {
        UpdateEcho();
        if (mCallBack == null)
            return;
        mCallBack(v);
    }

    public float GetSliderValue() => TheSlider.value;

    public void SetSliderLabel(string l) => TheLabel.text = l;

    public void SetSliderValue(float v)
    {
        TheSlider.value = v;
        UpdateEcho();
    }

    public void InitSliderRange(float min, float max, float v)
    {
        TheSlider.minValue = min;
        TheSlider.maxValue = max;
        SetSliderValue(v);
    }

    private void UpdateEcho()
    {
        if (TheSlider.wholeNumbers)
            TheEcho.text = TheSlider.value.ToString("0");
        else
            TheEcho.text = TheSlider.value.ToString("0.0000");
    }

    public delegate void SliderCallbackDelegate(float v);
}

