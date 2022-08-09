namespace WebAPI.Models.API.Responses.Statistics
{
    public class UserStatistic
    {
        
        public StatisticGroup Training { get; set; }
        public StatisticGroup Simulation { get; set; }
        public StatisticGroup Global { get; set; }

        public UserStatistic()
        {
        }

        public UserStatistic(StatisticGroup training, StatisticGroup simulation, StatisticGroup global)
        {
            Training = training;
            Simulation = simulation;
            Global = global;
        }
    }
}