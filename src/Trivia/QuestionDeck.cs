using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    public class QuestionDeck
    {
        private readonly Dictionary<Category, Queue<String>> questionsByCategory = new();

        public QuestionDeck(int questionCount, List<Category> categories)
        {
            foreach (var category in categories)
            {
                questionsByCategory.Add(category, new Queue<string>());
            }

            for (int i = 0; i < questionCount; i++)
            {
                foreach (var category in categories)
                {
                    questionsByCategory[category].Enqueue(category.ToString() + " Question " + i);
                }
            }
        }

        public String NextQuestionAbout(Category category)
        {
            return questionsByCategory[category].Dequeue();
        }
    }
}
