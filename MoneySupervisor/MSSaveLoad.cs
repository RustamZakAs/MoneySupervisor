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
    public static class MSSaveLoad
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

            System.Data.SQLite.SQLiteConnection conn1 = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn1.Open();

            string sql_command1 = "DROP TABLE IF EXISTS MSCategories;"
                                + "CREATE TABLE IF NOT EXISTS MSCategories ( "
                                + "MSCategoryId INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, "
                                + "MSIO         TEXT    NOT NULL CHECK (MSIO='+' OR MSIO='-'), "  //+ -
                                + "MSName       TEXT    NOT NULL, "  //Name
                                + "MSAccountId  INTEGER NOT NULL CHECK (MSAccountId >= 0), "  //id
                                + "MSColor      INTEGER NOT NULL CHECK (MSColor >= 0), "  //Color int
                                + "MSImage      TEXT    NOT NULL );"; //Symbols
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command1, conn1);
            command.ExecuteNonQuery();

            sql_command1 = "DROP TABLE IF EXISTS MSAccounts;"
                         + "CREATE TABLE IF NOT EXISTS MSAccounts ( "
                         + "MSAccountId	    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, "
                         + "MSIO	        TEXT    NOT NULL CHECK (MSIO='+' OR MSIO='-'), " //+ -
                         + "MSName	        TEXT    NOT NULL, "  //Name
                         + "MSColor	        INTEGER NOT NULL CHECK (MSColor >= 0), " //Color int
                         + "MSImage	        TEXT    NOT NULL, "  //Symbols
                         + "MSValute        TEXT    NOT NULL CHECK (LEN(MSValute) = 3), "  //AZN RUB USD CNY 
                         + "MSMulticurrency INTEGER NOT NULL CHECK (MSMulticurrency = 0 OR MSMulticurrency = 1), "
                         + "); ";
            command = new System.Data.SQLite.SQLiteCommand(sql_command1, conn1);
            command.ExecuteNonQuery();

            sql_command1 = "DROP TABLE IF EXISTS MSTransactions;"
                         + "CREATE TABLE IF NOT EXISTS MSTransactions ( "
                         + "MSTransactionId	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE                  , "
                         + "MSIO	        TEXT    NOT NULL CHECK (MSIO='+' OR MSIO='-')                      , "
                         + "MSValue	        NUMERIC NOT NULL CHECK (MSValue >= 0.000001)                       , "
                         + "MSValute	    TEXT    NOT NULL CHECK (LEN(MSValute) = 3)                         , "
                         + "MSAccountId	    INTEGER NOT NULL CHECK (MSAccountId >= 0)                          , "
                         + "MSCategoryId	INTEGER NOT NULL CHECK (MSCategoryId >= 0)                         , "
                         + "MSNote	        TEXT                                                               , "
                         + "MSDateTime	    INTEGER NOT NULL CHECK (MSDateTime >= 0)                           , "
                         + "MSMulticurrency INTEGER NOT NULL CHECK (MSMulticurrency = 0 OR MSMulticurrency = 1), "
                         + "); ";
            command = new System.Data.SQLite.SQLiteCommand(sql_command1, conn1);
            command.ExecuteNonQuery();

            conn1.Close();
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
            System.Data.SQLite.SQLiteConnection conn1 = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn1.Open();
            
            string sql_command1 = "INSERT INTO MSCategories (MSIO,     MSName, MSAccountId, MSColor, MSImage) "
                                                + "VALUES ( '+', 'Зарплата',            1,      1,    ';)');";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command1, conn1);
            command.ExecuteNonQuery();

            sql_command1 = "INSERT INTO MSAccounts (MSIO,     MSName, MSColor, MSImage, MSValute, MSMulticurrency) "
                                         + "VALUES ( '+', 'Наличные',        1,   ';)',    'AZN',               0);";
            command = new System.Data.SQLite.SQLiteCommand(sql_command1, conn1);
            command.ExecuteNonQuery();

            sql_command1 = "INSERT INTO MSTransactions (MSIO, MSValue, MSValute, MSAccountId, MSCategoryId, MSNote, MSDateTime, MSMulticurrency) "
                                             + "VALUES ( '+',    0.01,    'AZN',           1,            1, 'Test',   25532640,               0);";
            command = new System.Data.SQLite.SQLiteCommand(sql_command1, conn1);
            command.ExecuteNonQuery();

            conn1.Close();
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
        /*
        static public void SaveAll(object obj, string fileName)
        {
            if (File.Exists($"{fileName}.json"))
            {
                File.Delete($"{fileName}.json");
            }
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(obj.GetType());

            using (FileStream fs = new FileStream($"{fileName}.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, obj);
            }
        }

        static public object ReadAll(object objectType, string fileName)
        {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(objectType.GetType());
            object newpeople;
            try
            {
                using (FileStream fs = new FileStream($"{fileName}.json", FileMode.OpenOrCreate))
                {
                    if (fs.Length > 0)
                        newpeople = jsonFormatter.ReadObject(fs);
                    else return new List<Owner>(); //--error if "new object()"
                }
                return newpeople;
            }
            catch (Exception ew)
            {
                Console.WriteLine(ew);
                throw;
            }
        }
        */
    }
}
