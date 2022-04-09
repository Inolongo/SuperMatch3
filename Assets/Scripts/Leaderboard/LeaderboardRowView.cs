using TMPro;
using UnityEngine;

namespace Leaderboard
{
    public class LeaderboardRowView: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _score;

        public void Init(string name, int score)
        {
            _name.text = name;
            _score.text = score.ToString();
        }
    }
}