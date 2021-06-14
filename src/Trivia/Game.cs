using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Schema;

namespace Trivia
{
    public class Game
    {
        private readonly TextWriter stdOutput;
        private const int NUMBER_OF_CELLS = 12;
        private static readonly int NB_QUESTIONS = 50;
        private List<Category> categories = new() { Category.Pop, Category.Science, Category.Sports, Category.Rock };
        
        private readonly Dictionary<int, Category>
            categoriesByPosition = new Dictionary<int, Category>(NUMBER_OF_CELLS);
        
        private readonly Dictionary<Category, Queue<string>> questionsByCategory =
            new Dictionary<Category, Queue<string>>();

        private readonly Board board;
        private readonly PlayerList playerList = new PlayerList();
        private readonly  QuestionDeck deck;
        public Game(TextWriter stdOutput)
        {
            this.stdOutput = stdOutput;

            board = new Board(NUMBER_OF_CELLS, categories);
            deck = new QuestionDeck(NB_QUESTIONS, categories);
        }
        [Obsolete]
        public Game() : this(Console.Out)
        {

        }

        
        public void Add(string playerName)
        {
            playerList.Add(new Player(playerName));

            Print(playerName + " was added");
            Print("They are player number " + playerList.Count);
        }

        public void Roll(int roll)
        {
            Player currentPlayer = playerList.CurrentPlayer;
            Print(currentPlayer + " is the current player");
            Print("They have rolled a " + roll);

            if (currentPlayer.IsInPenaltyBox())
            {
                if (shouldReleaseFromPenaltyBox(roll))
                {
                    currentPlayer.ExitsPenaltyBox();
                    Print(currentPlayer + " is getting out of the penalty box");
                }
                else
                {
                    Print(currentPlayer + " is not getting out of the penalty box");
                    return;
                }
            }

            int newPosition = board.NewPosition(currentPlayer.Position, roll);

            currentPlayer.MoveTo(newPosition);

            Category currentCategory = board.CategoryOf(newPosition);

            Print(currentPlayer + "'s new location is " + newPosition);
            Print("The category is " + currentCategory);

            Print( deck.NextQuestionAbout(currentCategory));
        }

        private bool shouldReleaseFromPenaltyBox(int roll)
        {
            return (roll % 2 != 0);
        }

        private int NewPosition(int currentPosition, int roll)
        {
            return (currentPosition + roll) % NUMBER_OF_CELLS;
        }
        private string NextQuestionAbout(Category category)
        {
            return questionsByCategory[category].Dequeue();
        }
        private Category CategoryOf(int position)
        {
            return categoriesByPosition[position];
        }
        private void Print(string message)
        {
            stdOutput.WriteLine(message);
        }
        
        public bool WasCorrectlyAnswered()
        {
            Player currentPlayer = playerList.CurrentPlayer;

            if (currentPlayer.IsInPenaltyBox())
            {
                playerList.NextPlayer();
                return true;
            }

            Print("Answer was correct!!!!");
            currentPlayer.Reward(1);
            
            Print(currentPlayer + " now has " + currentPlayer.GoldCoins + " Gold Coins.");

            var doesGameContinues = !currentPlayer.HasWon();
            playerList.NextPlayer();

            return doesGameContinues;
        }

        public bool WrongAnswer()
        {
            Player currentPlayer = playerList.CurrentPlayer;
            Print("Question was incorrectly answered");
            Print(currentPlayer + " was sent to the penalty box");
            currentPlayer.EntersPenaltyBox();
            playerList.NextPlayer();
            return true;
        }
        
    }

}
