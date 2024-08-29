using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Saber.Net
{
    internal class G2C_LobbyFinishMessageHandler : Message<G2C_LobbyFinishMessage>
    {
        protected override async FTask Run(Session session, G2C_LobbyFinishMessage message)
        {
            SceneManager.LoadScene(1);
            await FTask.CompletedTask;
        }
    }
}
