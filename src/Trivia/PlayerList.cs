using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    public class PlayerList
    {
        private readonly List<Player> players = new List<Player>();
        private int currentPlayerIndex = 0;
        public void Add(Player player)
        {
            players.Add(player);
        }

        public Player CurrentPlayer
        {
            get { return players[currentPlayerIndex]; }
        }

        public int Count
        {
            get { return players.Count;}
        }

        public Player NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            return players[currentPlayerIndex];
            
        }
    }
}
