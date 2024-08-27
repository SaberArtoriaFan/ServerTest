using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Saber.Net
{
    public class G2C_DeleteNetworkObjHandler : Message<G2C_DeleteNetworkObj>
    {
        protected override async FTask Run(Session session, G2C_DeleteNetworkObj message)
        {
            var obj = NetWorkManager.Instance.GetNetworkObject(message.NetworkObjectID);
            if (obj != null)
            {
                GameObject.Destroy(obj.gameObject);
            }
            await FTask.CompletedTask;
        }
    }
}
