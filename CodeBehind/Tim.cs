using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind
{
    public class Tim
    { public string Team {  get; set; }
        public string ISOCode { get; set; }

        public int FIBARanking { get; set; }

        public List<Tim>? Grupa {  get; set; }

        public int Bodovi { get; set; } = 0;
        public int ScoredPoints { get; set; } = 0; 
        public int ReceivedPoints { get; set; } = 0;

        public int Pobede {  get; set; } = 0;
        public int Porazi {  get; set; } = 0;
        public int KosRazlika {  get; set; } = 0;

        public int Rang { get; set; } = 0;

        //tostring Kanada      3 / 0 / 6 / 267 / 247 / +20
        public override string ToString()
        {
            return string.Format("{0,4}. {1,-18} {2,3} / {3,3} / {4,3} / {5,5} / {6,5} / {7,5}",
                        this.Rang,
                        this.Team,
                        this.Pobede,
                        this.Porazi,
                        this.Bodovi,
                        this.ScoredPoints,
                        this.ReceivedPoints,
                        this.KosRazlika);
        }

        public void izracunajKosRazliku()
        {
            this.KosRazlika = this.ScoredPoints - this.ReceivedPoints;
        }
    }
}
