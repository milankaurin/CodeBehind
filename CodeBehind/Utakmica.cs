using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind
{
    public class Utakmica
    {
        public Tim Team1 { get; set; }
        public Tim Team2 { get; set; }
        public int Score1 { get; set; } // Poeni tima 1
        public int Score2 { get; set; } // Poeni tima 2

        public Tim? Pobednik {  get; set; }
        public Tim? Gubitnik { get; set; }

        public Utakmica (Tim t1, Tim t2)
        {
            Team1 = t1;
            Team2 = t2;
        }

        public void PlayMatch()
        {
            Random rnd = new Random();
            double verovatnoca = CalculateWinProbability(Team1, Team2);
             //ovo ce nam izgenerisati random broj izmedju -1 i 1
            

            if (verovatnoca > 0)  //ako je verovatnoca veca od nule to znaci da tim2 ima bolje rangiranje 
            {
                double randomFaktor = rnd.NextDouble() * 2 - 1;
                //rnd.nextdouble generise broj izmedju 0 i 1 i 
                Score1 = rnd.Next(70, 100);


                Score2 = Score1 + (verovatnoca > randomFaktor ?  +rnd.Next(1, 15) : -rnd.Next(1, 15));
            }

            else  //ako je verovatnoca manja od nule to znaci da tim1 ima bolje rangiranje
            {   

                //generisemo nasumicnu vrednost na intervalu izmedju -1 i 1 kodom ispod 
                double randomFaktor = rnd.NextDouble() * 2 - 1;
                Score2 = rnd.Next(50, 100);

                //i sada, ukoliko se recimo dogodi da nam je verovatnoca -0.5 to znaci da je tim1 osetno bolji, i uslov ce nam zadovoljiti bilo koja vrednost koju randomfaktor da izmedju -0.5 i 1 a opet postoji manja sansa da tim2 pobedi ako randomfaktor da vrednost izmedju -1 i -0.5  

                //sto je verovatnoca bliza -1, interval koji nam odgovara je veci 

                //isto je primenjeno i za gornji slucaj samo obrnuto, gore gledamo da sto nam je veca verovatnoca pobede tima2 to nam je interval veci


                Score1 = Score2 + (verovatnoca < randomFaktor  ? + rnd.Next(1,15) : - rnd.Next(1,15));

            }
            
            if (Score1 > Score2)
            {
                Team1.Bodovi += 2;
                Team1.Pobede++;
                Team2.Bodovi += 1;
                Team2.Porazi++;
                Pobednik = Team1;
                Gubitnik = Team2;
            }
            else
            {
                Team1.Bodovi += 1; 
                Team2.Pobede++;
                Team1.Porazi++; 
                Team2.Bodovi += 2;
                Pobednik = Team2;
                Gubitnik = Team1;
            }

            Team1.ScoredPoints += Score1;
            Team1.ReceivedPoints += Score2;
            Team2.ScoredPoints += Score2;
            Team2.ReceivedPoints += Score1;
        }

        private static double CalculateWinProbability(Tim team1, Tim team2)
        {
            double razlika = team1.FIBARanking - team2.FIBARanking;
            double maksRazlika = 29;


            return razlika/maksRazlika;   //sto je ovaj broj blizi broju 1 to je tim2 bolji a sto je broj blizi -1 to je tim1 bolji
        }

        



    }
}
