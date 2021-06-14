using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Trivia;

namespace Tests
{
    class PlayerTests
    {
        [Test]
        public void Should_Win_When_CoinsAre_6()
        {
            Player player = new Player("Chet");
            player.Reward(6);

            Assert.AreEqual(true,player.HasWon());
        }

        [Test]
        public void Should_Not_Win_When_CoinsAre_Less_Than_6()
        {
            Player player = new Player("Chet");
            player.Reward(5);

            Assert.AreEqual(false, player.HasWon());
        }

    }
}
