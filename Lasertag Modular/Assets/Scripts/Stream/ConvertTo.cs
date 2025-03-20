using System.Text;
using UnityEngine;

namespace Stream
{
    public static class ConvertTo
    {
        public static byte[] ObjectToByteArray<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            string json = JsonUtility.ToJson(obj);
            return Encoding.UTF8.GetBytes(json);
        }

        public static T ByteArrayToObject<T>(byte[] data)
        {
            string json = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(json);
        }
    }
}