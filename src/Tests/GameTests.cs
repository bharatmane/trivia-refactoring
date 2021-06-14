using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using Trivia;

namespace Tests
{
    [UseReporter(typeof(DiffReporter))]
    class GameTests
    {
        private bool _notAWinner;

        

        [Test]
        public void TestGame()
        {
            StringBuilder capturedOutput = new StringBuilder();
            Console.SetOut(new StringWriter(capturedOutput));

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

           Approvals.Verify(capturedOutput);
        }

    }
}
