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

        public static MSСurrency currency = new MSСurrency();
        public static List<MSСurrency> currencies = new List<MSСurrency>();
        public static MSAccount account                = new MSAccount();
        public static List<MSAccount> accounts         = new List<MSAccount>();
        public static MSCategory category              = new MSCategory();
        public static List<MSCategory> categories      = new List<MSCategory>();
        public static MSTransaction transaction        = new MSTransaction();
        public static List<MSTransaction> transactions = new List<MSTransaction>();

        public static ConsoleKeyInfo cki;
        public static Random random = new Random();

        public static int menyuId = 0, maxMenyuId = 5;
        public static char transactionSymbol = '+'; //+  -  =

        static void Main(string[] args)
        {
            currencies = currency.MSLoadСurrencies();

            MSSaveLoad.CreateDatabase();
            MSSaveLoad.InsertStandartValue();
            if (currencies.Count != 1) 
            MSSaveLoad.SQLiteLoadCurrenciesFromDatabase();
            MSSaveLoad.SQLiteLoadAccountsFromDatabase();
            MSSaveLoad.SQLiteLoadCategoriesFromDatabase();


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
                Console.WriteLine($"1. {dictionary["newaccount"].RetLang(staticLanguage)} ");
                Console.SetCursorPosition(0, 2);
                Console.WriteLine($"2. {dictionary["newcategory"].RetLang(staticLanguage)} ");

                switch (cki.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        account.ConsoleAdd(accounts.Count + 1, ' ');
                        accounts.Add(account);
                        Console.WriteLine("Аккаунт добавлен.");
                        Program.cki = Console.ReadKey();
                        Program.cki = default(ConsoleKeyInfo);
                        Console.Clear();
                        break;
                    case '2':
                        Console.Clear();
                        category.ConsoleAdd(categories.Count + 1, ' ');
                        categories.Add(category);
                        Console.WriteLine("Категория добавлена.");
                        Program.cki = Console.ReadKey();
                        Program.cki = default(ConsoleKeyInfo);
                        Console.Clear();
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
                        Program.cki = default(ConsoleKeyInfo);
                        switch (menyuId)
                        {
                            case 0: //+
                                transactionSymbol = '+';
                                if (accounts.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены счёта.");
                                    Console.WriteLine("Создате новый счёт.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    account.ConsoleAdd(accounts.Count+1, transactionSymbol);
                                    accounts.Add(account);
                                    Console.WriteLine("Аккаунт добавлен.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    Console.Clear();
                                }
                                if (categories.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены котегории.");
                                    Console.WriteLine("Создате новую котегорию.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    category.ConsoleAdd(categories.Count + 1, transactionSymbol);
                                    categories.Add(category);
                                    Console.WriteLine("Категория добавлена.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    Console.Clear();
                                }
                                transaction.ConsoleAdd(transactions.Count + 1, transactionSymbol);
                                transactions.Add(transaction);
                                goto case 99;
                                //break;
                            case 1: //-
                                transactionSymbol = '-';
                                if (accounts.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены счёта.");
                                    Console.WriteLine("Создате новый счёт.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    account.ConsoleAdd(accounts.Count + 1, transactionSymbol);
                                    accounts.Add(account);
                                    Console.WriteLine("Аккаунт добавлен.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    Console.Clear();
                                }
                                if (categories.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены котегории.");
                                    Console.WriteLine("Создате новую котегорию.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    category.ConsoleAdd(categories.Count + 1, transactionSymbol);
                                    categories.Add(category);
                                    Console.WriteLine("Категория добавлена.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    Console.Clear();
                                }
                                transaction.ConsoleAdd(transactions.Count + 1, transactionSymbol);
                                transactions.Add(transaction);
                                goto case 99;
                                //break;
                            case 2: //=
                                transactionSymbol = '=';
                                if (accounts.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены счёта.");
                                    Console.WriteLine("Создате новый счёт.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    account.ConsoleAdd(accounts.Count + 1, transactionSymbol);
                                    accounts.Add(account);
                                    Console.WriteLine("Аккаунт добавлен.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    Console.Clear();
                                }
                                if (categories.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены котегории.");
                                    Console.WriteLine("Создате новую котегорию.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    category.ConsoleAdd(categories.Count + 1, transactionSymbol);
                                    categories.Add(category);
                                    Console.WriteLine("Категория добавлена.");
                                    Program.cki = Console.ReadKey();
                                    Program.cki = default(ConsoleKeyInfo);
                                    Console.Clear();
                                }
                                MSTransaction.ConsoleTransfer(ref transactions, categories.Count + 1);
                                goto case 99;
                                //break;
                            case 3: //param
                                Console.Write("Выберите язык программы: ");
                                staticLanguage = MSLanguage.ChooseLanguage();
                                Console.WriteLine("Язык изменён.");
                                Program.cki = Console.ReadKey();
                                Program.cki = default(ConsoleKeyInfo);
                                Console.Clear();
                                break;
                            case 4: //help
                                Console.Clear();
                                Console.WriteLine(">> Money Supervisor << предназначена для ведения записей о Ваших доходах и расходах");
                                MSIntro.Show();
                                break;
                            case 99:
                                Console.WriteLine("Информация добавлена.");
                                Program.cki = Console.ReadKey();
                                Program.cki = default(ConsoleKeyInfo);
                                Console.Clear();
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

        public static DateTime msCompDateTime()
        {
            return new DateTime(int.Parse(DateTime.Now.ToString("yyyy")),
            int.Parse(DateTime.Now.ToString("MM")),
            int.Parse(DateTime.Now.ToString("dd")),
            int.Parse(DateTime.Now.ToString("HH")),
            int.Parse(DateTime.Now.ToString("mm")),
            int.Parse(DateTime.Now.ToString("ss")));
        }
    }
}
