using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRA_C3_DJ_SA_CH_AL.Models
{
    public class Results
    {
        public int Id { get; set; }
        public int Team1Id { get; set; }
        public string Team1Name { get; set; }
        public int Team1Score { get; set; }
        public int Team2Id { get; set; }
        public string Team2Name { get; set; }
        public int Team2Score { get; set; }
        public int WinnerId { get; set; }
    }
}
