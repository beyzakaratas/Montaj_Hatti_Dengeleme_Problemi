using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace EMU_1_MATRIS
{
    class Program
    {
        private static int i;

        static void Main(string[] args)
        {
            StreamWriter Yaz2= new StreamWriter(@"D:\Çözümler\Özet.txt");
            Yaz2.WriteLine("Problem\tGS\tÇZ\tAFD\tCPU");
            Random rassal = new Random();
            string DosyaAdı = "1.Merten";
            string[] Problemler = Directory.GetFiles(@"D:\Problemler");
            for (int Problem = 0; Problem < Problemler.Length; Problem++)
            {
                string[] Dizi = Problemler[Problem].Split('\\');
                DosyaAdı=Dizi[Dizi.Length - 1];
                DosyaAdı = DosyaAdı.Substring(0, DosyaAdı.Length - 4);
                //GS: Görev Sayısı
                //OnMat: Öncelik Matrisi
                int GS;
                char Ayrıştırıcı = ',';
                // Dosyadan bilgileri ayrıştırarak okuyoruz.
                String[] AyrıştırılmışDizi;
                int[] ÇevrimZamanı = new int[0];
                String Satır = "";
                StreamReader Reader = new StreamReader(Problemler[Problem]);
                GS = Convert.ToInt32(Reader.ReadLine()); //görev sayısının okunması                                        
                int Popülasyon = 10*GS;                 //Popülasyon sayısını girdik.
                int JenerasyonSayısı = 20 * GS;
                //Tanımlamalar:
                int[,] Çözüm = new int[Popülasyon, GS];
                int[,] AtananGörev = new int[Popülasyon, GS];
                int[] Ç1Atanan = new int[GS];
                int[] Ç2Atanan = new int[GS];
                int[] AmaçFonkDeg = new int[Popülasyon];
                int[] AmaçFonkDegKopya = new int[Popülasyon];
                int[,] matris = new int[GS, GS];
                int[] Süre = new int[GS];                     //süre dizisi tanımlaması
                int[,] Öncelik = new int[GS, GS];             //öncelik matrisi tanımlaması
                int[] AGK = new int[GS];
                int[] YAGK = new int[GS];
                int[] YAGK2 = new int[GS];
                int[] E1 = new int[GS];
                int[] E2 = new int[GS];
                int[] E1Ç = new int[GS];
                int[] E2Ç = new int[GS];
                int ÇZ = 7;
                int BZ = 0;
                int Ebeveyn1;
                int Ebeveyn2;
                int Aday1 = 0;
                int Aday2 = 0;
                int Aday3 = 0;
                int Aday4 = 0;
                int DBaş = 0;
                int DBit = 0;
                int[] YeniPopülasyon = new int[Popülasyon];
                int[] AtananÇ1Mut = new int[GS];
                double MutOlası = 0.1;
                int[] Ç1Mut = new int[GS];
                int[] Ç2Mut = new int[GS];
                int Ç1Ras;
                int Ç2Ras;
                int EnİyiÇ = 0;
                int nerde = 0;

                //Dosyadan bilgileri alma işlemleri:
                for (int i = 0; i < GS; i++)
                {
                    Süre[i] = Convert.ToInt32(Reader.ReadLine());
                }
                while (Satır != "-1,-1")
                {
                    Satır = Reader.ReadLine();
                    if (Satır != "-1,-1")
                    {
                        AyrıştırılmışDizi = Satır.Split(Ayrıştırıcı);
                        Öncelik[Convert.ToInt32(AyrıştırılmışDizi[0]) - 1, Convert.ToInt32(AyrıştırılmışDizi[1]) - 1] = 1;
                    }
                }
                i = 0;
                while (Satır != "-1")
                {
                    Satır = Reader.ReadLine();
                    if (Satır != "-1")
                    {
                        Array.Resize(ref ÇevrimZamanı, ÇevrimZamanı.Length + 1);
                        ÇevrimZamanı[i] = Convert.ToInt32(Satır);
                        i++;
                    }

                }
                Reader.Close();

                //---------------------------------------------------------------------------------------------------------------------------
                for (int cevrim = 0; cevrim < ÇevrimZamanı.Length; cevrim++)
                {
                    Console.WriteLine("Şu an çözülen problem: " + DosyaAdı + ÇZ);
                    ÇZ = ÇevrimZamanı[cevrim];
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    for (int pop = 0; pop < Popülasyon; pop++)
                    {
                        int[] SütunToplamı = new int[GS];

                        for (int i = 0; i < GS; i++)
                        {
                            for (int j = 0; j < GS; j++)
                            {
                                SütunToplamı[i] += Öncelik[j, i];
                            }
                        }

                        for (int seçim = 0; seçim < GS; seçim++)
                        {
                            AGK = new int[0];

                            for (int i = 0; i < SütunToplamı.Length; i++)
                            {

                                if (SütunToplamı[i] == 0)
                                {
                                    Array.Resize(ref AGK, AGK.Length + 1);
                                    AGK[AGK.Length - 1] = i;
                                }
                            }
                            int indis = rassal.Next(AGK.Length);
                            Çözüm[pop, seçim] = AGK[indis];
                            for (int i = 0; i < GS; i++)
                            {
                                if (Öncelik[Çözüm[pop, seçim], i] == 1)
                                {
                                    SütunToplamı[i] -= 1;
                                }
                            }
                            SütunToplamı[Çözüm[pop, seçim]] = -1;
                        }
                    }

                    for (int pop = 0; pop < Popülasyon; pop++)
                    {
                        BZ = ÇZ;
                        int İstasyon = 1;
                        for (int j = 0; j < GS; j++)
                        {
                            if (Süre[Çözüm[pop, j]] <= BZ)
                            {
                                AtananGörev[pop, j] = İstasyon;
                                BZ = BZ - Süre[Çözüm[pop, j]];
                            }
                            else
                            {
                                İstasyon += 1;
                                BZ = ÇZ;
                                j--;
                            }
                        }
                        AmaçFonkDeg[pop] = İstasyon;
                    }

                    for (int jenerasyon = 0; jenerasyon < JenerasyonSayısı; jenerasyon++)
                    {
                        //1. Ebeveyni ve 2. Ebeveyni oluşturduk.
                        do
                        {
                            Aday1 = rassal.Next(Popülasyon - 1);
                            do
                            {
                                Aday2 = rassal.Next(Popülasyon - 1);

                            } while (Aday1 == Aday2);
                            if (AmaçFonkDeg[Aday1] <= AmaçFonkDeg[Aday2])
                            {
                                Ebeveyn1 = Aday1;
                            }
                            else
                                Ebeveyn1 = Aday2;

                            Aday3 = rassal.Next(Popülasyon - 1);
                            do
                            {
                                Aday4 = rassal.Next(Popülasyon - 1);

                            } while (Aday3 == Aday4);

                            if (AmaçFonkDeg[Aday3] <= AmaçFonkDeg[Aday4])
                            {
                                Ebeveyn2 = Aday3;
                            }
                            else
                                Ebeveyn2 = Aday4;
                        } while (Ebeveyn1 == Ebeveyn2);

                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //ÇAPRAZLAMA İŞLEMİ:
                        DBaş = rassal.Next(0, GS);
                        DBit = rassal.Next(0, GS);
                        if (DBaş > DBit)
                        {
                            int DBaşK = DBaş;
                            DBaş = DBit;
                            DBit = DBaşK;
                        }

                        for (int i = 0; i < GS; i++)
                        {
                            E1[i] = Çözüm[Ebeveyn1, i];
                            E2[i] = Çözüm[Ebeveyn2, i];
                        }
                        E1.CopyTo(E1Ç, 0);
                        E2.CopyTo(E2Ç, 0);

                        int uzunluk = (DBit - DBaş + 1);
                        int[] E1D = new int[uzunluk];
                        int[] E2D = new int[uzunluk];

                        Array.Copy(E1, DBaş, E1D, 0, uzunluk);
                        Array.Copy(E2, DBaş, E2D, 0, uzunluk);

                        int[] E1Sıra = new int[E2D.Length];
                        int[] E2Sıra = new int[E1D.Length];

                        //E1Çocuğu hesaplamak
                        int kaç = 0;
                        foreach (int kaçıncı in E1D)
                        {
                            E2Sıra[kaç] = Array.IndexOf(E2, kaçıncı); ;
                            kaç++;
                        }
                        Array.Sort(E2Sıra);

                        int sayı = 0;
                        for (int i = DBaş; i <= DBit; i++)
                        {
                            E1Ç[i] = E2[E2Sıra[sayı]];
                            sayı++;
                        }

                        //E2Çocuğu hesaplamak
                        int kaç2 = 0;
                        foreach (int kaçıncı2 in E2D)
                        {
                            E1Sıra[kaç2] = Array.IndexOf(E1, kaçıncı2);
                            kaç2++;
                        }
                        Array.Sort(E1Sıra);

                        int sayı2 = 0;
                        for (int i = DBaş; i <= DBit; i++)
                        {
                            E2Ç[i] = E1[E1Sıra[sayı2]];
                            sayı2++;
                        }
                        int[] KopyaSütunToplamı = new int[GS];
                        int[] KopyaSütunToplamı2 = new int[GS];
                        for (int i = 0; i < GS; i++)
                        {
                            for (int j = 0; j < GS; j++)
                            {
                                KopyaSütunToplamı[i] += Öncelik[j, i];
                                KopyaSütunToplamı2[i] += Öncelik[j, i];
                            }
                        }
                        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        //MUTASYON İŞLEMİ:
                        int BaşRas = 0;                             //Sıfırncı indisten itibaren
                        Ç1Ras = rassal.Next(0, GS);
                        Ç2Ras = rassal.Next(0, GS);
                        double RassalMut = rassal.NextDouble();
                        if (RassalMut < MutOlası)
                        {
                            //Çocuk 1 için mutasyon:
                            for (int i = 0; i <= Ç1Ras; i++)
                            {
                                KopyaSütunToplamı[E1Ç[i]] = -1;
                            }
                            // -1 yapma işlemi:
                            for (int i = 0; i <= Ç1Ras; i++)
                            {
                                for (int j = 0; j < GS; j++)
                                {
                                    if (Öncelik[E1Ç[i], j] == 1)
                                    {
                                        KopyaSütunToplamı[j] -= 1;
                                    }
                                }
                            }
                            //-2 yapma işlemi:
                            for (int i = Ç1Ras + 1; i < GS; i++)
                            {
                                YAGK = new int[0];
                                for (int j = 0; j < GS; j++)
                                {
                                    if (KopyaSütunToplamı[j] == 0)
                                    {
                                        Array.Resize(ref YAGK, YAGK.Length + 1);
                                        YAGK[YAGK.Length - 1] = j;
                                    }
                                }
                                E1Ç[i] = YAGK[rassal.Next(YAGK.Length)];
                                KopyaSütunToplamı[E1Ç[i]] = -1;
                                for (int k = 0; k < GS; k++)
                                {
                                    if (Öncelik[E1Ç[i], k] == 1)
                                    {
                                        KopyaSütunToplamı[k] -= 1;
                                    }
                                }
                            }
                        }
                        double RassalMut2 = rassal.NextDouble();
                        if (RassalMut2 < MutOlası)
                        {
                            //Çocuk 2 için mutasyon:
                            for (int i = 0; i <= Ç2Ras; i++)
                            {
                                KopyaSütunToplamı2[E2Ç[i]] = -1;

                            }
                            // -1 yapma işlemi:
                            for (int i = 0; i <= Ç2Ras; i++)
                            {
                                for (int j = 0; j < GS; j++)
                                {
                                    if (Öncelik[E2Ç[i], j] == 1)
                                    {
                                        KopyaSütunToplamı2[j] -= 1;
                                    }
                                }
                            }
                            //-2 yapma işlemi:
                            for (int i = Ç2Ras + 1; i < GS; i++)
                            {
                                YAGK2 = new int[0];
                                for (int j = 0; j < GS; j++)
                                {
                                    if (KopyaSütunToplamı2[j] == 0)
                                    {
                                        Array.Resize(ref YAGK2, YAGK2.Length + 1);
                                        YAGK2[YAGK2.Length - 1] = j;
                                    }
                                }
                                E2Ç[i] = YAGK2[rassal.Next(YAGK2.Length)];
                                KopyaSütunToplamı2[E2Ç[i]] = -1;
                                for (int k = 0; k < GS; k++)
                                {
                                    if (Öncelik[E2Ç[i], k] == 1)
                                    {
                                        KopyaSütunToplamı2[k] -= 1;
                                    }
                                }
                            }
                        }
                        // Çocuk1 için amaç fonksiyonu değeri hesaplanması:
                        BZ = ÇZ;
                        int İstasyon2 = 1;
                        for (int j = 0; j < GS; j++)
                        {
                            if (Süre[E1Ç[j]] <= BZ)
                            {
                                Ç1Atanan[j] = İstasyon2;
                                BZ = BZ - Süre[E1Ç[j]];
                            }
                            else
                            {
                                İstasyon2 += 1;
                                BZ = ÇZ;
                                j--;
                            }
                        }
                        // Çocuk2 için amaç fonksiyonu değeri hesaplanması:
                        BZ = ÇZ;
                        int İstasyon3 = 1;
                        for (int j = 0; j < GS; j++)
                        {
                            if (Süre[E2Ç[j]] <= BZ)
                            {
                                Ç2Atanan[j] = İstasyon3;
                                BZ = BZ - Süre[E2Ç[j]];
                            }
                            else
                            {
                                İstasyon3 += 1;
                                BZ = ÇZ;
                                j--;
                            }
                        }
                        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                        // en iyi çocuğu karşılaştırıp bulma işlemi:
                        if (İstasyon2 <= İstasyon3)
                        {
                            EnİyiÇ = İstasyon2;
                        }
                        else
                        {
                            EnİyiÇ = İstasyon3;
                        }
                        AmaçFonkDeg.CopyTo(AmaçFonkDegKopya, 0);
                        Array.Sort(AmaçFonkDegKopya);
                        if (EnİyiÇ < AmaçFonkDegKopya[AmaçFonkDegKopya.Length - 1])
                        {
                            nerde = Array.IndexOf(AmaçFonkDeg, AmaçFonkDegKopya[AmaçFonkDegKopya.Length - 1]);
                        }
                        for (int i = 0; i < GS; i++)
                        {
                            if (EnİyiÇ == İstasyon2)
                            {
                                Çözüm[nerde, i] = E1Ç[i];
                                AmaçFonkDeg[nerde] = İstasyon2;
                                AtananGörev[nerde, i] = Ç1Atanan[i];
                            }
                            else if (EnİyiÇ == İstasyon3)
                            {
                                Çözüm[nerde, i] = E2Ç[i];
                                AmaçFonkDeg[nerde] = İstasyon3;
                                AtananGörev[nerde, i] = Ç2Atanan[i];
                            }
                        }
                    }
                    AmaçFonkDeg.CopyTo(AmaçFonkDegKopya, 0);
                    Array.Sort(AmaçFonkDegKopya);
                    int Enİyi = Array.IndexOf(AmaçFonkDeg, AmaçFonkDegKopya[0]);
                    sw.Stop();
                    StreamWriter Yaz1 = new StreamWriter(@"D:\Çözümler\" + DosyaAdı + "_" + ÇZ + ".txt");
                    Yaz1.WriteLine("Amaç Fonksiyonu Değeri : " + AmaçFonkDeg[Enİyi]);
                    Yaz1.WriteLine("Çözüm Süresi: " + sw.Elapsed.ToString());
                    Yaz1.WriteLine("Atamalar:");
                    Yaz1.Write("Görev");
                    Yaz1.Write("                            ");
                    Yaz1.WriteLine("İstasyon");
                    for (int i = 0; i < GS; i++)
                    {
                        Yaz1.Write(Çözüm[Enİyi, i]);
                        Yaz1.Write("                                ");
                        Yaz1.WriteLine(AtananGörev[Enİyi, i]);
                    }
                    Yaz1.Close();
                    Yaz2.WriteLine(DosyaAdı + "\t" + GS + "\t" + ÇZ + "\t" + AmaçFonkDeg[Enİyi] + "\t" + sw.Elapsed.ToString());
                }
            }
            Yaz2.Close();
            Console.WriteLine("bitti");
            Console.ReadLine();
        }
    }
}
