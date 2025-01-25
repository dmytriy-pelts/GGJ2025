using UnityEngine;

namespace GumFly.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 WithHeight(this Vector3 v, float height)
        {
            return v.WithY(height);
        }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector2 WithX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }

        public static Vector3 WithZ(this Vector2 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector3 WithHeight(this Vector2 v, float y)
        {
            return new Vector3(v.x, y, v.y);
        }

        public static Vector2 WithY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }

        public static Vector3 WithXZ(this Vector3 v, Vector2 xz)
        {
            return new Vector3(xz.x, v.y, xz.y);
        }

        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
        
        public static Vector2 XY(this Vector3 v)
        {
            return v;
        }

        public static Vector3 RotateAroundPivot(this Vector3 p, Vector3 pivot, Quaternion rotation)
        {
            return pivot + rotation * (p - pivot);
        }

        public static Pose ToPose(this Vector3 p)
        {
            return new Pose(p, Quaternion.identity);
        }

        public static float DistanceXZ(this Vector3 a, Vector3 b)
        {
            return Vector2.Distance(a.XZ(), b.XZ());
        }

        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}