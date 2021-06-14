using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private readonly TextWriter stdOutput;
        private readonly List<string> _players = new();

        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];

        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly Queue<string> _popQuestions = new();
        private readonly Queue<string> _scienceQuestions = new();
        private readonly Queue<string> _sportsQuestions = new();
        private readonly Queue<string> _rockQuestions = new();

        private int _currentPlayer;
        private bool _isGettingOutOfPenaltyBox;


        public Game(TextWriter stdOutput)
        {
            this.stdOutput = stdOutput;
            for (var i = 0; i < 50; i++)
            {
                _popQuestions.Enqueue("Pop Question " + i);
                _scienceQuestions.Enqueue(("Science Question " + i));
                _sportsQuestions.Enqueue(("Sports Question " + i));
                _rockQuestions.Enqueue(CreateRockQuestion(i));
            }
        }
        [Obsolete]
        public Game():this(Console.Out)
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
                    _isGettingOutOfPenaltyBox = true;

                    Print(_players[_currentPlayer] + " is getting out of the penalty box");
                    _places[_currentPlayer] = _places[_currentPlayer] + roll;
                    if (_places[_currentPlayer] > 11)
                    {
                        _places[_currentPlayer] = _places[_currentPlayer] - 12;
                    }

                    Print(_players[_currentPlayer]
                            + "'s new location is "
                            + _places[_currentPlayer]);
                    Print("The category is " + CurrentCategory());
                    AskQuestion();
                }
                else
                {
                    Print(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                _places[_currentPlayer] = _places[_currentPlayer] + roll;
                if (_places[_currentPlayer] > 11)
                {
                    _places[_currentPlayer] = _places[_currentPlayer] - 12;
                }

                Print(_players[_currentPlayer]
                        + "'s new location is "
                        + _places[_currentPlayer]);
                Print("The category is " + CurrentCategory());
                AskQuestion();
            }
        }

        private void Print(string message)
        {
            stdOutput.WriteLine(message);
        }

        private void AskQuestion()
        {
            if (CurrentCategory().Equals(Category.Pop))
            {
                Print(_popQuestions.Dequeue());
            }
            if (CurrentCategory().Equals(Category.Science))
            {
                Print(_scienceQuestions.Dequeue());
            }
            if (CurrentCategory().Equals(Category.Sports))
            {
                Print(_sportsQuestions.Dequeue());
            }
            if (CurrentCategory().Equals(Category.Rock))
            {
                Print(_rockQuestions.Dequeue());
            }
        }

        private Category CurrentCategory()
        {
            if (_places[_currentPlayer] == 0) return Category.Pop;
            if (_places[_currentPlayer] == 4) return Category.Pop;
            if (_places[_currentPlayer] == 8) return Category.Pop;
            if (_places[_currentPlayer] == 1) return Category.Science;
            if (_places[_currentPlayer] == 5) return Category.Science;
            if (_places[_currentPlayer] == 9) return Category.Science;
            if (_places[_currentPlayer] == 2) return Category.Sports;
            if (_places[_currentPlayer] == 6) return Category.Sports;
            if (_places[_currentPlayer] == 10) return Category.Sports;
            return Category.Rock;
        }

        public bool WasCorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Print("Answer was correct!!!!");
                    _purses[_currentPlayer]++;
                    Print(_players[_currentPlayer]
                            + " now has "
                            + _purses[_currentPlayer]
                            + " Gold Coins.");

                    var winner = DidPlayerWin();
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count)
                    {
                        _currentPlayer = 0;
                    }

                    return winner;
                }
                else
                {
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count)
                    {
                        _currentPlayer = 0;
                    }
                    return true;
                }
            }
            else
            {
                Print("Answer was corrent!!!!");
                _purses[_currentPlayer]++;
                Print(_players[_currentPlayer]
                        + " now has "
                        + _purses[_currentPlayer]
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                _currentPlayer++;
                if (_currentPlayer == _players.Count)
                {
                    _currentPlayer = 0;
                }

                return winner;
            }
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
