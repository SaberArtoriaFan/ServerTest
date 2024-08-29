using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.Net
{
    internal class NetworkSpriteSyncHandler : Message<G2C_SyncSprite>
    {
        protected override async FTask Run(Session session, G2C_SyncSprite message)
        {
            var netobj = NetWorkManager.Instance.GetNetworkObject(message.NetworkObjectID);
            if(netobj != null)
            {
                var ns= netobj.GetComponent<NetworkSprite>();
                if(ns != null)
                {
                    ns.HandleSprite(message.SpriteName);
                }
            }

            await FTask.CompletedTask;
        }
    }
}
