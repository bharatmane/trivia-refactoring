using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    public class PlayerList
    {
        private readonly List<Player> _players;
        private int _currentPlayerIndex = 0;

        [Obsolete("Replaced by the constructor to inject player names")]
        public PlayerList()
        {
            _players = new List<Player>();
        }

        public PlayerList(List<String> names)
        {
            _players = new List<Player>();
            foreach (var name in names)
            {
                this.Add(new Player(name));
            }
        }

        public void Add(Player player)
        {
            _players.Add(player);
        }

        public Player CurrentPlayer => _players[_currentPlayerIndex];
        public List<Player> Players => _players;
        public int Count => _players.Count;

        public Player NextPlayer()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            return _players[_currentPlayerIndex];
            
        }
    }
}
