using UnityEngine;

namespace Data
{
public static class DataExtensions
{
    public static Vector3Data AsVectorData(this Vector3 vector3) => 
        new Vector3Data(vector3.x, vector3.y, vector3.z);

    public static Vector3 AsUnityVector(this Vector3Data vector3) => 
        new Vector3(vector3.x, vector3.y, vector3.z);
    
    public static Vector3 AddY(this Vector3 vector, float y)
    {
        vector.y += y;

        return vector;
    }

    public static string ToJson(this object obj) => 
        JsonUtility.ToJson(obj);

    public static T ToDeserialized<T>(this string json) => 
        JsonUtility.FromJson<T>(json);
}
}