using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    public class Player
    {
        private readonly string name;
        private int position;
        private int goldCoinsCount;
        private bool isInPenaltyBox = false;
        public Player(string name)
        {
            this.name = name;
        }

        public int Position
        {
            get { return position; }
        }

        void MoveTo(int newPosition)
        {
            this.position = newPosition;
        }

        public int GoldCoins
        {
            get { return goldCoinsCount; }
        }
        
        public void Reward(int goldCoinsCount)
        {
            this.goldCoinsCount += goldCoinsCount;
        }

        public bool HasWon()
        {
            return goldCoinsCount >= 6;
        }

        public void EntersPenaltyBox()
        {
            this.isInPenaltyBox = true;
        }

        public void ExitsPenaltyBox()
        {
            this.isInPenaltyBox = false;
        }

        public bool IsInPenaltyBox()
        {
            return isInPenaltyBox;
        }
        public override string ToString()
        {
            return name;
        }
    }
}
