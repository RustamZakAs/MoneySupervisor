using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MoneySupervisor
{
    //[DataContract]
    class MSAccount
    {
        //[DataMember]
        public int          MSAccountId     { get; set; }
        //[DataMember]                      
        public char         MSIO            { get; set; }
        //[DataMember]                      
        public string       MSName          { get; set; }
        //[DataMember]                      
        public ConsoleColor MSColor         { get; set; }
        //[DataMember]                      
        public string       MSImage         { get; set; }
        //[DataMember]                      
        public string       MSСurrencyCode  { get; set; }
        //[DataMember]
        public bool         MSMulticurrency { get; set; }

        public MSAccount()
        {

        }

        public MSAccount(int          msAccountId, 
                         char         msIO, 
                         string       msName, 
                         ConsoleColor msColor, 
                         string       msImage,
                         string       msСurrency,
                         bool         msMulticurrency)
        {
            MSAccountId     = msAccountId;
            MSIO            = msIO;
            MSName          = msName;
            MSColor         = msColor;
            MSImage         = msImage;
            MSСurrencyCode  = msСurrency;
            MSMulticurrency = msMulticurrency;
        }

        public MSAccount(MSAccount account)
        {
            MSAccountId     = account.MSAccountId;
            MSIO            = account.MSIO;
            MSName          = account.MSName;
            MSColor         = account.MSColor;
            MSImage         = account.MSImage;
            MSСurrencyCode  = account.MSСurrencyCode;
            MSMulticurrency = account.MSMulticurrency;
        }

        public void ConsoleAdd(int accountId, char msIO = '=')
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            MSAccountId = accountId;
            if (msIO == '+' || msIO == '-') MSIO = msIO;
            else
            {
                bool xreplace = true;
                do
                {
                    Console.SetCursorPosition(left, top);
                    Console.WriteLine("Введите тип аккаунта (+,-): ");
                    string temp = Console.ReadLine();
                    if (temp.Length > 0)
                    {
                        if (temp[0] == '+' | temp[0] == '-')
                        {
                            MSIO = temp[0];
                            xreplace = false;
                        }
                    }
                } while (xreplace);
            }
            Console.WriteLine("Введите наименование аккаунта: ");
            MSName = Console.ReadLine();
            Console.WriteLine("Выбермте цвет аккаунта: ");
            MSColor = MSIntro.ChooseColor();
            Console.WriteLine("Введите символ аккаунта: ");
            MSImage = Console.ReadLine();
            Console.WriteLine("Введите тип валюты аккаунта: ");
            //MSСurrencyCode = Console.ReadLine();
            MSСurrencyCode = MSСurrency.ChooseСurrency(ref Program.currencies);
            if (MSСurrencyCode == "ALL")
            {
                MSMulticurrency = true;
            }
            else MSMulticurrency = false;
        }

        public static int ChooseAccount(ref List<MSAccount> msAccountList)
        {
            int left = Console.CursorLeft;
            int top  = Console.CursorTop;
            int maxLen = 0;
            if (msAccountList.Count > 0)
                maxLen = msAccountList.Max(s => s.MSName).Length;
            int accountId = 0;
            string xSynbol = "↓ "; // ↓   ↑   ↓↑
            if (msAccountList.Count == 1) xSynbol = "  ";
            Console.WriteLine($"{msAccountList[accountId].MSImage} {msAccountList[accountId].MSName} {xSynbol}");
            do
            {
                //if (Console.KeyAvailable)
                //{
                    Program.cki = Console.ReadKey();
                //}
                switch (Program.cki.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (++accountId >= msAccountList.Count) accountId = msAccountList.Count - 1;
                        for (int i = 0; i < maxLen + 3; i++)
                        {
                            Console.Write(" ");
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (--accountId < 0) accountId = 0;
                        for (int i = 0; i < maxLen + 3; i++)
                        {
                            Console.Write(" ");
                        }
                        break;
                    case ConsoleKey.Enter:
                        Program.cki = default(ConsoleKeyInfo);
                        Console.SetCursorPosition(0, top + 1);
                        return msAccountList[accountId].MSAccountId;
                    default:
                        break;
                }
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"{msAccountList[accountId].MSImage} {msAccountList[accountId].MSName} {xSynbol}");
            } while (true);
        }

        public static string GetName(int MSAccountId)
        {
            for (int i = 0; i < Program.accounts.Count; i++)
            {
                if (Program.accounts[i].MSAccountId == MSAccountId)
                    return Program.accounts[i].MSName;
            }
            return "";
        }

        static public void SQLiteInsertAccountInDatabase(MSAccount a)
        {
            Program.conn.Open();
            int tbool = a.MSMulticurrency == true ? 1 : 0;
            string sql_command = "INSERT INTO MSAccounts (MSAccountId, " +
                                                         "MSIO,     " +
                                                         "MSName, " +
                                                         "MSColor, " +
                                                         "MSImage, " +
                                                         "MSСurrencyCode, " +
                                                         "MSMulticurrency) "
                                              + $"VALUES ({a.MSAccountId}, " +
                                                       $"'{a.MSIO}'," +
                                                       $"'{a.MSName}'," +
                                                       $" {(int)a.MSColor}, " +
                                                       $"'{a.MSImage}'," +
                                                       $"'{a.MSСurrencyCode}'," +
                                                       $" {tbool});";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command, Program.conn);
            command.ExecuteNonQuery();
            Program.conn.Close();
        }
    }
}
