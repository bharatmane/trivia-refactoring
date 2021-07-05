using System;
using System.Collections.Generic;

namespace Trivia
{
    public class GameRunner
    {
        private static bool _notAWinner;
        private const int NUMBER_OF_CELLS = 12;
        private static readonly int NB_QUESTIONS = 50;

        public static void Main(string[] args)
        {
            List<Category> categories = new() {Category.Pop, Category.Science, Category.Sports, Category.Rock};
            Board board = new Board(NUMBER_OF_CELLS, categories);
            QuestionDeck questionDeck = new QuestionDeck(NB_QUESTIONS, categories);
            PlayerList playerList = new PlayerList(new List<string>(){ "Chet", "Pat", "Sue" });

            var aGame = new Game(Console.Out, board, questionDeck, playerList);

            var rand = new Random();

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