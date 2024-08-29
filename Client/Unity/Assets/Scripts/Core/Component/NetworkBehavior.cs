using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Fantasy
{
    [RequireComponent(typeof(NetworkObject))]
    public abstract class NetworkBehavior:MonoBehaviour
    {
        public NetworkObject netObj { get; private set; }
        public long ScriptID{get;private set;}
        public bool isNetworkInit { get; private set; } = false;
        protected virtual void Awake()
        {
            netObj = GetComponent<NetworkObject>();
            this.ScriptID=this.GetType().GetHashCode();
        }
        protected virtual void Start()
        {
            //if (isNetworkInit == false && netObj.isNetworkInit == true)
            //    OnNetworkObjectInit();
        }
        public virtual void OnNetworkObjectInit()
        {
            isNetworkInit = true;
            netObj.Register(this);
            Debug.Log("初始化成功");

        }
        public virtual void NetworkUpdate()
        {

        }

        protected virtual void OnDestroy() 
        {
            netObj.UnRegister(this);
            netObj = null;
        }  
    }
}
