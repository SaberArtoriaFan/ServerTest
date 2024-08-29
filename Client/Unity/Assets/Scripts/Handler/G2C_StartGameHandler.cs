
using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.Net
{
    public class G2C_StartGameHandler : Message<G2C_StartGame>
    {
        protected override async FTask Run(Session session, G2C_StartGame message)
        {
            PaoPaoGameManager.Instance.ResetGame();

            await FTask.CompletedTask;
        }
    }
}
