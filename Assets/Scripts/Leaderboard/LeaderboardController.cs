using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServerEmulator;
using ServerEmulator.ServerModels;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardController : MonoBehaviour
    {
        [SerializeField] private LeaderboardRowView rowPrefab;
        [SerializeField] private Transform content;

        private APIController _apiController;
        private List<LeaderboardModel> _listLeaderboardModel;

        public async void Init(APIController apiController)
        {
            _apiController = apiController;
            await InitData();

            Show();
        }

        private void Show()
        {
            foreach (var model in _listLeaderboardModel)
            {
                var leaderboardRowView = Instantiate(rowPrefab, content);
                leaderboardRowView.Init(model.Name, model.Score);
            }
        }

        private async Task InitData()
        {
            var usersJson = await _apiController.Get(ServerMessages.Leaderboard);
            _listLeaderboardModel = ParseServerData(usersJson);
        }


        private List<LeaderboardModel> ParseServerData(string jsonData)
        {
            return JsonConvert.DeserializeObject<List<LeaderboardModel>>(jsonData);
        }
    }
}