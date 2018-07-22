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
        public static int maxWidth = 40, maxheight = 25;
        public static char transactionSymbol = '+'; //+  -  =
        public static string curFile = @"MSBase.sqlite";
        public static System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection($"Data Source={curFile};Version=3;");

        static void Main(string[] args)
        {
            int maxWidth = 80, maxheight = 25;
            Console.SetWindowSize(maxWidth, maxheight);
            Console.SetBufferSize(maxWidth, maxheight);

            if (MSСurrency.CheckURL(MSСurrency.msСurrencyLink))
                currencies = currency.MSLoadСurrencies();
            else
            {
                if (true) //File exist
                {
                    if (true) //Table exist
                    {
                        if (true) //Value exist
                        {
                            MSSaveLoad.SQLiteLoadCurrenciesFromDatabase();
                        }
                    }
                }
            }
            //if (currencies.Count != 1)
            //{
            //    MSSaveLoad.SQLiteLoadCurrenciesFromDatabase();
            //}
            if (!File.Exists(Program.curFile))
            {
                MSSaveLoad.CreateDatabase();
                MSSaveLoad.InsertStandartValue();
            }
            MSSaveLoad.SQLiteLoadAccountsFromDatabase();
            MSSaveLoad.SQLiteLoadCategoriesFromDatabase();
            MSSaveLoad.SQLiteLoadTransactionsFromDatabase();

            Console.Title = "Money Supervisor - Управление деньгами - Pullara nəzarət";
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            MSLanguage.CreateDictionary(ref dictionary);

            MSIntro.Show();
            MSMainMenyu();
        }

        static void MSMainMenyu()
        {
            Console.Clear();
            maxWidth = 40; maxheight = 25;
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
                //Console.WriteLine(cki.KeyChar);

                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"1. {dictionary["newaccount"].RetLang(staticLanguage)} ");
                Console.SetCursorPosition(0, 2);
                Console.WriteLine($"2. {dictionary["newcategory"].RetLang(staticLanguage)} ");

                switch (cki.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        AddAccount();
                        cki = default(ConsoleKeyInfo);
                        MSMainMenyu();
                        break;
                    case '2':
                        Console.Clear();
                        AddCategory();
                        cki = default(ConsoleKeyInfo);
                        MSMainMenyu();
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
                        Console.Clear();
                        int tTranCount = 0;
                        switch (menyuId)
                        {
                            case 0: //+
                                transactionSymbol = '+';
                                if (accounts.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены счёта.");
                                    Console.WriteLine("Создайте новый счёт.");
                                    Program.AddAccount();
                                }
                                if (categories.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены котегории.");
                                    Console.WriteLine("Создайте новую котегорию.");
                                    Program.AddCategory();
                                }
                                tTranCount = transactions.Count;
                                bool xReplaceTrCount = true;
                                do
                                {
                                    tTranCount++;
                                    if (!transactions.Exists(x => x.MSTransactionId == tTranCount))
                                    {
                                        xReplaceTrCount = false;
                                        transaction.ConsoleAdd(tTranCount, transactionSymbol);
                                    }
                                } while (xReplaceTrCount);
                                transactions.Add(new MSTransaction(transaction));
                                MSTransaction.SQLiteInsertTransactionInDatabase(transaction);
                                goto case 99;
                                //break;
                            case 1: //-
                                transactionSymbol = '-';
                                if (accounts.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены счёта.");
                                    Console.WriteLine("Создайте новый счёт.");
                                    Program.AddAccount();
                                }
                                if (categories.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены котегории.");
                                    Console.WriteLine("Создайте новую котегорию.");
                                    Program.AddCategory();
                                }
                                tTranCount = transactions.Count;
                                do
                                {
                                    tTranCount++;
                                    transaction.ConsoleAdd(tTranCount, transactionSymbol);
                                } while (transactions.Exists(x => x.MSTransactionId == tTranCount));
                                transactions.Add(new MSTransaction(transaction));
                                MSTransaction.SQLiteInsertTransactionInDatabase(transaction);
                                goto case 99;
                                //break;
                            case 2: //=
                                transactionSymbol = '=';
                                if (accounts.Count < 2)
                                {
                                    Console.WriteLine("В базе не достаточно счётов.");
                                    Console.WriteLine("Создайте новый счёт.");
                                    Program.AddAccount();
                                }
                                if (categories.Count == 0)
                                {
                                    Console.WriteLine("В базе не найдены котегории.");
                                    Console.WriteLine("Создайте новую котегорию.");
                                    Program.AddCategory();
                                }
                                MSTransaction.ConsoleTransfer(ref transactions, categories.Count + 1);
                                goto case 99;
                                //break;
                            case 3: //param
                                transactionSymbol = '=';
                                bool xReplace = true;
                                do
                                {
                                    Console.SetCursorPosition(0, 1);
                                    Console.WriteLine($"1. {dictionary["newaccount"].RetLang(staticLanguage)} ");
                                    Console.SetCursorPosition(0, 2);
                                    Console.WriteLine($"2. {dictionary["newcategory"].RetLang(staticLanguage)} ");
                                    Console.SetCursorPosition(0, 3);
                                    Console.Write($"3. Выберите язык программы: ");
                                    Console.SetCursorPosition(0, 4);
                                    Console.Write($"4. Сброс базы данных: ");
                                    Console.SetCursorPosition(0, 5);
                                    Console.Write($"5. Выгрузка в CSV файл: ");
                                    Console.SetCursorPosition(0, 6);
                                    Console.Write($"6. Назад: ");

                                    cki = Console.ReadKey();
                                    //Console.WriteLine($"2. {dictionary["newcategory"].RetLang(staticLanguage)} ");
                                    switch (cki.KeyChar)
                                    {
                                        case '1':
                                            Console.Clear();
                                            Console.WriteLine($"1. {dictionary["newaccount"].RetLang(staticLanguage)} ");
                                            Program.AddAccount();
                                            break;
                                        case '2':
                                            Console.Clear();
                                            Console.WriteLine($"2. {dictionary["newcategory"].RetLang(staticLanguage)} ");
                                            Program.AddCategory();
                                            break;
                                        case '3':
                                            Console.Write($"3. Выберите язык программы: ");
                                            staticLanguage = MSLanguage.ChooseLanguage();
                                            Console.WriteLine("Язык изменён.");
                                            Program.cki = Console.ReadKey();
                                            break;
                                        case '4':
                                            MSSaveLoad.CreateDatabase();
                                            MSSaveLoad.InsertStandartValue();
                                            currency = new MSСurrency();
                                            currencies = new List<MSСurrency>();
                                            account = new MSAccount();
                                            accounts = new List<MSAccount>();
                                            category = new MSCategory();
                                            categories = new List<MSCategory>();
                                            transaction = new MSTransaction();
                                            transactions = new List<MSTransaction>();
                                            break;
                                        case '5':
                                            MSTransaction.SQLiteOutputTransactionsInCSV(transactions);
                                            break;
                                        case '6':
                                            xReplace = false;
                                            break;
                                        default:
                                            break;
                                    }
                                    cki = default(ConsoleKeyInfo);
                                } while (xReplace);
                                Console.Clear();
                                break;
                            case 4: //help
                                maxWidth = 80; maxheight = 25;
                                Console.SetWindowSize(maxWidth, maxheight);
                                Console.SetBufferSize(maxWidth, maxheight);
                                Console.WriteLine(">> Money Supervisor << предназначена для ведения записей о Ваших доходах и расходах");
                                MSIntro.Show();
                                Console.Clear();
                                maxWidth = 40; maxheight = 25;
                                Console.SetWindowSize(maxWidth, maxheight);
                                Console.SetBufferSize(maxWidth, maxheight);
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
                Console.SetCursorPosition(0,11);
                MSStatistics.Show(ref accounts, ref categories, ref transactions);

                //Console.WriteLine($"3. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"4. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"5. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.WriteLine($"6. {dictionary[" "].RetLang(staticLanguage)} ");
                //Console.ReadKey();
            } while (xreplace);
        }

        private static void AddAccount()
        {
            int tAccId = accounts.Count;
            do
            {
                tAccId++;
                if (!(accounts.Exists(x => x.MSAccountId == tAccId)))
                {
                    account.ConsoleAdd(tAccId, transactionSymbol);
                    break;
                }
            } while (true);
            accounts.Add(new MSAccount(account));
            MSAccount.SQLiteInsertAccountInDatabase(account);
            Console.WriteLine("Аккаунт добавлен.");
            Program.cki = Console.ReadKey();
        }

        private static void AddCategory()
        {
            int tCatId = categories.Count;
            do
            {
                tCatId++;
                if (!(categories.Exists(x => x.MSAccountId == tCatId)))
                {
                    category.ConsoleAdd(tCatId, transactionSymbol);
                    break;
                }
            } while (true);
            categories.Add(new MSCategory(category));
            MSCategory.SQLiteInsertCategoryInDatabase(category);
            Console.WriteLine("Категория добавлена.");
            Program.cki = Console.ReadKey();
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

        public static float MSReadDouble()
        {
            StringBuilder tStr = new StringBuilder(10);
            int DelimentrCount = 0;
            do
            {
                cki = Console.ReadKey();
                DelimentrCount = 0;
                switch (cki.KeyChar)
                {
                    case '0':
                        tStr.Append("0");
                        break;
                    case '1':
                        tStr.Append("1");
                        break;
                    case '2':
                        tStr.Append("2");
                        break;
                    case '3':
                        tStr.Append("3");
                        break;
                    case '4':
                        tStr.Append("4");
                        break;
                    case '5':
                        tStr.Append("5");
                        break;
                    case '6':
                        tStr.Append("6");
                        break;
                    case '7':
                        tStr.Append("7");
                        break;
                    case '8':
                        tStr.Append("8");
                        break;
                    case '9':
                        tStr.Append("9");
                        break;
                    case '.':
                        for (int i = 0; i < tStr.Length; i++)
                        {
                            if (tStr[i] == ',') DelimentrCount++;
                        }
                        if (DelimentrCount == 0)
                        {
                            tStr.Append(",");
                        } else Console.Write("\b \b");
                        break;
                    case ',':
                        for (int i = 0; i < tStr.Length; i++)
                        {
                            if (tStr[i] == ',') DelimentrCount++;
                        }
                        if (DelimentrCount == 0)
                        {
                            tStr.Append(",");
                        }
                        else Console.Write("\b \b");
                        break;
                    default:
                        //Console.Write("\b \b");
                        break;
                }
                switch (cki.Key)
                {
                    case ConsoleKey.Enter:
                        cki = default(ConsoleKeyInfo);
                        if (tStr.Length > 0)
                            return (float)Convert.ToDouble(tStr.ToString());
                        else return 0;
                    case ConsoleKey.Escape:
                        cki = default(ConsoleKeyInfo);
                        return 0;
                    case ConsoleKey.Backspace:
                        cki = default(ConsoleKeyInfo);
                        if (tStr.Length > 0)
                        {
                            tStr.Remove(tStr.Length - 1, 1);
                            Console.Write(" ");
                            Console.Write("\b");
                        }
                        break;
                    default:
                        break;
                }
            } while (true);
        }
    }
}
