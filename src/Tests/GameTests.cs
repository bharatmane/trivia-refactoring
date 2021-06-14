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
            int startingGameId = 345;

            string actualOutput = RunAGame(startingGameId, capturedOutput);

            Approvals.Verify(actualOutput);
        }

        private string RunAGame(int startingGameId, StringBuilder capturedOutput)
        {
            var aGame = new Game();

            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            var rand = new Random(startingGameId);

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

            return capturedOutput.ToString();
        }

        [Test]
        public void TestSeveralGames()
        {
            int howManyGames = 2;
            Dictionary<int, string> gamesOutput  = RunSeveralGames(howManyGames);

            Approvals.VerifyAll(gamesOutput);
        }

        private Dictionary<int, string> RunSeveralGames(int howManyGames)
        {
            Dictionary<int, string> gamesOutput = new Dictionary<int, string>();
            for (int i = 0; i < howManyGames; i++)
            {
                StringBuilder capturedOutput = new StringBuilder();
                Console.SetOut(new StringWriter(capturedOutput));

                int startingGameId = 456 + i * 17;

                string gameOutput;

                gameOutput = RunAGame(startingGameId, capturedOutput);

                gamesOutput.Add(i, gameOutput);
            }

            return gamesOutput;
        }
    }
}
