using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fantasy
{
    internal class G2M_RemoveClientHandler : Route<Scene, G2M_RemoveClient>
    {
        protected override async FTask Run(Scene entity, G2M_RemoveClient message)
        {
            var logicMgr=entity.GetComponent<LogicMgr>();
            logicMgr.RemoveUnit(message.ClientID);
            Log.Debug($"玩家被移除:ClientID:{message.ClientID}");
            await FTask.CompletedTask;
        }
    }
}