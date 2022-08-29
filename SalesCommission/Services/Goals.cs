using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesCommission.Services
{
    public class Goals
    {
        public List<(int, int)> GetGoals()
        {
            var goals = new List<(int, int)>
            {
                (1, 5),
                (2, 3),
                (3, 2),
                (4, 2),
                (5, 5),
                (6, 60),
                (8, 2),
                (9, 4),
                (10, 4),
                (11, 7),
                (12, 2)
            };

            return goals;
        }
    }
}
