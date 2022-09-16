using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Networking
{
    public static class Extensions
    {
        public static void Put(this NetDataWriter writer, Vector2 vector)
        {
            writer.Put(vector.x);
            writer.Put(vector.y);
        }

        public static void Put(this NetDataWriter writer, Vector3 vector)
        {
            writer.Put(vector.x);
            writer.Put(vector.y);
            writer.Put(vector.z);
        }

        public static void Put(this NetDataWriter writer, Quaternion quaternion)
        {
            writer.Put(quaternion.x);
            writer.Put(quaternion.y);
            writer.Put(quaternion.z); 
            writer.Put(quaternion.w); 
        }

        public static Vector3 GetVector2(this NetDataReader reader)
        {
            Vector3 v = new Vector3();
            v.x = reader.GetFloat();
            v.y = reader.GetFloat();
            return v;
        }

        public static Vector3 GetVector3(this NetDataReader reader)
        {
            Vector3 v = new Vector3();
            v.x = reader.GetFloat();
            v.y = reader.GetFloat(); 
            v.z = reader.GetFloat();
            return v;
        }

        public static Quaternion GetQuaternion(this NetDataReader reader)
        {
            Quaternion v = new Quaternion(); 
            v.x = reader.GetFloat();
            v.y = reader.GetFloat();
            v.z = reader.GetFloat();
            v.w = reader.GetFloat();
            return v;
        }
    }
}
