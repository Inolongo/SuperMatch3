using Newtonsoft.Json;

namespace ServerEmulator.ServerModels
{
    public class LeaderboardModel : AbstractServerModel
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public int Score { get; set; }

        public LeaderboardModel(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}