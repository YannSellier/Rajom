using UnityEngine;

public static class UnityExtensions
{
        
    
    public const float EPSILON = .0001f;
    public static bool AlmostEqual(this float a, float b, float epsilon = EPSILON)
    {
        float diff = Mathf.Abs(a - b);
        return diff < epsilon;
    }
    public static bool AlmostEqual(this Vector3 vector3, Vector3 otherVector3, float epsilon = EPSILON)
    {
        return vector3.x.AlmostEqual(otherVector3.x, epsilon) && vector3.y.AlmostEqual(otherVector3.y, epsilon) && vector3.z.AlmostEqual(otherVector3.z, epsilon);
    }
}