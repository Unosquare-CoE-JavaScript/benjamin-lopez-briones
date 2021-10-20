using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp
{
    public class Calculations
    {
        public List<int> FiboN => new List<int> { 1, 1, 2, 3, 5, 8, 13 };

        public bool IsOdd(int vall)
        {
            return (vall % 2) == 1;
        }
    }
}
