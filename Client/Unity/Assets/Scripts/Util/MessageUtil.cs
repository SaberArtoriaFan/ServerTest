using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class MessageUtil
{
    public static Vector3 ToUnity(this V3 data)
    {
        return new Vector3(data.x, data.y, data.z);
    }
    public static Quaternion ToUnity(this V4 data)
    {
        return new Quaternion(data.x,data.y,data.z,data.w);
    }
    public static V3 ToMessage(this Vector3 data)
    {
        return new V3() 
        {
            x = data.x,
            y = data.y,
            z = data.z
        };
    }
    public static V4 ToMessage(this Quaternion data)
    {
        return new V4() { 
            x= data.x,
            y= data.y,
            z= data.z,
            w= data.w
        };
    }
}

