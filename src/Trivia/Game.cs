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
        private const int NUMBER_OF_CELLS = 12;
        private readonly Category[] CATEGORIES = new[] { Category.Pop, Category.Science, Category.Sports, Category.Rock };

        private readonly Dictionary<int, Category>
            categoriesByPosition = new Dictionary<int, Category>(NUMBER_OF_CELLS);

        private readonly Dictionary<Category, Queue<string>> questionsByCategory =
            new Dictionary<Category, Queue<string>>();


        private readonly TextWriter stdOutput;
        private readonly List<string> _players = new();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private int _currentPlayer;
        


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

        
        public bool Add(string playerName)
        {
            _players.Add(playerName);
            _places[_players.Count] = 0;
            _purses[_players.Count] = 0;
            _inPenaltyBox[_players.Count] = false;

            Print(playerName + " was added");
            Print("They are player number " + _players.Count);
            return true;
        }

        public void Roll(int roll)
        {
            string playerName = _players[_currentPlayer];
            Print(playerName + " is the current player");
            Print("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _inPenaltyBox[_currentPlayer] = false;
                    Print(playerName + " is getting out of the penalty box");
                }
                else
                {
                    Print(playerName + " is not getting out of the penalty box");
                    return;
                }
            }

            int currentPosition = PositionOf(_currentPlayer);

            MoveTo(_currentPlayer, NewPosition(currentPosition, roll));

            currentPosition = PositionOf(_currentPlayer);

            Category currentCategory = CategoryOf(currentPosition);

            Print(playerName + "'s new location is " + currentPosition);
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

        private void MoveTo(int currentPlayer, int position)
        {
            _places[currentPlayer] = position;
        }
        private int PositionOf(int currentPlayer)
        {
            return _places[currentPlayer];
        }
        private void Print(string message)
        {
            stdOutput.WriteLine(message);
        }
        private int NextPlayer()
        {
            return (_currentPlayer + 1) % _players.Count;
        }

        private bool HasWon(int currentPlayer)
        {
            return (_purses[currentPlayer] == 6);
        }



        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                _currentPlayer = NextPlayer();
                return true;
            }

            Print("Answer was correct!!!!");
            _purses[_currentPlayer]++;
            Print(_players[_currentPlayer]
                  + " now has "
                  + _purses[_currentPlayer]
                  + " Gold Coins.");

            var doesGameContinues = !HasWon(_currentPlayer);
            _currentPlayer = NextPlayer();

            return doesGameContinues;

        }

       

        public bool WrongAnswer()
        {
            Print("Question was incorrectly answered");
            Print(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            _currentPlayer = NextPlayer();
            return true;
        }


        
    }

}
