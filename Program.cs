using System.ComponentModel.Design;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace CodeBehind
{
#pragma warning disable CS8618
#pragma warning disable CS8603

#pragma warning disable CS8604
#pragma warning disable CS8602


    internal class Program
    {
        public static List<Utakmica> TekmeGrupaA = new List<Utakmica>();
        public static List<Utakmica> TekmeGrupaB = new List<Utakmica>();
        public static List<Utakmica> TekmeGrupaC = new List<Utakmica>();

        public static List<Tim> prosliDalje = new List<Tim>();

        public static List<Tim> SesirD = new List<Tim>();
        public static List<Tim> SesirE = new List<Tim>();
        public static List<Tim> SesirF = new List<Tim>();
        public static List<Tim> SesirG = new List<Tim>();
        public static List<Tim> grupaA, grupaB, grupaC;
        public static List<Tuple<Tim, Tim>> cetvrtDG=new List<Tuple<Tim, Tim>>();
        public static List<Tuple<Tim, Tim>> cetvrtEF = new List<Tuple<Tim, Tim>>();
        public static List<Tuple<Tim,Tim>> LevaGrana = new List<Tuple<Tim, Tim>>();
        public static List<Tuple<Tim, Tim>> DesnaGrana = new List<Tuple<Tim, Tim>>();

        public static Random random = new Random();
        public static void Main(string[] args)
        {
            string jsonFilePath = "groups.json";
            Turnir turnir = LoadTournamentFromJson(jsonFilePath);
            
            PrintTeams(turnir.A, "Grupa A");
            PrintTeams(turnir.B, "Grupa B");
            PrintTeams(turnir.C, "Grupa C");
            Console.WriteLine("\n\n\n Započinjem turnir...\n\n\n");


            SimulacijaKola(turnir);
            DodeliGrupuTimovima();
            DodajUProsliDaljeIRangirajPaPrebaciUSesire();

            Console.WriteLine("\n\n\n\n PROŠLI DALJE:");
            for (int i = 0; i < prosliDalje.Count; i++)
            {
                if (i == 8) Console.Write("ispao---");
                Console.WriteLine(prosliDalje[i].ToString());
               
            }

            ispisiSesire();
            promesajSesireUkrstiCetvrt();
            PromesajZaPoluFinala();
            //odmah nakon ukrstanaja za cetvrtfinale odredjujemo dve grane za polufinale
            simulacijaPlayOff();





        }

        public static Turnir LoadTournamentFromJson(string filePath)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Turnir>(jsonString, options);
            
        }

        public static void DodeliGrupuTimovima()
        {
            foreach(Tim t in grupaA)
            {
                t.Grupa = grupaA;
            }
            foreach (Tim t in grupaB)
            {
                t.Grupa = grupaB;
            }
            foreach (Tim t in grupaC)
            {
                t.Grupa = grupaC;
            }
        }

        private static void PrintTeams(List<Tim> teams, string groupName)
        {
            Console.WriteLine($"{groupName}:");
            foreach (var team in teams)
            {
                Console.WriteLine($"{team.Team} ({team.ISOCode}) - FIBA Ranking: {team.FIBARanking}");
            }
            Console.WriteLine();
        }


        public static void SimulacijaKola(Turnir turnir)
        {
            grupaA = turnir.A;
            grupaB = turnir.B;
            grupaC = turnir.C;

            //PRIMENA ROUND ROBIN-a
            for (int i = 1; i <= 3; i++) //ukupno cemo imati tri kola da bi svaka ekipa igrala sa svakom jednom
            {
                Console.WriteLine("\nGrupna faza - " + i.ToString() + ". kolo:\n");
                Console.WriteLine(" Grupa A:");
                for (int j = 0; j < 2; j++)  //a imacemo dve utakmice po kolu
                {

                    Tim tim1 = grupaA[j];
                    Tim tim2 = grupaA[grupaA.Count - 1 - j];
                    Utakmica utakmica = new Utakmica(tim1, tim2);
                    utakmica.PlayMatch();
                    Console.WriteLine($"  {tim1.Team} - {tim2.Team} ({utakmica.Score1}:{utakmica.Score2})");
                    TekmeGrupaA.Add(utakmica);

                }
                grupaA = RotacijaZaRoundRobimSemPrvog(grupaA);

                Console.WriteLine("\n Grupa B:");
                for (int j = 0; j < 2; j++)  //a imacemo dve utakmice po kolu
                {

                    Tim tim1 = grupaB[j];
                    Tim tim2 = grupaB[grupaB.Count - 1 - j];
                    Utakmica utakmica = new Utakmica(tim1, tim2);
                    utakmica.PlayMatch();
                    Console.WriteLine($"  {tim1.Team} - {tim2.Team} ({utakmica.Score1}:{utakmica.Score2})");
                    TekmeGrupaB.Add(utakmica);
                }
                grupaB = RotacijaZaRoundRobimSemPrvog(grupaB);

                Console.WriteLine("\n Grupa C:");
                for (int j = 0; j < 2; j++)  //a imacemo dve utakmice po kolu
                {

                    Tim tim1 = grupaC[j];
                    Tim tim2 = grupaC[grupaC.Count - 1 - j];
                    Utakmica utakmica = new Utakmica(tim1, tim2);
                    utakmica.PlayMatch();
                    Console.WriteLine($"  {tim1.Team} - {tim2.Team} ({utakmica.Score1}:{utakmica.Score2})");
                    TekmeGrupaC.Add(utakmica);
                }
                grupaC = RotacijaZaRoundRobimSemPrvog(grupaC);
            }

            Console.WriteLine("\nZavršena je cela grupna faza!\nKonačan plasman po grupama:");

            //RACUNANJE KOS RAZLIKE, RANGIRANJE TIMOVA UNUTAR GRUPA, ISPIS GRUPA

            Console.WriteLine("\n    Grupa A (Ime tima    Pobede / Porazi / Bodovi / ScoredPoints / ReceivedPoints / KosRazlika)\n");
            kosRazlikaGrupa(grupaA);
           grupaA= Rangiraj(grupaA, TekmeGrupaA);

            for (int i = 0; i < grupaA.Count; i++)
            {
                Console.WriteLine(grupaA[i].ToString());
            }



            Console.WriteLine("\n    Grupa B (Ime tima    Pobede / Porazi / Bodovi / ScoredPoints / ReceivedPoints / KosRazlika)\n");
            kosRazlikaGrupa(grupaB);
           grupaB= Rangiraj(grupaB, TekmeGrupaB);
            for (int i = 0; i < grupaB.Count; i++)
            {
                Console.WriteLine(grupaB[i].ToString());
            }


            Console.WriteLine("\n    Grupa C (Ime tima    Pobede / Porazi / Bodovi / ScoredPoints / ReceivedPoints / KosRazlika)\n");
            kosRazlikaGrupa(grupaC);
            grupaC = Rangiraj(grupaC, TekmeGrupaC);
            for (int i = 0; i < grupaC.Count; i++)
            {

                Console.WriteLine(grupaC[i].ToString());
            }





        }


        public static List<Tim> RotacijaZaRoundRobimSemPrvog(List<Tim> grupa)   //rotacija timova u grupi (neophodno za round robin metodu (prvi element ostaje fiksiran, poslednji ubacujemo na drugo mesto i tako u krug)
        {
            Tim poslednji = grupa.Last();
            grupa.Remove(grupa.Last());
            grupa.Insert(1, poslednji);
            return grupa;

        }

        public static List<Tim> Rangiraj(List<Tim> grupa, List<Utakmica> tekmeGrupna)
        {

            // U slučaju da 3 tima iz iste grupe imaju isti broj bodova, kriterijum za rangiranje biće razlika u poenima u međusobnim utakmicama između ta 3 tima (takozvano formiranje kruga).

            if (triTimaIstiBodovi(grupa) == true)
            {

                grupa = grupa.OrderByDescending(t => t.Bodovi).ToList();

                if ((grupa[0].Bodovi == grupa[1].Bodovi) && (grupa[1].Bodovi == grupa[2].Bodovi))  // -- prva tri tima imaju isti broj bodova
                {
                    //pom0     pom1    pom2
                    int prvi = 0, drugi = 0, treci = 0;

                    foreach (Utakmica t in tekmeGrupna)
                    {
                        if ((t.Team1 == grupa[0] && t.Team2 == grupa[1]) || (t.Team1 == grupa[1] && t.Team2 == grupa[0]))
                        {
                            prvi += t.Team1 == grupa[0] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                            drugi += t.Team1 == grupa[1] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                        }
                        else if ((t.Team1 == grupa[0] && t.Team2 == grupa[2]) || (t.Team1 == grupa[2] && t.Team2 == grupa[0]))
                        {
                            prvi += t.Team1 == grupa[0] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                            treci += t.Team1 == grupa[2] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                        }
                        else if ((t.Team1 == grupa[1] && t.Team2 == grupa[2]) || (t.Team1 == grupa[2] && t.Team2 == grupa[1]))
                        {
                            drugi += t.Team1 == grupa[1] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                            treci += t.Team1 == grupa[2] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                        }


                    }
                    grupa[0].pomocnaRazlika = prvi;
                    grupa[1].pomocnaRazlika = drugi;
                    grupa[2].pomocnaRazlika = treci;
                    
                    grupa = grupa.OrderByDescending(t => t.Bodovi).ThenByDescending(t=>t.pomocnaRazlika).ToList();
                    foreach (Tim tim in grupa)
                    {
                        tim.Rang = grupa.IndexOf(tim) + 1;
                       
                    }
                    return grupa;



                }
            
            else if ((grupa[1].Bodovi == grupa[2].Bodovi) && (grupa[1].Bodovi == grupa[3].Bodovi))  // -- poslednja tri tima imaju isti broj bodova
            {

                int drugi = 0, treci = 0, cetvrti = 0;
                foreach (Utakmica t in tekmeGrupna)
                {
                    if ((t.Team1 == grupa[1] && t.Team2 == grupa[2]) || (t.Team1 == grupa[2] && t.Team2 == grupa[1]))
                    {
                        drugi += t.Team1 == grupa[1] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                        treci += t.Team1 == grupa[2] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                    }
                    else if ((t.Team1 == grupa[1] && t.Team2 == grupa[3]) || (t.Team1 == grupa[3] && t.Team2 == grupa[1]))
                    {
                        drugi += t.Team1 == grupa[1] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                        cetvrti += t.Team1 == grupa[3] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                    }
                    else if ((t.Team1 == grupa[2] && t.Team2 == grupa[3]) || (t.Team1 == grupa[3] && t.Team2 == grupa[2]))
                    {
                        treci += t.Team1 == grupa[2] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                        cetvrti += t.Team1 == grupa[3] ? t.Score1 - t.Score2 : t.Score2 - t.Score1;
                    }
                }

                grupa[1].pomocnaRazlika = drugi;
                grupa[2].pomocnaRazlika = treci;
                grupa[3].pomocnaRazlika = cetvrti;
                grupa = grupa.OrderByDescending(t => t.Bodovi).ThenByDescending(a => a.pomocnaRazlika).ToList();
                foreach (Tim tim in grupa)
                {
                    tim.Rang = grupa.IndexOf(tim) + 1;
                }
                return grupa;



            }






        }

        //Timovi u okviru grupe se rangiraju na osnovu broja bodova. U slučaju da dva tima iz iste grupe imaju isti broj bodova, rezultat međusobnog susreta će biti korišćen kao kriterijum za rangiranje.


        Tim cuvar;
            for (int i = 0; i < grupa.Count - 1; i++)
            {
                for (int j = i + 1; j < grupa.Count; j++)
                {
                    if (grupa[j].Bodovi > grupa[i].Bodovi)
                    {
                        cuvar = grupa[i];
                        grupa[i] = grupa[j];
                        grupa[j] = cuvar;

                        continue;
                    }
                    if (grupa[j].Bodovi == grupa[i].Bodovi)
                    {
                        if (koJePobedio(tekmeGrupna, grupa[j], grupa[i]) == grupa[j])
                        {
                            cuvar = grupa[i];
                            grupa[i] = grupa[j];
                            grupa[j] = cuvar;

                        }
                    }

                }

            }

            foreach (Tim tim in grupa)
            {

                tim.Rang = grupa.IndexOf(tim) + 1;
            }

            return grupa;

        }



        public static Tim koJePobedio(List<Utakmica> utakmica, Tim a, Tim b)
        {
            for (int i = 0; i < utakmica.Count; i++)
            {
                if (utakmica[i].Team1 == a && utakmica[i].Team2 == b)
                {
                    if (utakmica[i].Score1 > utakmica[i].Score2)
                    {
                        return a;
                    }
                    else return b;

                }
                if (utakmica[i].Team2 == a && utakmica[i].Team1 == b)
                {

                    if (utakmica[i].Score1 > utakmica[i].Score2)
                    {
                        return b;
                    }
                    else return a;

                }
            }

            return null;

        }


        public static bool triTimaIstiBodovi(List<Tim> grupa)
        {

            List<Tim> pom = grupa.OrderBy(t => t.Bodovi).ToList();

            if (((pom[0].Bodovi == pom[1].Bodovi) && (pom[1].Bodovi == pom[2].Bodovi)) || ((pom[1].Bodovi == pom[2].Bodovi) && (pom[1].Bodovi == pom[3].Bodovi)))
            {
                return true;
            }

            return false;

        }

        public static void kosRazlikaGrupa(List<Tim> lista)
        {
            foreach (Tim t in lista)
            {
                t.izracunajKosRazliku();
            }
        }


        public static void DodajUProsliDaljeIRangirajPaPrebaciUSesire()
        {

            //dodavanje u pomocni prosliDalje listu
            for (int i = 0; i < 3; i++)
            {
                List<Tim> pom = new List<Tim>();
                pom.Add(grupaA[i]);
                pom.Add(grupaB[i]);
                pom.Add(grupaC[i]);
                pom = pom.OrderByDescending(t => t.Bodovi).ThenByDescending(t => t.KosRazlika).ThenByDescending(t => t.ScoredPoints).ToList();
                prosliDalje.AddRange(pom);
            }

            for (int i = 0; i < prosliDalje.Count; i++)
            {

                prosliDalje[i].Rang = i + 1;

                switch (prosliDalje[i].Rang)
                {
                    case < 3:
                        SesirD.Add(prosliDalje[i]);
                        break;
                    case < 5:
                        SesirE.Add(prosliDalje[i]);
                        break;
                    case < 7:
                        SesirF.Add(prosliDalje[i]);
                        break;
                    case < 9:
                        SesirG.Add(prosliDalje[i]);
                        break;
                    default:
                        break;
                }

            }
        }

        public static void RangirajPoTri(List<Tim> lista, int a)
        {
            for (int i = a; i < a + 3; i++)
            {

            }

        }

        public static void ispisiSesire()
        {
            Console.WriteLine("\nŠesiri:");
            Console.WriteLine(" Šesir D:");
            foreach (Tim t in SesirD)
            {
                Console.WriteLine($"   {t.Team}");

            }
            Console.WriteLine(" Šesir E:");
            foreach (Tim t in SesirE)
            {
                Console.WriteLine($"   {t.Team}");

            }
            Console.WriteLine(" Šesir F:");
            foreach (Tim t in SesirF)
            {
                Console.WriteLine($"   {t.Team}");

            }
            Console.WriteLine(" Šesir G:");
            foreach (Tim t in SesirG)
            {
                Console.WriteLine($"   {t.Team}");

            }


        }
    
        public static void promesajSesireUkrstiCetvrt()  
        {
            //ovako cemo da "izmesamo" raspored timova u svakom sesiru kako bismo kasnije mogli da spojimo index0 sa indexom0 i index1 sa indexom1 

            List<List<Tim>> li = new List<List<Tim>>();
            li.Add(SesirD);
            li.Add(SesirE);
            li.Add(SesirF);
            li.Add(SesirG);
            foreach (List<Tim> l in li)
            {
                
                int a = random.Next(0, 2);
                Tim cuvar;
                cuvar = l[0];
                l[0] = l[a];
                l[a] = cuvar;
            }

//Timovi iz šešira D se nasumično ukrštaju sa timovima iz šešira G, a timovi iz šešira E sa timovima iz šešira F i tako se formiraju parovi četvrtfinala.Veoma važna propozicija je da se timovi koji su igrali međusobno u grupnoj fazi, ne mogu sresti u četvrtfinalu.

//U istom trenutku se formiraju i parovi polufinala, nasumičnim ukrštanjem novonastalih parova četvrtfinala, uz pravilo da se parovi nastali ukrštanjem šešira D i E ukrštaju sa parovima nastalim ukrštanjem šešira F i G.

            if (daLiSuIstaGrupa(SesirD[0], SesirG[0]) == false && daLiSuIstaGrupa(SesirD[1], SesirG[1]) == false)
            {
                cetvrtDG.Add(new Tuple<Tim, Tim>(SesirD[0], SesirG[0]));
                cetvrtDG.Add(new Tuple<Tim, Tim>(SesirD[1], SesirG[1]));
            }
            else
            {
                cetvrtDG.Add(new Tuple<Tim, Tim>(SesirD[0], SesirG[1]));
                cetvrtDG.Add(new Tuple<Tim, Tim>(SesirD[1], SesirG[0]));

            }

            if (daLiSuIstaGrupa(SesirE[0], SesirF[0]) == false && daLiSuIstaGrupa(SesirE[1], SesirF[1]) == false)
            {
                cetvrtEF.Add(new Tuple<Tim, Tim>(SesirE[0], SesirF[0]));
                cetvrtEF.Add(new Tuple<Tim, Tim>(SesirE[1], SesirF[1]));
            }
            else
            {
                cetvrtEF.Add(new Tuple<Tim, Tim>(SesirE[0], SesirF[1]));
                cetvrtEF.Add(new Tuple<Tim, Tim>(SesirE[1], SesirF[0]));

            }

            

           
        }
    
        
        
        public static void PromesajZaPoluFinala()
        {
            
            int a = random.Next(0, 2);
            LevaGrana.Add(cetvrtDG[a]);     //nasumican par iz DG dela stavljamo u levu granu a ovaj drugi u desnu
            DesnaGrana.Add(cetvrtDG[Math.Abs(a - 1)]);
            a=random.Next(0,2);             //nasumican par iz EF dela stavljamo u levu granu a ovaj drugi u desnu
            LevaGrana.Add(cetvrtEF[a]);
            DesnaGrana.Add(cetvrtEF[Math.Abs(a - 1)]);

            //ispis eliminacione faze:

            Console.WriteLine("\n\nEliminaciona faza:");
            foreach (Tuple<Tim, Tim> t in LevaGrana)
            { Console.WriteLine($"  {t.Item1.Team} - {t.Item2.Team}");
            }
            Console.WriteLine();
            foreach (Tuple<Tim, Tim> t in DesnaGrana)
            {
                Console.WriteLine($"  {t.Item1.Team} - {t.Item2.Team}");
            }
        }

        public static bool daLiSuIstaGrupa(Tim t1, Tim t2)
        {
            if (t1.Grupa == t2.Grupa) { return true; }
            return false;
        }

        public static void simulacijaPlayOff()
        {
            List<Tim> polufinalistiRedom = new List<Tim>();  //ovde smo kreirali listu redom ali to nam je u redu jer redom smestamo pobednike pa cemo upariti prvog polufinalistu sa drugim i treceg sa cetvrtim
            Console.WriteLine("\n\nČetvrtfinale:");

            foreach (Tuple<Tim, Tim> par in LevaGrana)
            {
                Utakmica ut1 = new Utakmica(par.Item1, par.Item2);
                ut1.PlayMatch();
                Console.WriteLine($"  {ut1.Team1.Team} - {ut1.Team2.Team} ({ut1.Score1}:{ut1.Score2})");
                polufinalistiRedom.Add(ut1.Pobednik);
            }
            Console.WriteLine();
            foreach (Tuple<Tim, Tim> par in DesnaGrana)
            {
                Utakmica ut1 = new Utakmica(par.Item1, par.Item2);
                ut1.PlayMatch();
                Console.WriteLine($"  {ut1.Team1.Team} - {ut1.Team2.Team} ({ut1.Score1}:{ut1.Score2})");
                polufinalistiRedom.Add(ut1.Pobednik);
            }

            Console.WriteLine("\n\nPolufinale");
            Utakmica polu1 = new Utakmica(polufinalistiRedom[0], polufinalistiRedom[1]);
            Utakmica polu2 = new Utakmica(polufinalistiRedom[2], polufinalistiRedom[3]);
            polu1.PlayMatch();
            Console.WriteLine($"  {polu1.Team1.Team} - {polu1.Team2.Team} ({polu1.Score1}:{polu1.Score2})");
            polu2.PlayMatch();
            Console.WriteLine($"  {polu2.Team1.Team}  -  {polu2.Team2.Team} ({polu2.Score1} : {polu2.Score2})");

            Console.WriteLine("\n\nUtakmica za treće mesto:");

            Utakmica zaTrece = new Utakmica(polu1.Gubitnik, polu2.Gubitnik);
            zaTrece.PlayMatch();
            Console.WriteLine($"  {zaTrece.Team1.Team}  -  {zaTrece.Team2.Team} ({zaTrece.Score1} : {zaTrece.Score2})");
            
            Console.WriteLine("\n\nFinale:");
            Utakmica finale = new Utakmica(polu1.Pobednik, polu2.Pobednik);
            finale.PlayMatch();
            Console.WriteLine($"  {finale.Team1.Team}  -  {finale.Team2.Team} ({finale.Score1} : {finale.Score2})");

            Console.WriteLine($"\n\n\nMedalje: \n   1. {finale.Pobednik.Team}\n   2. {finale.Gubitnik.Team}\n   3. {zaTrece.Pobednik.Team}");
        }

     

    }
}
