using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Models.API.Responses.Statistics
{
    public class StatisticGroup
    {
        
        public int Correct { set; get; }
        public int Wrong { get; set; }
        public int Total { get; set; }

        public StatisticGroup()
        {
        }

        public StatisticGroup(int correct, int wrong, int total)
        {
            Correct = correct;
            Wrong = wrong;
            Total = total;
        }

        public StatisticGroup(IEnumerable<StatisticGroup> groups)
        {
            Correct = groups.Sum(g => g.Correct);
            Wrong = groups.Sum(g => g.Wrong);
            Total = groups.Sum(g => g.Total);
        }
        
    }
}