using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class UnityUtil
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var res=gameObject.GetComponent<T>();
        if (res != null) return res;
        return gameObject.AddComponent<T>();
    }

    public static NetworkBehavior AddNetworkComponentById(this NetworkObject networkObject,long id)
    {
        return null;
    }
}
