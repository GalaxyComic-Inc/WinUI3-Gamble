using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL.Models
{
    public class Results
    {
        public int Id { get; set; }

        [JsonPropertyName("team1_id")]
        public int Team1Id { get; set; }

        [JsonPropertyName("team1_name")]
        public string Team1Name { get; set; }

        [JsonPropertyName("team1_score")]
        public int Team1Score { get; set; }

        [JsonPropertyName("team2_id")]
        public int Team2Id { get; set; }

        [JsonPropertyName("team2_name")]
        public string Team2Name { get; set; }

        [JsonPropertyName("team2_score")]
        public int Team2Score { get; set; }

        [JsonPropertyName("winner_id")]
        public int WinnerId { get; set; }

        public string GetWinnerName(Results result)
        {
            if (result.WinnerId == result.Team1Id)
            {
                return result.Team1Name; // Team 1 is the winner
            }
            else if (result.WinnerId == result.Team2Id)
            {
                return result.Team2Name; // Team 2 is the winner
            }
            else
            {
                return "Draw"; // In case no winner is set or there's a draw (e.g., if WinnerId is 0 or null)
            }
        }

    }
}
