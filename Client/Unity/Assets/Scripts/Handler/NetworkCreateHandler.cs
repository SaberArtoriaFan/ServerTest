using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.Net
{
    public class NetworkCreateHandler : Message<G2C_CreateNetworkObjectId>
    {
        protected override async FTask Run(Session session, G2C_CreateNetworkObjectId message)
        {
            NetWorkManager.Instance.CreateGameObjByServer(message.data,message.Authority);

            await FTask.CompletedTask;
        }
    }
}
