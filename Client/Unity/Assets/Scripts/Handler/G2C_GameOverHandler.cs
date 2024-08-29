using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Saber.Net
{
    internal class G2C_GameOverHandler : Message<G2C_GameOver>
    {
        protected override async FTask Run(Session session, G2C_GameOver message)
        {
            NetWorkManager.isAddress = false;
            SceneManager.LoadScene(0);
            NetWorkManager.SendC2M(new C2M_ExitRoom());
            Log.Debug("游戏结束，退出房间");
            //游戏结束退出房间
            //TODO:可以退出之后展示一下UI
            await FTask.CompletedTask;
        }
    }
}
