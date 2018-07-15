using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MoneySupervisor
{
    class Program
    {
        public static int staticLanguage = (int)MSLang.RU;

        public static Dictionary<string, MSLanguage> dictionary = new Dictionary<string, MSLanguage>();

        public static List<MSCategory> categories = new List<MSCategory>();
        public static List<MSAccount> accounts = new List<MSAccount>();

        public static ConsoleKeyInfo cki;

        static void Main(string[] args)
        {
            Console.Title = "Управление деньгами";
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            MSLanguage.CreateDictionary(ref dictionary);

            MSIntro.Show();
            MSMainMenyu();
        }

        static void MSMainMenyu()
        {
            Console.SetWindowSize(80, 25);
            Console.SetBufferSize(80, 25);
            
            bool xreplace = true;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                string str = "Добро пожаловать!";

                for (int i = 0; i < 65; i++)
                {
                    Console.Write(str);
                    Thread.Sleep(100);
                    Console.SetCursorPosition(i-1 < 0 ? 0 : i-1, 0);
                    Console.Write("                 ");
                    Console.SetCursorPosition(i, 0);
                }


                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey();
                }

                Console.WriteLine($"1. {dictionary["newcategory"].RetLang(staticLanguage)} ");
                Console.WriteLine($"2. {dictionary["newaccount"].RetLang(staticLanguage)} ");

                switch (cki.KeyChar)
                {
                    case '1':
                        break;
                    default:
                        break;
                }
                //Console.WriteLine($"3. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"4. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"5. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"6. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.ReadKey();
            } while (xreplace);
        }
    }
}
