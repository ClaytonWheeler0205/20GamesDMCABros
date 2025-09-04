using System;
using Godot;

namespace Game.Util
{
    public static class FloatOperator
    {
        public static bool EqualityComparisonWithTolerance(float a, float b, float tolerance)
        {
            if (float.IsNaN(a) || float.IsNaN(b))
            {
                return false;
            }
            if (float.IsInfinity(a) || float.IsInfinity(b))
            {
                return a == b;
            }

            return Mathf.Abs(a - b) < tolerance;
        }
    }
}