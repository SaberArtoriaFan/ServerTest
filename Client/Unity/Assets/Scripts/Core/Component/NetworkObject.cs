using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Fantasy
{
    public class NetworkObject:MonoBehaviour
    {
        private long identity=-1;
        protected bool authority = default;

        public long Identity { get => identity;protected set => identity = value; }
        public bool Authority { get => authority;protected set => authority = value; }
        public List<NetworkBehavior> NetworkBehaviors { get => networkBehaviors;  }

        List<NetworkBehavior> networkBehaviors = new List<NetworkBehavior>();

        public bool isNetworkInit { get; protected set; } = false;

        internal void Register(NetworkBehavior networkBehavior)
        {
            networkBehaviors.Add(networkBehavior);
        }
        internal void UnRegister(NetworkBehavior networkBehavior)
        {
            networkBehaviors.Remove(networkBehavior);
        }
        internal void InitNetwork(long id,bool authority)
        {
            this.identity = id;
            this.Authority = authority;
            isNetworkInit = true;
        }
        internal void IdentifyScript(List<long> scriptsID)
        {
            var nb = GetComponents<NetworkBehavior>().ToList();

            if (scriptsID != null)
            {
                var sIDs = nb.Select(u => u.ScriptID).ToList();
                for (int i = 0; i < scriptsID.Count; i++)
                {
                    if (sIDs.Contains(scriptsID[i]))
                        continue;
                    else
                    {
                        var comp = this.AddNetworkComponentById(scriptsID[i]);
                        if (comp != null) nb.Add(comp);
                        //只负责加，数据自己同步去
                    }
                }
            }

            foreach (var n in nb)
            {
                if(n.isNetworkInit==false)
                    n.OnNetworkObjectInit();
            }
            
        }
    }
}
