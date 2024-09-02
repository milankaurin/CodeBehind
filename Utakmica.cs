using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehind
{
#pragma warning disable CS8618
#pragma warning disable CS8603

#pragma warning disable CS8604
#pragma warning disable CS8602

    public class Utakmica
    {
        public Tim Team1 { get; set; }
        public Tim Team2 { get; set; }
        public int Score1 { get; set; } // Poeni tima 1
        public int Score2 { get; set; } // Poeni tima 2

        public Tim? Pobednik {  get; set; }
        public Tim? Gubitnik { get; set; }

        Random rnd = new Random();
        public Utakmica (Tim t1, Tim t2)
        {
            Team1 = t1;
            Team2 = t2;
        }

        public void PlayMatch()
        {
            
            double verovatnoca = CalculateWinProbability(Team1, Team2);
            //ovo ce nam izgenerisati random broj izmedju -1 i 1


            if (verovatnoca > 0)  //ako je verovatnoca veca od nule to znaci da tim2 ima bolje rangiranje 


            {
                double randomFaktor = rnd.NextDouble() * 2 - 1;
                //rnd.nextdouble generise broj izmedju 0 i 1 i 
                Score1 = (int)Math.Round((rnd.Next(70, 100)*Team1.Forma));  //mnozemo dobijeme poene sa formom

                //ukoliko random faktor izgenerise povoljnu vrednost (bilo sta manje izmedju -1 i verovatnoce koja je >0) timu 2 ce biti dodati kosevi a ako izgenerise nepovoljnu bice mu oduzeti

                //POBOLJSANJE MODELA -- uzecemo u obzir i da kos razlika bude zavisna od verovatnoce, dakle nece biti fiksno 1-15 koseva dodato, nego cemo to skalirati



                if (verovatnoca > 0.5)   //ovde mozemo da kazemo da je dosta bolji tim 2 pa cemo mu omoguciti ubedljiviju pobedu, do cak 30 koseva razlike, a ako izgube, maksimalna razlika ce biti 15 poenta
                {
                    Score2 = (int)Math.Round((Score1 + (verovatnoca > randomFaktor ? +rnd.Next(5, 30) : -rnd.Next(1, 15))) * Team2.Forma);  // mnozenje dobijenih poena sa formom
                }
                else  //ovde je verovatnoca izmedju 0 i 0.5 sto znaci da nije mnogo bolji tim2 pa cemo ogranicti maksimalnu pobedu sa 15 koseva razlike, gubitak je isti 
                {

                    Score2 = Score1 + (verovatnoca > randomFaktor ? +rnd.Next(1, 15) : -rnd.Next(1, 15));
                }
            }

            else  //ako je verovatnoca manja od nule to znaci da tim1 ima bolje rangiranje
            {

                //generisemo nasumicnu vrednost na intervalu izmedju -1 i 1 kodom ispod 
                double randomFaktor = rnd.NextDouble() * 2 - 1;
                Score2 = rnd.Next(70, 100);

                //i sada, ukoliko se recimo dogodi da nam je verovatnoca manja od -0.5 to znaci da je tim1 osetno bolji, i uslov ce nam zadovoljiti bilo koja vrednost koju randomfaktor da izmedju -0.5 i 1 a opet postoji neka sansa da tim2 pobedi ako randomfaktor da vrednost izmedju -1 i -0.5  

                //sto je verovatnoca bliza -1, interval koji nam odgovara je veci 

                //poboljsanje modela 

                if (verovatnoca < -0.5) {  //ovde takodje poboljsavamo model, tako da ako je verovatnoca manja od -0.5 tada razlika moze biti ubedljiva sa do 30 koseva razlike u korist tima1
                    Score1 = Score2 + (verovatnoca < randomFaktor ? +rnd.Next(5, 30) : -rnd.Next(1, 15));

                }

                else
                {

                    Score1 = Score2 + (verovatnoca < randomFaktor ? +rnd.Next(1, 15) : -rnd.Next(1, 15));
                }
            }

            //zakljucak: model je sada dovoljno fleksibilan da omogucava neizvenost a sa druge strane verovatno je da ce bolji timovi pobediti, dok je razlika u kosevima poprilicno odredjena razlikom u fiba rankingu


            
            if (Score1 > Score2)
            {
                Team1.Bodovi += 2;
                Team1.Pobede++;
                Team2.Bodovi += 1;
                Team2.Porazi++;
                Pobednik = Team1;
                Gubitnik = Team2;
                Team1.Forma = Team1.Forma * 1.04;
                if (Team1.Forma > 1.15) { Team1.Forma = 1.15; }
                Team2.Forma = Team2.Forma * 0.97;
                if (Team2.Forma < 0.95) { Team2.Forma = 0.95; }
            }
            else
            {
                Team1.Bodovi += 1; 
                Team2.Pobede++;
                Team1.Porazi++; 
                Team2.Bodovi += 2;
                Pobednik = Team2;
                Gubitnik = Team1;
                Team2.Forma = Team2.Forma * 1.04;
                if (Team2.Forma > 1.15) { Team2.Forma = 1.15; }
                Team1.Forma = Team1.Forma * 0.98;
                if (Team1.Forma < 0.95) { Team1.Forma = 0.95; }
            }

            Team1.ScoredPoints += Score1;
            Team1.ReceivedPoints += Score2;
            Team2.ScoredPoints += Score2;
            Team2.ReceivedPoints += Score1;
        }

        private static double CalculateWinProbability(Tim team1, Tim team2)
        {
            double razlika = team1.FIBARanking - team2.FIBARanking;
            double maksRazlika = 34; // -  iako je zapravu u nasem json fajlu maks razlika u fiba rangu 33, ja sam se odlucio da postavim tu vrednost na 34, da CAK i u utakmici juzni sudan-usa, juzni sudan ima malu malenu sansu za pobedu
            

           


            return razlika/maksRazlika;   //sto je ovaj broj blizi broju 1 to je tim2 bolji a sto je broj blizi -1 to je tim1 bolji
        }

        



    }
}
