using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneySupervisor
{
    class Program
    {
        public static int staticLanguage = (int)MSLang.RU;

        public static Dictionary<string, MSLanguage> dictionary = new Dictionary<string, MSLanguage>();

        public static List<MSCategory> categories = new List<MSCategory>();
        public static List<MSAccount> accounts = new List<MSAccount>();

        static void Main(string[] args)
        {
            Console.Title = "Управление деньгами";
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            MSIntro.Show();
            MSMainMenyu();
        }

        static void MSMainMenyu()
        {
            bool xreplace = true;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                Console.SetWindowSize(35, 19);
                Console.SetBufferSize(35, 19);

                Console.WriteLine($"1. {dictionary[" "].RetLang(staticLanguage)} ");
                Console.WriteLine($"2. {dictionary[" "].RetLang(staticLanguage)} ");
                Console.WriteLine($"3. {dictionary[" "].RetLang(staticLanguage)} ");
                Console.WriteLine($"4. {dictionary[" "].RetLang(staticLanguage)} ");
                Console.WriteLine($"5. {dictionary[" "].RetLang(staticLanguage)} ");
                Console.WriteLine($"6. {dictionary[" "].RetLang(staticLanguage)} ");

            } while (xreplace);
        }
    }
}
