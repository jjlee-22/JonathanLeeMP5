using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Matrix3x3Helpers : MonoBehaviour
{
    public static Matrix3x3 CreateTranslation(Vector2 translation) => new Matrix3x3()
    {
        m00 = 1f,
        m11 = 1f,
        m22 = 1f,
        m02 = translation.x,
        m12 = translation.y
    };

    public static Matrix3x3 CreateTRS(Vector2 translation, float rotation, Vector2 scale)
    {
        float num1 = Mathf.Cos(rotation * ((float)Math.PI / 180f));
        float num2 = Mathf.Sin(rotation * ((float)Math.PI / 180f));
        
        return new Matrix3x3()
        {
            m00 = scale.x * num1,
            m01 = scale.x * -num2,
            m02 = translation.x,
            m10 = scale.y * num2,
            m11 = scale.y * num1,
            m12 = translation.y,

            m20 = 0.0f,
            m21 = 0.0f,
            m22 = 1f
        };
    }
}
