using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.Net
{
    public class NetworkTransformSyncHandler : Message<G2C_SyncTransform>
    {
        protected override async FTask Run(Session session, G2C_SyncTransform message)
        {
            var netobj= NetWorkManager.Instance.GetNetworkObject(message.NetworkObjectID);
            if(netobj != null)
            {
                var syncTrasnform = netobj.GetComponent<NetworkTransform>();
                if (syncTrasnform != null) syncTrasnform.HandleSyncTransform(message.Transform);
                else
                {
                    netobj.transform.position = message.Transform.position.ToUnity();
                    netobj.transform.rotation = message.Transform.quaternion.ToUnity();
                    netobj.gameObject.AddComponent<NetworkTransform>(); 
                }
            }

            await FTask.CompletedTask;
        }
    }
}
