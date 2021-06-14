﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Trivia
{
    public class Game
    {
        private const int NUMBER_OF_CELLS = 12;
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
                    Move(roll);

                    Print(_players[_currentPlayer]
                          + "'s new location is "
                          + CurrentPosition());
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
                Move(roll);

                Print(_players[_currentPlayer]
                        + "'s new location is "
                        + CurrentPosition());
                Print("The category is " + CurrentCategory());
                AskQuestion();
            }
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
            if (CurrentPosition() == 0) return Category.Pop;
            if (CurrentPosition() == 4) return Category.Pop;
            if (CurrentPosition() == 8) return Category.Pop;
            if (CurrentPosition() == 1) return Category.Science;
            if (CurrentPosition() == 5) return Category.Science;
            if (CurrentPosition() == 9) return Category.Science;
            if (CurrentPosition() == 2) return Category.Sports;
            if (CurrentPosition() == 6) return Category.Sports;
            if (CurrentPosition() == 10) return Category.Sports;
            return Category.Rock;
        }

        private int CurrentPosition()
        {
            return _places[_currentPlayer];
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
                    NextPlayer();

                    return winner;
                }
                else
                {
                    NextPlayer();
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
                
                NextPlayer();

                return winner;
            }
        }

        private void NextPlayer()
        {
            _currentPlayer++;
            if (_currentPlayer == _players.Count)
            {
                _currentPlayer = 0;
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
