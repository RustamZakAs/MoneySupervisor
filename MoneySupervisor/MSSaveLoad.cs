using System;
//using SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MoneySupervisor
{
    public class MSSaveLoad
    {
        //SQLite min date 1970-01-01 00:00:00 UTC
        // strftime('%s', value)        write
        // datetime(value, 'unixepoch') read
        public static void CreateDatabase()
        {
            string curFile = @"MSBase.sqlite";
            Console.WriteLine("Работа с файлом бызы данных.");
            Console.WriteLine(File.Exists(curFile) ? "Файл существует." : "Файла не существует.");
            try
            {
                Console.WriteLine("Попытка удаления файла.");
                if (File.Exists(curFile))
                    File.Delete(curFile);
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("Файл используется другим приложением!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка с файлом.");
                Console.WriteLine(e);
            }
            if (!File.Exists(curFile)) Console.WriteLine("Файл удалён.");
            else Console.WriteLine("Файл не удалён.");

            try
            {
                Console.WriteLine("Попытка создания файла.");
                if (!File.Exists(curFile))
                {
                    Console.WriteLine("Создание файла с базой данных.");
                    System.Data.SQLite.SQLiteConnection.CreateFile("MSBase.sqlite");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка с файлом.");
                Console.WriteLine(e);
            }
            if (File.Exists(curFile)) Console.WriteLine("Файл создан.");
            else Console.WriteLine("Файл не создан.");

            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn.Open();

            string sql_command = "DROP TABLE IF EXISTS MSAccounts;"
                               + "CREATE TABLE IF NOT EXISTS MSAccounts ( "
                               + "MSAccountId	  INTEGER NOT NULL CHECK (MSAccountId >= 0)                         , "
                               + "MSIO	          TEXT    NOT NULL CHECK (MSIO='+' OR MSIO='-')                     , " //+ -
                               + "MSName	      TEXT    NOT NULL                                                  , " //Name
                               + "MSColor	      INTEGER NOT NULL CHECK (MSColor >= 0)                             , " //Color int
                               + "MSImage	      TEXT    NOT NULL                                                  , " //Symbols
                               + "MSСurrencyCode  TEXT    NOT NULL CHECK (LENGTH(MSСurrencyCode) = 3)                     , " //AZN RUB USD CNY 
                               + "MSMulticurrency INTEGER NOT NULL CHECK (MSMulticurrency = 0 OR MSMulticurrency = 1) "
                               + "); ";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
            command.ExecuteNonQuery();

            sql_command = "DROP TABLE IF EXISTS MSCategories;"
                         + "CREATE TABLE IF NOT EXISTS MSCategories ( "
                         + "MSCategoryId INTEGER NOT NULL CHECK (MSCategoryId >= 0)   , "
                         + "MSIO         TEXT    NOT NULL CHECK (MSIO='+' OR MSIO='-'), " //+ -
                         + "MSName       TEXT    NOT NULL                             , " //Name
                         + "MSAccountId  INTEGER NOT NULL CHECK (MSAccountId >= 0)    , " //id
                         + "MSColor      INTEGER NOT NULL CHECK (MSColor >= 0)        , " //Color int
                         + "MSImage      TEXT    NOT NULL                               " //Symbols
                         + "); ";
            command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
            command.ExecuteNonQuery();

            sql_command = "DROP TABLE IF EXISTS MSTransactions;"
                        + "CREATE TABLE IF NOT EXISTS MSTransactions ( "
                        + "MSTransactionId INTEGER NOT NULL CHECK (MSTransactionId >= 0)                     , "
                        + "MSIO	           TEXT    NOT NULL CHECK (MSIO='+' OR MSIO='-')                     , "
                        + "MSValue	       NUMERIC NOT NULL CHECK (MSValue >= 0.000001)                      , "
                        + "MSСurrencyCode  TEXT    NOT NULL CHECK (LENGTH(MSСurrencyCode) = 3)               , "
                        + "MSAccountId	   INTEGER NOT NULL CHECK (MSAccountId >= 0)                         , "
                        + "MSCategoryId	   INTEGER NOT NULL CHECK (MSCategoryId >= 0)                        , "
                        + "MSNote	       TEXT                                                              , "
                        + "MSDateTime	   TEXT    NOT NULL CHECK (MSDateTime >= 0)                          , "
                        + "MSMulticurrency INTEGER NOT NULL CHECK (MSMulticurrency = 0 OR MSMulticurrency = 1) "
                        + "); ";
            command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
            command.ExecuteNonQuery();

            sql_command = "DROP TABLE IF EXISTS MSСurrencies;"
                        + "CREATE TABLE IF NOT EXISTS MSСurrencies ( "
                        + "MSСurrencyId      INTEGER NOT NULL CHECK (MSСurrencyId >= 0)             , "
                        + "MSСurrencyDate    TEXT NOT NULL CHECK (MSСurrencyDate >= 0)              , "
                        + "MSСurrencyType    TEXT                                                   , "
                        + "MSСurrencyCode    TEXT    NOT NULL CHECK (LENGTH(MSСurrencyCode) = 3)    , "
                        + "MSСurrencyNominal NUMERIC NOT NULL CHECK (MSСurrencyNominal >= 0.000001) , "
                        + "MSСurrencyName	 TEXT                                                   , "
                        + "MSСurrencyValue	 NUMERIC NOT NULL CHECK (MSСurrencyValue >= 0)            "
                        + "); ";
            command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
            command.ExecuteNonQuery();

            conn.Close();
            //using (SQLiteConnection conn2 = new SQLiteConnection("Data Source=MSBase.db; Version=3;"))
            //{
            //    SQLiteCommand cmd = conn2.CreateCommand();
            //    string sql_command2 = "DROP TABLE IF EXISTS MSCategory;"
            //      + "CREATE TABLE MSCategory("
            //      + "MSCategoryId INTEGER PRIMARY KEY AUTOINCREMENT, "
            //      + "MSIO         TEXT, "
            //      + "MSName       TEXT, "
            //      + "MSAccountId  INTEGER, "
            //      + "MSColor      INTEGER,"
            //      + "MSImage      TEXT);";
            //    cmd.CommandText = sql_command2;
            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (SQLiteException ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
        }

        public static void InsertStandartValue()
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn.Open();

            string sql_command = "INSERT INTO MSAccounts (MSAccountId, MSIO,     MSName, MSColor, MSImage, MSСurrencyCode, MSMulticurrency) "
                                               + "VALUES (          0,  '+', 'Наличные',       1,    ';)',          'AZN',               0);";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
            command.ExecuteNonQuery();

            sql_command = "INSERT INTO MSCategories (MSCategoryId, MSIO,     MSName, MSAccountId, MSColor, MSImage) "
                                          + "VALUES (           0,  '+', 'Зарплата',           0,       1,    ';)');";
            command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
            command.ExecuteNonQuery();

            sql_command = "INSERT INTO MSTransactions (MSTransactionId, MSIO, MSValue, MSСurrencyCode, MSAccountId, MSCategoryId, MSNote,            MSDateTime, MSMulticurrency) "
                                            + "VALUES (              0,  '+',    0.01,          'AZN',           0,            0, 'Test', '20.07.2018 00:40:00',               0);";
            command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
            command.ExecuteNonQuery();

            sql_command = "INSERT INTO MSСurrencies (MSСurrencyId,        MSСurrencyDate, MSСurrencyType, MSСurrencyCode, MSСurrencyNominal, MSСurrencyName, MSСurrencyValue) "
                                          + "VALUES (           0, '20.07.2018 00:40:00',             '',          'AZN',                 1,             '',               1);";
            command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
            command.ExecuteNonQuery();

            conn.Close();
            //using (SQLiteConnection conn = new SQLiteConnection("Data Source=MSBase.db; Version=3;"))
            //{
            //    SQLiteCommand cmd = conn.CreateCommand();
            //    string sql_command = "INSERT INTO MSCategory(MSIO,     MSName, MSAccountId, MSColor, MSImage) "
            //                                       + "VALUES ('+', 'Зарплата',            1,      1,    ';)');";
            //    cmd.CommandText = sql_command;
            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //    }
            //    catch (SQLiteException ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
        }

        //public static List<object> Load ()
        //{


        //return ;
        //}

        static public void SQLiteSaveAccountsList()
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn.Open();

            for (int i = 0; i < Program.accounts.Count; i++)
            {
                int MSMulticurrency = Program.accounts[0].MSMulticurrency == true ? 1 : 0;
                string sql_command = $"INSERT INTO MSAccounts (MSAccountId, MSIO,     MSName, MSColor, MSImage, MSСurrencyCode, MSMulticurrency) "
                                                    + $"VALUES (" +
                                                      $"  {Program.accounts[0].MSAccountId},  " +
                                                      $" '{Program.accounts[0].MSIO}'   ," +
                                                      $" '{Program.accounts[0].MSName}' ," +
                                                      $"  {Program.accounts[0].MSColor} ," +
                                                      $"  {Program.accounts[0].MSImage} ," +
                                                      $" '{Program.accounts[0].MSСurrencyCode}'," +
                                                      $"  {MSMulticurrency});";
                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command, conn);
                command.ExecuteNonQuery();
            }
            conn.Close();
        }

        private int DateTimeToSecondInt(DateTime dateTime)
        {
            System.TimeSpan ts = dateTime.Subtract(DateTime.Parse("01.01.1970"));
            return (((((ts.Days * 24) + ts.Hours) * 60) + ts.Minutes) * 60) + ts.Seconds;
        }

        static public void SQLiteLoadCurrenciesFromDatabase()
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn.Open();

            //sql_command = "INSERT INTO MSСurrencies (MSСurrencyId, MSСurrencyDate, MSСurrencyType, MSСurrencyCode, MSСurrencyNominal, MSСurrencyName, MSСurrencyValue) "
            //                              + "VALUES (           0,     1532044620,             '',          'AZN',                 1,             '',               1);";

            SQLiteCommand cmd = new SQLiteCommand("", conn);
            cmd.CommandText = "SELECT MSСurrencyId, MSСurrencyDate, MSСurrencyType, MSСurrencyCode, MSСurrencyNominal, MSСurrencyName, MSСurrencyValue"
              + " FROM MSСurrencies";
            try
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Program.currency.MSСurrencyId = int.Parse(dr["MSСurrencyId"].ToString());
                    Program.currency.MSСurrencyDate = DateTime.Parse((string)dr["MSСurrencyDate"]);
                    Program.currency.MSСurrencyType = (string)dr["MSСurrencyType"];
                    Program.currency.MSСurrencyCode = (string)dr["MSСurrencyCode"];
                    Program.currency.MSСurrencyNominal = int.Parse(dr["MSСurrencyNominal"].ToString());
                    Program.currency.MSСurrencyName = (string)dr["MSСurrencyName"];
                    Program.currency.MSСurrencyValue = float.Parse(dr["MSСurrencyValue"].ToString());
                    Program.currencies.Add(new MSСurrency(Program.currency));
                }
                dr.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
        }

        static public void SQLiteLoadAccountsFromDatabase()
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn.Open();

            //string sql_command = "INSERT INTO MSAccounts (MSAccountId, MSIO,     MSName, MSColor, MSImage, MSСurrencyCode, MSMulticurrency) "
            //                                + "VALUES (          0,  '+', 'Наличные',        1,   ';)',          'AZN',               0);";

            SQLiteCommand cmd = new SQLiteCommand("", conn);
            cmd.CommandText = "SELECT MSAccountId, MSIO, MSName, MSColor, MSImage, MSСurrencyCode, MSMulticurrency FROM MSAccounts";
            try
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Program.account.MSAccountId = int.Parse(dr["MSAccountId"].ToString());
                    Program.account.MSIO = ((string)dr["MSIO"])[0];
                    Program.account.MSName = (string)dr["MSName"];
                    Program.account.MSColor = (ConsoleColor)(int.Parse(dr["MSColor"].ToString()));
                    Program.account.MSImage = (string)dr["MSImage"];
                    Program.account.MSСurrencyCode = (string)dr["MSСurrencyCode"];
                    if (int.Parse(dr["MSMulticurrency"].ToString()) == 1) Program.account.MSMulticurrency = true; else Program.account.MSMulticurrency = false;
                    Program.accounts.Add(new MSAccount(Program.account));
                }
                dr.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
        }

        static public void SQLiteLoadCategoriesFromDatabase()
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn.Open();

            //sql_command = "INSERT INTO MSCategories (MSCategoryId, MSIO,     MSName, MSAccountId, MSColor, MSImage) "
            //                              + "VALUES (           0,  '+', 'Зарплата',           1,       1,    ';)');";

            SQLiteCommand cmd = new SQLiteCommand("", conn);
            cmd.CommandText = "SELECT MSCategoryId, MSIO, MSName, MSAccountId, MSColor, MSImage FROM MSCategories";
            try
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Program.category.MSCategoryId = int.Parse(dr["MSCategoryId"].ToString());
                    Program.category.MSIO = ((string)dr["MSIO"])[0];
                    Program.category.MSName = (string)dr["MSName"];
                    Program.category.MSAccountId = int.Parse(dr["MSAccountId"].ToString());
                    Program.category.MSColor = (ConsoleColor)(int.Parse(dr["MSColor"].ToString()));
                    Program.category.MSImage = (string)dr["MSImage"];
                    Program.categories.Add(new MSCategory(Program.category));
                }
                dr.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static public void SQLiteLoadTransactionsFromDatabase()
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn.Open();

            //sql_command = "INSERT INTO MSTransactions (MSTransactionId, MSIO, MSValue, MSСurrencyCode, MSAccountId, MSCategoryId, MSNote,            MSDateTime, MSMulticurrency) "
            //                                + "VALUES (              0,  '+',    0.01,          'AZN',           1,            1, 'Test', '20.07.2018 00:40:00',               0);";

            SQLiteCommand cmd = new SQLiteCommand("", conn);
            cmd.CommandText = "SELECT MSTransactionId, MSIO, MSValue, " +
                                     "MSСurrencyCode, MSAccountId, MSCategoryId, " +
                                     "MSNote, MSDateTime, MSMulticurrency " +
                              "FROM MSTransactions";
            try
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Program.transaction.MSTransactionId = int.Parse(dr["MSTransactionId"].ToString());
                    Program.transaction.MSIO            = ((string)dr["MSIO"])[0];
                    Program.transaction.MSValue         = float.Parse(dr["MSValue"].ToString());
                    Program.transaction.MSСurrencyCode  = (string)dr["MSСurrencyCode"];
                    Program.transaction.MSAccountId     = int.Parse(dr["MSAccountId"].ToString());
                    Program.transaction.MSCategoryId    = int.Parse(dr["MSCategoryId"].ToString());
                    Program.transaction.MSNote          = (string)dr["MSNote"];
                    Program.transaction.MSDateTime      = DateTime.Parse((string)dr["MSDateTime"]);
                    Program.transaction.MSMulticurrency = (string)dr["MSDateTime"] == "1" ? true : false;
                    Program.transactions.Add(new MSTransaction(Program.transaction));
                }
                dr.Close();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
