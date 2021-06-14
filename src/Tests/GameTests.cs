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
        private static readonly int NUMBER_OF_CELLS = 12;
        private static readonly int NUMBER_OF_QUESTIONS = 50;

        private static readonly List<Category> CATEGORIES = new List<Category>()
            {Category.Pop, Category.Science, Category.Sports, Category.Rock};
        private static readonly List<String> PLAYERS = new List<string>(){"Chet", "Pat", "Sue"};

        [Test]
        public void TestGame()
        {
            StringBuilder capturedOutput = new StringBuilder();
            
            int startingGameId = 345;

            Approvals.Verify(RunAGame(startingGameId, new StringWriter(capturedOutput)));
        }

        private string RunAGame(int startingGameId, TextWriter textWriter)
        {
            var aGame = new Game(textWriter, new Board(NUMBER_OF_CELLS, CATEGORIES),
                new QuestionDeck(NUMBER_OF_QUESTIONS, CATEGORIES),
                new PlayerList(PLAYERS));

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

            return textWriter.ToString();
        }

        [Test]
        public void TestSeveralGames()
        {
            int howManyGames = 2;

            Approvals.VerifyAll(RunSeveralGames(howManyGames));
        }

        private Dictionary<int, string> RunSeveralGames(int howManyGames)
        {
            Dictionary<int, string> gamesOutput = new Dictionary<int, string>();
            for (int i = 0; i < howManyGames; i++)
            {
                StringBuilder capturedOutput = new StringBuilder();
                
                int startingGameId = 456 + i * 17;

                gamesOutput.Add(i, RunAGame(startingGameId, new StringWriter(capturedOutput)));
            }

            return gamesOutput;
        }
    }
}
