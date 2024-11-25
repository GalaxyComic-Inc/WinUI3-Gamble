using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL.Models
{
    public class Goals
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int Minute { get; set; }
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public string PlayerName { get; set; }
    }
}
