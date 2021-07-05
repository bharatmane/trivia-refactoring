using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    public class PlayerList
    {
        private readonly List<Player> players;
        private int currentPlayerIndex = 0;

        [Obsolete("Replaced by the construction to inject player names")]
        public PlayerList()
        {
            players = new List<Player>();
        }

        public PlayerList(List<String> names)
        {
            players = new List<Player>();
            foreach (var name in names)
            {
                this.Add(new Player(name));
            }
        }

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
