using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Trivia;

namespace Tests
{
    class QuestionDeckTests
    {
        [Test]
        public void should_ask_next_question_for_a_category()
        {
            QuestionDeck deck = new QuestionDeck(5, new List<Category>(){Category.Rock, Category.Sports});

            Assert.That(deck.NextQuestionAbout(Category.Rock), Is.EqualTo("Rock Question 0"));
            Assert.That(deck.NextQuestionAbout(Category.Sports), Is.EqualTo("Sports Question 0"));
            Assert.That(deck.NextQuestionAbout(Category.Sports), Is.EqualTo("Sports Question 1"));
        }
    }
}
