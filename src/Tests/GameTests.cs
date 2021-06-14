﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Trivia;

namespace Tests
{
    class GameTests
    {
        private bool _notAWinner;

        private readonly string GoldenMaster = @"Chet was added
    They are player number 1
    Pat was added
    They are player number 2
    Sue was added
    They are player number 3
    Chet is the current player
    They have rolled a 5
    Chet's new location is 5
    The category is Science
    Science Question 0
    Answer was corrent!!!!
    Chet now has 1 Gold Coins.
    Pat is the current player
    They have rolled a 5
    Pat's new location is 5
    The category is Science
    Science Question 1
    Answer was corrent!!!!
    Pat now has 1 Gold Coins.
    Sue is the current player
    They have rolled a 5
    Sue's new location is 5
    The category is Science
    Science Question 2
    Answer was corrent!!!!
    Sue now has 1 Gold Coins.
    Chet is the current player
    They have rolled a 3
    Chet's new location is 8
    The category is Pop
    Pop Question 0
    Answer was corrent!!!!
    Chet now has 2 Gold Coins.
    Pat is the current player
    They have rolled a 4
    Pat's new location is 9
    The category is Science
    Science Question 3
    Answer was corrent!!!!
    Pat now has 2 Gold Coins.
    Sue is the current player
    They have rolled a 3
    Sue's new location is 8
    The category is Pop
    Pop Question 1
    Question was incorrectly answered
    Sue was sent to the penalty box
    Chet is the current player
    They have rolled a 5
    Chet's new location is 1
    The category is Science
    Science Question 4
    Answer was corrent!!!!
    Chet now has 3 Gold Coins.
    Pat is the current player
    They have rolled a 3
    Pat's new location is 0
    The category is Pop
    Pop Question 2
    Answer was corrent!!!!
    Pat now has 3 Gold Coins.
    Sue is the current player
    They have rolled a 5
    Sue is getting out of the penalty box
    Sue's new location is 1
    The category is Science
    Science Question 5
    Answer was correct!!!!
    Sue now has 2 Gold Coins.
    Chet is the current player
    They have rolled a 1
    Chet's new location is 2
    The category is Sports
    Sports Question 0
    Answer was corrent!!!!
    Chet now has 4 Gold Coins.
    Pat is the current player
    They have rolled a 4
    Pat's new location is 4
    The category is Pop
    Pop Question 3
    Answer was corrent!!!!
    Pat now has 4 Gold Coins.
    Sue is the current player
    They have rolled a 2
    Sue is not getting out of the penalty box
    Chet is the current player
    They have rolled a 3
    Chet's new location is 5
    The category is Science
    Science Question 6
    Answer was corrent!!!!
    Chet now has 5 Gold Coins.
    Pat is the current player
    They have rolled a 3
    Pat's new location is 7
    The category is Rock
    Rock Question 0
    Answer was corrent!!!!
    Pat now has 5 Gold Coins.
    Sue is the current player
    They have rolled a 5
    Sue is getting out of the penalty box
    Sue's new location is 6
    The category is Sports
    Sports Question 1
    Answer was correct!!!!
    Sue now has 3 Gold Coins.
    Chet is the current player
    They have rolled a 3
    Chet's new location is 8
    The category is Pop
    Pop Question 4
    Answer was corrent!!!!
    Chet now has 6 Gold Coins.
";

        [Test]
        public void TestGame()
        {
            var aGame = new Game();

            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            var rand = new Random(345);

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