using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.Net
{
    internal class G2C_ExitRoomHandler : Message<G2C_ExitRoom>
    {
        protected override async FTask Run(Session session, G2C_ExitRoom message)
        {
            Log.Debug($"{message.ClientID},退出房间了");

            await FTask.CompletedTask;
        }
    }
}
