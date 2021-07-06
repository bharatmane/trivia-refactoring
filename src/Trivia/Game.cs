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
        private readonly TextWriter _stdOutput;
        private readonly Board _board;
        private readonly PlayerList _playerList;
        private readonly  QuestionDeck _deck;
                
        public Game(TextWriter stdOutput, Board board, QuestionDeck deck, PlayerList players)
        {
            this._stdOutput = stdOutput;

            this._board = board;
            this._deck = deck;
            this._playerList = players;
            PrintPlayerInformation();
        }
        
        private void PrintPlayerInformation()
        {
            foreach (var player in _playerList.Players)
            {
                Print(player.Name + " was added");
            }
            Print("Total players are : " + _playerList.Count);
        }

        public void Roll(int roll)
        {
            Player currentPlayer = _playerList.CurrentPlayer;
            Print(currentPlayer + " is the current player");
            Print("They have rolled a " + roll);

            if (currentPlayer.IsInPenaltyBox())
            {
                if (ShouldReleaseFromPenaltyBox(roll))
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

            int newPosition = _board.NewPosition(currentPlayer.Position, roll);

            currentPlayer.MoveTo(newPosition);

            Category currentCategory = _board.CategoryOf(newPosition);

            Print(currentPlayer + "'s new location is " + newPosition);
            Print("The category is " + currentCategory);

            Print( _deck.NextQuestionAbout(currentCategory));
        }

        private static bool ShouldReleaseFromPenaltyBox(int roll)
        {
            return (roll % 2 != 0);
        }

        private void Print(string message)
        {
            _stdOutput.WriteLine(message);
        }
        
        public bool WasCorrectlyAnswered()
        {
            Player currentPlayer = _playerList.CurrentPlayer;

            if (currentPlayer.IsInPenaltyBox())
            {
                _playerList.NextPlayer();
                return true;
            }

            Print("Answer was correct!!!!");
            currentPlayer.Reward(1);
            
            Print(currentPlayer + " now has " + currentPlayer.GoldCoins + " Gold Coins.");

            var doesGameContinues = !currentPlayer.HasWon();
            _playerList.NextPlayer();

            return doesGameContinues;
        }

        public bool WrongAnswer()
        {
            Player currentPlayer = _playerList.CurrentPlayer;
            Print("Question was incorrectly answered");
            Print(currentPlayer + " was sent to the penalty box");
            currentPlayer.EntersPenaltyBox();
            _playerList.NextPlayer();
            return true;
        }
        
    }

}
