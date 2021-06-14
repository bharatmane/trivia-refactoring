using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Trivia;

namespace Tests
{
    class GameTests
    {
        private bool _notAWinner;

        [Test]
        public void TestGame()
        {
            var aGame = new Game();

            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            var rand = new Random(345);

            do
            {
                aGame.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    _notAWinner = aGame.WrongAnswer();
                }
                else
                {
                    _notAWinner = aGame.WasCorrectlyAnswered();
                }
            } while (_notAWinner);
        }

    }
}
