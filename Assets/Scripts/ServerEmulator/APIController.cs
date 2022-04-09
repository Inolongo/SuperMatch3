using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServerEmulator.ServerModels;
using UnityEngine;
using UnityEngine.Rendering;

namespace ServerEmulator
{
    public class APIController
    {
        private const string LeaderboardPath = "/FakeServerData/Leaderboard/Users.json";


        public APIController()
        {
            MakeFakeData();
        }

        public async Task<string> Get(string uri)
        {
            return await GetFakeData(uri);
        }

        private async Task<string> GetFakeData(string uri)
        {
            switch (uri)
            {
                case ServerMessages.Leaderboard:
                    return await GetLeaderboardData();
                    break;
                default:
                    return null;
            }
        }

        private async Task<string> GetLeaderboardData()
        {
            var usersJson = File.ReadAllText(Application.persistentDataPath + LeaderboardPath);
            await Task.Delay(100);
            return usersJson;
        }


        private void MakeFakeData()
        {
            if (File.Exists(Application.dataPath + LeaderboardPath)) return;

            var users = new List<LeaderboardModel>
            {
                new LeaderboardModel("Cock", 69),
                new LeaderboardModel("Dick", 42),
                new LeaderboardModel("Dungeon Master", 228)
            };
            var json = JsonConvert.SerializeObject(users);
            //var json = JsonUtility.ToJson(users);
            var fileInfo = new FileInfo(Application.persistentDataPath + LeaderboardPath);
            fileInfo.Directory?.Create();
            File.WriteAllText(Application.persistentDataPath + LeaderboardPath, json);
        }
    }
}