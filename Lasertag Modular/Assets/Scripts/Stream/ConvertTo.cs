﻿using System.Text;
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
        
        public static T ByteArrayToObjectT<T>(this byte[] data)
        {
            string json = Encoding.UTF8.GetString(data).TrimEnd('\0');
            return JsonUtility.FromJson<T>(json);
        }
    }
}