using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Trivia;

namespace Tests
{
    class BoardTests
    {
        private List<Category> EmptyList()
        {
            return new List<Category>();
        }
        [Test]
        public void Should_Move_Forward_When_Moved()
        {
            Board board = new Board(12, EmptyList());

            Assert.That(board.NewPosition(0, 4),Is.EqualTo(4));
            
        }

        [Test]
        public void Should_Return_to_Start_When_Move_Goes_Beyond_Number_of_Cells()
        {
            Board board = new Board(12, EmptyList());

            Assert.That(board.NewPosition(11, 1), Is.EqualTo(0));
        }

        [Test]
        public void Should_Return_Categories_in_the_Right_Order()
        {
            Board board = new Board(12, new List<Category>(){Category.Pop, Category.Science});

            Assert.That(board.CategoryOf(0), Is.EqualTo(Category.Pop));
            Assert.That(board.CategoryOf(1), Is.EqualTo(Category.Science));
            Assert.That(board.CategoryOf(2), Is.EqualTo(Category.Pop));
        }
    }
}
