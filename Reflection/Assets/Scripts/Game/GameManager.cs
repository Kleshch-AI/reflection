using Reflection.Game.Player;

namespace Reflection.Game
{

    internal class GameManager
    {

        private PlayerManager playerManager;

        internal void InitGame()
        {
            CreateManagers();
        }

        private void CreateManagers()
        {
            playerManager = new PlayerManager();
        }
        
    }

}