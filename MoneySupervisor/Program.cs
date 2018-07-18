using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data.SQLite;
//using SQLite;

namespace MoneySupervisor
{
    class Program
    {
        public static int staticLanguage = (int)MSLang.RU;

        public static Dictionary<string, MSLanguage> dictionary = new Dictionary<string, MSLanguage>();

        public static MSCategory category = new MSCategory();
        public static List<MSCategory> categories = new List<MSCategory>();
        public static MSAccount account = new MSAccount();
        public static List<MSAccount> accounts = new List<MSAccount>();

        public static ConsoleKeyInfo cki;
        public static Random random = new Random();

        public static int menyuId = 0, maxMenyuId = 5;
        public static char transactionSymbol = '+'; //+  -  =

        static void Main(string[] args)
        {
            MSSaveLoad.CreateDatabase();
            MSSaveLoad.InsertStandartValue();

            Console.Title = "Money Supervisor - Управление деньгами - Pullara nəzarət";
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            MSLanguage.CreateDictionary(ref dictionary);

            MSIntro.Show();
            Console.Clear();
            MSMainMenyu();
        }

        static void MSMainMenyu()
        {
            int maxWidth = 40, maxheight = 25;
            Console.SetWindowSize(maxWidth, maxheight);
            Console.SetBufferSize(maxWidth, maxheight);
            
            bool xreplace = true;
            int i = 0;
            do
            {
                string str = "Добро пожаловать!";
                if (i >= maxWidth - str.Length)
                {
                    Console.SetCursorPosition(maxWidth - str.Length,0);
                    for (int j = 0; j < str.Length; j++)
                    {
                        Console.Write(" ");
                    }
                    i = 0;
                } 
                
                Console.SetCursorPosition(i - 1 < 0 ? i >= maxWidth-str.Length ? 0 : 0 : i - 1, 0);
                Console.Write("                  ");
                Console.SetCursorPosition(i++ < maxWidth - str.Length ? i : 0, 0);
                Console.Write(str);
                Thread.Sleep(300);
                
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey();
                }
                Console.SetCursorPosition(0, 3);
                Console.WriteLine(cki.KeyChar);

                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"1. {dictionary["newcategory"].RetLang(staticLanguage)} ");
                Console.SetCursorPosition(0, 2);
                Console.WriteLine($"2. {dictionary["newaccount"].RetLang(staticLanguage)} ");

                switch (cki.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        category.ConsoleAdd(categories.Count + 1);
                        categories.Add(category);
                        break;
                    case '2':
                        Console.Clear();
                        account.ConsoleAdd(accounts.Count + 1);
                        accounts.Add(account);
                        break;
                    default:
                        break;
                }

                switch (cki.Key)
                {
                    case ConsoleKey.Tab:
                        if (++menyuId > maxMenyuId-1) menyuId = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        if (++menyuId > maxMenyuId-1) menyuId = maxMenyuId-1;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (--menyuId < 0) menyuId = 0;
                        break;
                    case ConsoleKey.DownArrow:
                        if (++menyuId > maxMenyuId-1) menyuId = maxMenyuId-1;
                        break;
                    case ConsoleKey.UpArrow:
                        if (--menyuId < 0) menyuId = 0;
                        break;
                    case ConsoleKey.Enter:
                        switch (menyuId)
                        {
                            case 0: //+
                                if (categories.Count == 0)
                                {
                                    Console.Clear();
                                    category.ConsoleAdd(categories.Count+1);
                                    categories.Add(category);
                                }
                                break;
                            case 1: //-
                                break;
                            case 2: //=
                                break;
                            case 3: //param
                                break;
                            case 4: //help
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                cki = default(ConsoleKeyInfo);

                MSIntro.Plus(menyuId, 5, 5, ConsoleColor.Green, ConsoleColor.Black);
                MSIntro.Minus(menyuId, 15, 5, ConsoleColor.Red, ConsoleColor.Black);
                MSIntro.Transfer(menyuId, 25, 5, ConsoleColor.Blue, ConsoleColor.Black);
                MSIntro.NotFound(menyuId, maxWidth - 14, maxheight - 5, ConsoleColor.Magenta, ConsoleColor.Black);
                MSIntro.Param(menyuId, 1, maxheight - 5, ConsoleColor.Yellow, ConsoleColor.Black);
                
                //Console.WriteLine($"3. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"4. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"5. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"6. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.ReadKey();
            } while (xreplace);
        }
    }
}
