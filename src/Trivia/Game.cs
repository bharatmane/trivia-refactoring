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
        private bool _isGettingOutOfPenaltyBox;


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

        public string CreateRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public bool IsPlayable()
        {
            return (HowManyPlayers() >= 2);
        }

        public bool Add(string playerName)
        {
            _players.Add(playerName);
            _places[HowManyPlayers()] = 0;
            _purses[HowManyPlayers()] = 0;
            _inPenaltyBox[HowManyPlayers()] = false;

            Print(playerName + " was added");
            Print("They are player number " + _players.Count);
            return true;
        }

        public int HowManyPlayers()
        {
            return _players.Count;
        }

        public void Roll(int roll)
        {
            Print(_players[_currentPlayer] + " is the current player");
            Print("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _inPenaltyBox[_currentPlayer] = false;
                    Print(_players[_currentPlayer] + " is getting out of the penalty box");
                }
                else
                {
                    Print(_players[_currentPlayer] + " is not getting out of the penalty box");
                    return;
                }
            }

            Move(roll);

            Print(_players[_currentPlayer]
                  + "'s new location is "
                  + CurrentPosition());
            Print("The category is " + CurrentCategory());
            AskQuestion();
        }

        private void Move(int roll)
        {
            _places[_currentPlayer] = (CurrentPosition() + roll) % NUMBER_OF_CELLS;
        }

        private void Print(string message)
        {
            stdOutput.WriteLine(message);
        }

        private void AskQuestion()
        {
            Print(questionsByCategory[CurrentCategory()].Dequeue());
        }

        private Category CurrentCategory()
        {
            return categoriesByPosition[CurrentPosition()];
        }

        private int CurrentPosition()
        {
            return _places[_currentPlayer];
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

            var winner = DidPlayerWin();
            _currentPlayer = NextPlayer();

            return winner;

        }

        private int NextPlayer()
        {
            return  (_currentPlayer + 1) % _players.Count;
        }

        public bool WrongAnswer()
        {
            Print("Question was incorrectly answered");
            Print(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count)
            {
                _currentPlayer = 0;
            }
            return true;
        }


        private bool DidPlayerWin()
        {
            return !(_purses[_currentPlayer] == 6);
        }
    }

}
