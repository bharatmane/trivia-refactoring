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
        private readonly Category[] CATEGORIES = new[] { Category.Pop, Category.Science, Category.Sports, Category.Rock };
        private readonly Dictionary<int, Category>
            categoriesByPosition = new Dictionary<int, Category>(NUMBER_OF_CELLS);
        private readonly Dictionary<Category, Queue<string>> questionsByCategory =
            new Dictionary<Category, Queue<string>>();
        private readonly List<Player> players = new List<Player>();
        private Player currentPlayer = null;
        private int currentPlayerIndex = 0;

        public Game(TextWriter stdOutput)
        {
            this.stdOutput = stdOutput;

            foreach (var category in CATEGORIES)
            {
                questionsByCategory.Add(category, new Queue<string>());
            }

            for (var i = 0; i < 50; i++)
            {
                foreach (var category in CATEGORIES)
                {
                    questionsByCategory[category].Enqueue(category.ToString() + " Question " + i);
                }
            }

            for (int i = 0; i < NUMBER_OF_CELLS; i++)
            {
                categoriesByPosition.Add(i, CATEGORIES[i % CATEGORIES.Length]);
            }
        }
        [Obsolete]
        public Game() : this(Console.Out)
        {

        }

        
        public void Add(string playerName)
        {
            players.Add(new Player(playerName));

            Print(playerName + " was added");
            Print("They are player number " + players.Count);
            currentPlayer = players[currentPlayerIndex];
        }

        public void Roll(int roll)
        {
            
            Print(currentPlayer + " is the current player");
            Print("They have rolled a " + roll);

            if (currentPlayer.IsInPenaltyBox())
            {
                if (roll % 2 != 0)
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

            currentPlayer.MoveTo(NewPosition(currentPlayer.Position, roll));

            Category currentCategory = CategoryOf(currentPlayer.Position);

            Print(currentPlayer + "'s new location is " + currentPlayer.Position);
            Print("The category is " + currentCategory);

            Print( NextQuestionAbout(currentCategory));
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
        private Player NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            return players[currentPlayerIndex];
        }
        public bool WasCorrectlyAnswered()
        {
            if (currentPlayer.IsInPenaltyBox())
            {
                currentPlayer = NextPlayer();
                return true;
            }

            Print("Answer was correct!!!!");
            currentPlayer.Reward(1);
            Print(currentPlayer + " now has " + currentPlayer.GoldCoins + " Gold Coins.");

            var doesGameContinues = !currentPlayer.HasWon();
            currentPlayer = NextPlayer();

            return doesGameContinues;
        }

        public bool WrongAnswer()
        {
            Print("Question was incorrectly answered");
            Print(currentPlayer + " was sent to the penalty box");
            currentPlayer.EntersPenaltyBox();

            currentPlayer = NextPlayer();
            return true;
        }
        
    }

}
