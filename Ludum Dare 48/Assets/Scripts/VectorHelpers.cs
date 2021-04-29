using System.Collections.Generic;
using UnityEngine;

public static class VectorHelpers
{
    public static Vector3 Add(this Vector3 vector, float x = 0, float y = 0, float z = 0)
    {
        return new Vector3(vector.x + x, vector.y + y, vector.z + z);
    }

    public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
    }

    public static Vector2 Add(this Vector2 vector, float x = 0, float y = 0)
    {
        return new Vector2(vector.x + x, vector.y + y);
    }

    public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
    {
        return new Vector2(x ?? vector.x, y ?? vector.y);
    }
}