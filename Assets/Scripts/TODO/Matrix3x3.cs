using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Matrix3x3
{
    public float m00;
    public float m01;
    public float m02;
    public float m10;
    public float m11;
    public float m12;
    public float m20;
    public float m21;
    public float m22;

    public Matrix3x3(
      float m00,
      float m01,
      float m02,
      float m10,
      float m11,
      float m12,
      float m20,
      float m21,
      float m22)
    {
        this.m00 = m00;
        this.m01 = m01;
        this.m02 = m02;
        this.m10 = m10;
        this.m11 = m11;
        this.m12 = m12;
        this.m20 = m20;
        this.m21 = m21;
        this.m22 = m22;
    }

    public Matrix3x3(Matrix3x3 m)
    {
        m00 = m.m00;
        m10 = m.m10;
        m20 = m.m20;
        m01 = m.m01;
        m11 = m.m11;
        m21 = m.m21;
        m02 = m.m02;
        m12 = m.m12;
        m22 = m.m22;
    }

    public static Vector2 MultiplyVector2(Matrix3x3 m1, Vector2 inVector) => new Vector2()
    {
        x = m1.m00 * inVector.x + m1.m01 * inVector.y + m1.m02,
        y = m1.m10 * inVector.x + m1.m11 * inVector.y + m1.m12
    };

    public static Vector3 MultiplyVector3(Matrix3x3 m1, Vector3 inVector) => new Vector3()
    {
        x = m1.m00 * inVector.x + m1.m01 * inVector.y + m1.m02,
        y = m1.m10 * inVector.x + m1.m11 * inVector.y + m1.m12,
        z = inVector.z
    };

    public static Matrix3x3 MultiplyMatrix3x3(Matrix3x3 m1, Matrix3x3 m2) => new Matrix3x3()
    {
        m00 = m1.m00 * m2.m00 + m1.m10 * m2.m01 + m1.m20 * m2.m02,
        m10 = m1.m00 * m2.m10 + m1.m10 * m2.m11 + m1.m20 * m2.m12,
        m20 = m1.m00 * m2.m20 + m1.m10 * m2.m21 + m1.m20 * m2.m22,
        m01 = m1.m01 * m2.m00 + m1.m11 * m2.m01 + m1.m21 * m2.m02,
        m11 = m1.m01 * m2.m10 + m1.m11 * m2.m11 + m1.m21 * m2.m12,
        m21 = m1.m01 * m2.m20 + m1.m11 * m2.m21 + m1.m21 * m2.m22,
        m02 = m1.m02 * m2.m00 + m1.m12 * m2.m01 + m1.m22 * m2.m02,
        m12 = m1.m02 * m2.m10 + m1.m12 * m2.m11 + m1.m22 * m2.m12,
        m22 = m1.m02 * m2.m20 + m1.m12 * m2.m21 + m1.m22 * m2.m22
    };

}
