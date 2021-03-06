﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneySupervisor
{
    //[DataContract]
    class MSTransaction
    {
        //[DataMember]
        public int      MSTransactionId { get; set; }
        //[DataMember]
        public char     MSIO            { get; set; }
        //[DataMember]
        public float    MSValue         { get; set; }
        //[DataMember]                      
        public string   MSCurrencyCode { get; set; }
        //[DataMember]
        public int      MSAccountId     { get; set; }
        //[DataMember]
        public int      MSCategoryId    { get; set; }
        //[DataMember]
        public string   MSNote          { get; set; }
        //[DataMember]
        public DateTime MSDateTime      { get; set; }
        //[DataMember]
        public bool     MSMulticurrency { get; set; }

        public int MyProperty { get; set; }

        public MSTransaction()
        {
        }

        public MSTransaction(MSTransaction msTransaction)
        {
            MSTransactionId = msTransaction.MSTransactionId;
            MSIO            = msTransaction.MSIO;
            MSValue         = msTransaction.MSValue;
            MSCurrencyCode  = msTransaction.MSCurrencyCode;
            MSAccountId     = msTransaction.MSAccountId;
            MSCategoryId    = msTransaction.MSCategoryId;
            MSNote          = msTransaction.MSNote;
            MSDateTime      = msTransaction.MSDateTime;
            MSMulticurrency = msTransaction.MSMulticurrency;
        }

        public void ConsoleAdd(int msTransactionId, char msIO)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            MSTransactionId = msTransactionId;
            //bool xreplace = true;
            //do
            //{
            //    Console.SetCursorPosition(left, top);
            //    Console.WriteLine("Введите тип транзакции (+,-): ");
            //    string temp = Console.ReadLine();
            //    if (temp[0] == '+' | temp[0] == '-')
            //    {
            //        MSIO = temp[0];
                    MSIO = msIO;
            //        xreplace = false;
            //    }
            //} while (xreplace);

            string insertSumm = "Введите значение (сумма): ";
            Console.WriteLine(insertSumm);
            left = Console.CursorLeft;
            top = Console.CursorTop;
            //string tReadLine = "";
            do
            {
                Console.SetCursorPosition(left, top);
                if (MSValue > 0)
                {
                    string str = Convert.ToString(MSValue);
                    for (int i = 0; i < str.Length; i++)
                    {
                        Console.WriteLine(" ");
                    }
                }
                Console.SetCursorPosition(left, top);
                //    tReadLine = Console.ReadLine();
                //    float.TryParse(tReadLine, out float tMSValue);
                //    MSValue = tMSValue;
                MSValue = Program.MSReadDouble();
            } while (!(MSValue > 0));
            if (MSIO == '+') 
            {
                if (MSValue < 0) { MSValue *= -1; }
            }
            else if (MSIO == '-')
            {
                if (MSValue > 0) { MSValue *= -1; }
            }
            Console.SetCursorPosition(insertSumm.Length + 1, top - 1);
            Console.WriteLine($"{MSValue:f2}");
            Program.cki = default(ConsoleKeyInfo);

            Console.WriteLine("Выберите тип валюты: ");
            MSCurrencyCode = MSCurrency.ChooseCurrency(ref Program.currencies);

            Console.WriteLine("Выберите аккаунт: ");
            if (Program.accounts.Count > 0)
                MSAccountId = MSAccount.ChooseAccount(ref Program.accounts);

            Console.WriteLine("Выберите категорию: ");
            if (Program.categories.Count > 0)
                MSCategoryId = MSCategory.ChooseCategory(ref Program.categories, MSAccountId);

            Console.WriteLine("Введите заметку: ");
            MSNote = Console.ReadLine();
            if (MSNote.Length == 0) MSNote = " ";

            Console.WriteLine("Введите дату и время\n(DD.MM.YYYY hh.mm.ss): "); Console.Write($" {Program.msCompDateTime()}");
            int xtop = Console.CursorTop;
            Console.SetCursorPosition(0, xtop + 1);
            string tDateTime = Console.ReadLine();
            if (tDateTime.Length == 0 | tDateTime == " ") MSDateTime = Program.msCompDateTime();
            else MSDateTime = DateTime.Parse(tDateTime);
            if (MSCurrencyCode == "ALL")
            {
                MSMulticurrency = true;
            }
            else MSMulticurrency = false;
            Program.cki = default(ConsoleKeyInfo);
            Console.Clear();
        }

        public static void ConsoleTransfer(ref List<MSTransaction> transactions, int msTransactionId)
        {
            int tAccount2 = 0;
            MSTransaction tMSTransaction = new MSTransaction();
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            tMSTransaction.MSTransactionId = msTransactionId;

            tMSTransaction.MSIO = '-';
            
            string insertSumm = "Введите значение (сумма): ";
            Console.WriteLine(insertSumm);
            left = Console.CursorLeft;
            top = Console.CursorTop;
            do
            {
                Console.SetCursorPosition(left, top);
                if (Convert.ToString(tMSTransaction.MSValue).Length > 0)
                {
                    for (int i = 0; i < Convert.ToString(tMSTransaction.MSValue).Length; i++)
                    {
                        Console.WriteLine(" ");
                    }
                }
                Console.SetCursorPosition(left, top);
                tMSTransaction.MSValue = Program.MSReadDouble();
            } while (!(tMSTransaction.MSValue > 0));
            if (tMSTransaction.MSIO == '+')
            {
                if (tMSTransaction.MSValue < 0) tMSTransaction.MSValue *= -1;
            }
            else if (tMSTransaction.MSIO == '-')
            {
                if (tMSTransaction.MSValue > 0) tMSTransaction.MSValue *= -1;
            }
            Console.SetCursorPosition(insertSumm.Length + 1, top - 1);
            Console.WriteLine($"{tMSTransaction.MSValue:f2}");
            Program.cki = default(ConsoleKeyInfo);

            Console.WriteLine("Выберите аккаунт вывода суммы: "); //1
            if (Program.accounts.Count > 0)
                tMSTransaction.MSAccountId = MSAccount.ChooseAccount(ref Program.accounts);

            left = Console.CursorLeft;
            top = Console.CursorTop;
            bool xreplace = true;
            do
            {
                Console.SetCursorPosition(left, top);
                Console.WriteLine("Выберите тип валюты: ");
                string tMSCurrencyCode = MSCurrency.ChooseCurrency(ref Program.currencies);

                if (tMSCurrencyCode != "ALL")
                {
                    tMSTransaction.MSCurrencyCode = tMSCurrencyCode;
                    xreplace = false;
                }
            } while (xreplace);

            Console.WriteLine("Выберите аккаунт ввода суммы: "); //2
            if (Program.accounts.Count > 0)
                tAccount2 = MSAccount.ChooseAccount(ref Program.accounts);

            Console.WriteLine("Введите заметку: ");
            tMSTransaction.MSNote = Console.ReadLine();
            if (tMSTransaction.MSNote.Length == 0) tMSTransaction.MSNote = " ";

            Console.WriteLine("Введите дату и время\n(DD.MM.YYYY hh.mm.ss): ");
            Console.Write($" {Program.msCompDateTime()}");
            int xtop = Console.CursorTop;
            Console.SetCursorPosition(0, xtop + 1);
            string tDateTime = Console.ReadLine();
            if (tDateTime.Length == 0 | tDateTime == " ") tMSTransaction.MSDateTime = Program.msCompDateTime();
            else tMSTransaction.MSDateTime = DateTime.Parse(tDateTime);

            transactions.Add(new MSTransaction(tMSTransaction));
            MSTransaction.SQLiteInsertTransactionInDatabase(tMSTransaction);

            tMSTransaction.MSIO = '+';

            tMSTransaction.MSAccountId = tAccount2;

            if (tMSTransaction.MSIO == '+')
            {
                if (tMSTransaction.MSValue < 0) tMSTransaction.MSValue *= -1;
            }
            else if (tMSTransaction.MSIO == '-')
            {
                if (tMSTransaction.MSValue > 0) tMSTransaction.MSValue *= -1;
            }

            transactions.Add(new MSTransaction(tMSTransaction));
            MSTransaction.SQLiteInsertTransactionInDatabase(tMSTransaction);

            Program.cki = default(ConsoleKeyInfo);
            Console.Clear();
        }

        static public void SQLiteInsertTransactionInDatabase(MSTransaction tr)
        {
            using (System.Data.SQLite.SQLiteConnection conn = 
                new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;"))
            {
                conn.Open();
                int tbool = tr.MSMulticurrency == true ? 1 : 0;
                string tstr = (tr.MSValue.ToString()).Replace(',', '.');
                string sql_command = "INSERT INTO MSTransactions (MSTransactionId, " +
                                                                 "MSIO, " +
                                                                 "MSValue, " +
                                                                 "MSCurrencyCode, " +
                                                                 "MSAccountId, " +
                                                                 "MSCategoryId, " +
                                                                 "MSNote,            " +
                                                                 "MSDateTime, " +
                                                                 "MSMulticurrency) "
                    + $"VALUES ({tr.MSTransactionId}," +
                              $"'{tr.MSIO}'," +
                              $" {tstr}," +
                              $"'{tr.MSCurrencyCode}'," +
                              $" {tr.MSAccountId}," +
                              $" {tr.MSCategoryId}," +
                              $"'{tr.MSNote}'," +
                              $"'{(tr.MSDateTime).ToString()}'," +
                              $" {tbool});";
                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        static public void SQLiteOutputTransactionsInCSV(List<MSTransaction> trList)
        {
            //MSTransaction to CSV file
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("MSTransactionId, MSIO, MSValue, MSCurrencyCode, MSAccountId, MSCategoryId, MSNote, MSDateTime, MSMulticurrency");
            foreach (var tr in trList)
            {
                sb.AppendLine($"{tr.MSTransactionId}," +
                    $"{tr.MSIO}," +
                    $"{tr.MSValue}," +
                    $"{tr.MSCurrencyCode}," +
                    $"{MSAccount.GetName(tr.MSAccountId)}," +
                    $"{MSCategory.GetName(tr.MSCategoryId)}," +
                    $"{tr.MSNote}," +
                    $"{tr.MSDateTime}," +
                    $"{tr.MSMulticurrency}");
            }

            //Console.WriteLine(sb.ToString());

            System.IO.File.WriteAllText(
                System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Transaction.csv"),
                sb.ToString());
        }

        static public void ShowAllTransaction(List<MSTransaction> trList)
        {
            int maxWidth = 80;
            Console.SetWindowSize(maxWidth, 25);
            foreach (var tr in trList)
            {
                Console.WriteLine($"{tr.MSTransactionId}," +
                    $"{tr.MSIO}," +
                    $"{tr.MSValue}," +
                    $"{tr.MSCurrencyCode}," +
                    $"{MSAccount.GetName(tr.MSAccountId)}," +
                    $"{MSCategory.GetName(tr.MSCategoryId)}," +
                    $"{tr.MSNote}," +
                    $"{tr.MSDateTime}," +
                    $"{tr.MSMulticurrency}\n");
            }
        }
    }
}
