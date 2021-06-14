using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Trivia;

namespace Tests
{
    class PlayerListTests
    {
        [Test]
        public void Should_Select_First_Player_When_In_First_Round()
        {
            PlayerList players = new PlayerList();
            players.Add(new Player("J B Rains"));
            players.Add(new Player("Emily B"));
            players.Add(new Player("Michael Feather"));

            Assert.That(players.CurrentPlayer.Name, Is.EqualTo("J B Rains"));
        }

        [Test]
        public void Should_Select_Next_Player_When_In_Next_Round()
        {
            PlayerList players = new PlayerList();
            players.Add(new Player("J B Rains"));
            players.Add(new Player("Emily B"));
            players.Add(new Player("Michael Feather"));

            Assert.That(players.NextPlayer().Name, Is.EqualTo("Emily B"));
        }

        [Test]
        public void Should_Select_First_Player_When_Moved_To_Next_After_Last_Round()
        {
            PlayerList players = new PlayerList();
            players.Add(new Player("J B Rains"));
            players.Add(new Player("Emily B"));
            players.Add(new Player("Michael Feather"));
            players.NextPlayer();
            players.NextPlayer();
            Assert.That(players.NextPlayer().Name, Is.EqualTo("J B Rains"));
        }
    }
}
