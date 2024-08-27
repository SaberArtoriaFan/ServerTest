using Fantasy;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

partial class NetWorkManager
{
    //序列化的
    public List<NetworkObject> networkObjects = new List<NetworkObject>();

    Dictionary<long, NetworkObject> networkPrefabDict= new Dictionary<long, NetworkObject>();

    //维护当前活跃的networkObj
    Dictionary<long,NetworkObject> networkObjDict= new Dictionary<long,NetworkObject>();


    protected void InitNetworkObj()
    {
        long id = 1;
        networkPrefabDict.Clear();
        foreach (var v in networkObjects)
            networkPrefabDict.Add(id++, v);
    }

    void HandleNewNetworkObj(NetworkObject networkObject)
    {
        networkObjDict.Add(networkObject.Identity, networkObject);
    }
    /// <summary>
    /// 被动添加的单位，都是无权限
    /// </summary>
    /// <param name="data"></param>
    internal void CreateGameObjByServer(InitData data,bool authority=false)
    {
        var model = networkPrefabDict[data.PrefabID].gameObject;
        model=GameObject.Instantiate(model,data.Transform.position.ToUnity(),data.Transform.quaternion.ToUnity());
        var no= model.GetOrAddComponent<NetworkObject>();
        no.InitNetwork(data.NetworkObjectID, authority);
        no.IdentifyScript(data.NetworkScriptsID);
        HandleNewNetworkObj(no);

    }
    internal async FTask<NetworkObject> CreateGameObj(long prefabID,Vector3 pos,Quaternion quaternion)
    {
        var scriptId = networkPrefabDict[prefabID].GetComponents<NetworkBehavior>().Select((u=>u.ScriptID)).ToList();
        var callback = (M2C_ResponseNetworkObjectId)await _session.Call(new C2M_RequestNetworkObjectId()
        {
            PrefabID = prefabID,
            NetworkScriptsID = scriptId,
            Transform=new TransformData() { position=pos.ToMessage(),quaternion=quaternion.ToMessage()}
            
        });
        var model = GameObject.Instantiate(networkPrefabDict[prefabID].gameObject,pos, quaternion);
        var no = model.GetOrAddComponent<NetworkObject>();
        no.InitNetwork(callback.AddressableId,true);
        HandleNewNetworkObj(no);
        no.IdentifyScript(null);
        return no;
    }

    public NetworkObject GetNetworkObject(long id)
    {
        if(networkObjDict.TryGetValue(id, out var networkObj)) return networkObj;
        return null;
    }

    protected void UpdateNetworkObj()
    {
        var objs=networkObjDict.Values.ToArray();
        for(int i = 0; i < objs.Length; i++)
        {
            var obj = objs[i];
            for(int j = 0; j < obj.NetworkBehaviors.Count; j++)
            {
                obj.NetworkBehaviors[j].NetworkUpdate();
            }
        }
    }

    [Button]
    async void Test()
    {
        await CreateGameObj(1,Vector3.zero,Quaternion.identity);
    }
}