using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class XFormControl : MonoBehaviour
{
    public Toggle T;
    public Toggle R;
    public Toggle S;
    public SliderWithEcho X;
    public SliderWithEcho Y;
    public SliderWithEcho Z;
    public Text ObjectName;
    private Transform mSelected;
    private Vector3 mPreviousSliderValues = Vector3.zero;

    private void Start()
    {
        T.onValueChanged.AddListener(SetToTranslation);
        R.onValueChanged.AddListener(SetToRotation);
        S.onValueChanged.AddListener(SetToScaling);
        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        Z.SetSliderListener(ZValueChanged);
        T.isOn = true;
        R.isOn = false;
        S.isOn = false;
        SetToTranslation(true);
    }

    private void SetToTranslation(bool v)
    {
        if (!v)
            return;
        Vector3 selectedXformParameter = GetSelectedXformParameter();
        mPreviousSliderValues = selectedXformParameter;
        X.InitSliderRange(-4f, 4f, selectedXformParameter.x);
        Y.InitSliderRange(-4f, 4f, selectedXformParameter.y);
        Z.InitSliderRange(0.0f, 0.0f, selectedXformParameter.z);
    }

    private void SetToScaling(bool v)
    {
        if (!v)
            return;
        Vector3 selectedXformParameter = GetSelectedXformParameter();
        mPreviousSliderValues = selectedXformParameter;
        X.InitSliderRange(0.1f, 10f, selectedXformParameter.x);
        Y.InitSliderRange(0.1f, 10f, selectedXformParameter.y);
        Z.InitSliderRange(1f, 1f, selectedXformParameter.z);
    }

    private void SetToRotation(bool v)
    {
        if (!v)
            return;
        Vector3 selectedXformParameter = GetSelectedXformParameter();
        mPreviousSliderValues = selectedXformParameter;
        X.InitSliderRange(0.0f, 0.0f, selectedXformParameter.x);
        Y.InitSliderRange(0.0f, 0.0f, selectedXformParameter.y);
        Z.InitSliderRange(-180f, 180f, selectedXformParameter.z);
        mPreviousSliderValues = selectedXformParameter;
    }

    private void XValueChanged(float v)
    {
        Vector3 selectedXformParameter = GetSelectedXformParameter();
        float angle = v - mPreviousSliderValues.x;
        mPreviousSliderValues.x = v;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.right);
        selectedXformParameter.x = v;
        SetSelectedXform(ref selectedXformParameter, ref q);
    }

    private void YValueChanged(float v)
    {
        Vector3 selectedXformParameter = GetSelectedXformParameter();
        float angle = v - mPreviousSliderValues.y;
        mPreviousSliderValues.y = v;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        selectedXformParameter.y = v;
        SetSelectedXform(ref selectedXformParameter, ref q);
    }

    private void ZValueChanged(float v)
    {
        Vector3 selectedXformParameter = GetSelectedXformParameter();
        float angle = v - mPreviousSliderValues.z;
        mPreviousSliderValues.z = v;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        selectedXformParameter.z = v;
        SetSelectedXform(ref selectedXformParameter, ref q);
    }

    public void SetSelectedObject(Transform g)
    {
        mSelected = g;
        mPreviousSliderValues = Vector3.zero;
        ObjectName.text = !(g != null) ? "Selected: none" : "Selected:" + g.name;
        ObjectSetUI();
    }

    public void ObjectSetUI()
    {
        Vector3 selectedXformParameter = GetSelectedXformParameter();
        X.SetSliderValue(selectedXformParameter.x);
        Y.SetSliderValue(selectedXformParameter.y);
        Z.SetSliderValue(selectedXformParameter.z);
    }

    private Vector3 GetSelectedXformParameter() => !T.isOn ? (!S.isOn ? Vector3.zero : (!(mSelected != null) ? Vector3.one : mSelected.localScale)) : (!(mSelected != null) ? Vector3.zero : mSelected.localPosition);

    private void SetSelectedXform(ref Vector3 p, ref Quaternion q)
    {
        if (mSelected == null)
            return;
        if (T.isOn)
            mSelected.localPosition = p;
        else if (S.isOn)
            mSelected.localScale = p;
        else
            mSelected.localRotation *= q;
    }
}
