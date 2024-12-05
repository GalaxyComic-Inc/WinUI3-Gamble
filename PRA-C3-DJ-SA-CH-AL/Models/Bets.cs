using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL.Models
{
    public class Bets
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public int PlayerGuess {  get; set; }
        public bool Payed { get; set; } = false;
        public DateTime BetTime { get; set; } = DateTime.Now;
    }
}
