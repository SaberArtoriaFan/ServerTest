using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.Net
{
    internal class G2C_HurtHandler : Message<G2C_Hurt>
    {
        protected override async FTask Run(Session session, G2C_Hurt message)
        {
            var netObj= NetWorkManager.Instance.GetNetworkObject(message.NetworkObjectID);
            if(netObj != null)
            {
                var role = netObj.GetComponent<PaoPaoRoleController>();
                if(role != null)
                {
                    role.Hurt(true);
                }
            }
            await FTask.CompletedTask;
        }
    }
}
