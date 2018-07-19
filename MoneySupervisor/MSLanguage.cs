using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MoneySupervisor
{
    public enum MSLang { RU = 0, AZ, EN, KO }
    //[DataContract]
    public class MSLanguage
    {
        //[DataMember]
        public string RUS { get; set; }
        //[DataMember]
        public string AZE { get; set; }
        //[DataMember]
        public string ENG { get; set; }
        //[DataMember]
        public string KOR { get; set; }

        public MSLanguage()
        {
        }

        public MSLanguage(string rUS, string aZE, string eNG, string kOR)
        {
            RUS = rUS;
            AZE = aZE;
            ENG = eNG;
            KOR = kOR;
        }

        public string RetLang(int lang)
        {
            if (lang == 0)
            {
                return RUS;
            }
            else if (lang == 1)
            {
                return AZE;
            }
            else if (lang == 2)
            {
                return ENG;
            }
            else return KOR;
        }

        public static int ChooseLanguage()
        {
            List<string> msCurrenciesCodeList = new List<string>() { "RU", "AZ", "EN", "KO" };
            int left = Console.CursorLeft;
            int top  = Console.CursorTop;
            int xIndex = 0;
            string xSynbol = "↓ "; // ↓   ↑   ↓↑
            Console.WriteLine($"{msCurrenciesCodeList[xIndex]} {xSynbol}");
            do
            {
                Program.cki = Console.ReadKey();
                switch (Program.cki.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (++xIndex >= msCurrenciesCodeList.Count) xIndex = msCurrenciesCodeList.Count - 1;
                        break;
                    case ConsoleKey.UpArrow:
                        if (--xIndex < 0) xIndex = 0;
                        break;
                    case ConsoleKey.Enter:
                        Program.cki = default(ConsoleKeyInfo);
                        Console.SetCursorPosition(0, top + 1);
                        return xIndex;
                    default:
                        break;
                }
                if (xIndex == 0) xSynbol = "↓ ";
                else if (xIndex >= msCurrenciesCodeList.Count - 1) xSynbol = "↑ ";
                else xSynbol = "↓↑";
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"{msCurrenciesCodeList[xIndex]} {xSynbol}");
            } while (true);
        }

        public static void CreateDictionary(ref Dictionary<string, MSLanguage> dictionary)
        {
            MSLanguage phrase1 = new MSLanguage
            {
                RUS = "Новая категория",
                AZE = "Yeni kateqoriya",
                ENG = "New category",
                KOR = "새 카테고리"
            };
            dictionary.Add("newcategory", phrase1);

            MSLanguage phrase2 = new MSLanguage
            {
                RUS = "Новый счёт",
                AZE = "Yeni hesab",
                ENG = "New account",
                KOR = "새 계정"
            };
            dictionary.Add("newaccount", phrase2);

            MSLanguage phrase3 = new MSLanguage
            {
                RUS = "",
                AZE = "",
                ENG = "",
                KOR = ""
            };
            dictionary.Add("", phrase3);

            MSLanguage phrase4 = new MSLanguage
            {
                RUS = "",
                AZE = "",
                ENG = "",
                KOR = ""
            };
            dictionary.Add(" ", phrase4);
        }
    }
}
