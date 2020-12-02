using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
    protected Vector3 mP1 = Vector3.zero;
    protected Vector3 mP2 = Vector3.one;
    protected Vector3 mV;
    protected float mL;

    private void Start() => ComputeLineDetails();

    public virtual void SetEndPoints(Vector3 p1, Vector3 P2)
    {
        mP1 = p1;
        mP2 = P2;
        ComputeLineDetails();
    }

    public void SetWidth(float w)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = localScale.z = w;
        transform.localScale = localScale;
    }

    public float GetLineLength() => mL;

    public Vector3 GetLineDir() => mV;

    public Vector3 GetStartPos() => mP1;

    public Vector3 GetEndPos() => mP2;

    public float DistantToPoint(Vector3 p, out Vector3 ptOnLine)
    {
        Vector3 lhs = p - mP1;
        float num1 = Vector3.Dot(lhs, mV);
        ptOnLine = Vector3.zero;
        float num2;
        if (num1 < 0.0 || num1 > mL)
        {
            num2 = -1f;
        }
        else
        {
            num2 = Mathf.Sqrt(lhs.sqrMagnitude - num1 * num1);
            ptOnLine = mP1 + num1 * mV;
        }
        return num2;
    }

    protected void ComputeLineDetails()
    {
        mV = mP2 - mP1;
        mL = mV.magnitude;
        mV /= mL;
        Vector3 localScale = transform.localScale;
        localScale.y = mL / 2f;
        transform.localScale = localScale;
        transform.localRotation = Quaternion.FromToRotation(Vector3.up, mV);
        transform.localPosition = mP1 + mV * (mL / 2f);
    }
}
