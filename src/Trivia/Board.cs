using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    public class Board
    {
        private readonly int cellsCount;
        private readonly List<Category> categories;

        public Board(int cellsCount, List<Category> categories)
        {
            this.cellsCount = cellsCount;
            this.categories = categories;
        }

        public int NewPosition(int currentPosition, int offset)
        {
            return (currentPosition + offset) % cellsCount;
        }

        public Category CategoryOf(int position)
        {
            return categories[position % categories.Count];
        }
    }
}
