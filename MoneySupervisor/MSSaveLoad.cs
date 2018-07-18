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


            if (!File.Exists(curFile))
            {
                Console.WriteLine("Создание файла с базой данных.");
                System.Data.SQLite.SQLiteConnection.CreateFile("MSBase.sqlite");
            }

            System.Data.SQLite.SQLiteConnection conn1 = new System.Data.SQLite.SQLiteConnection("Data Source=MSBase.sqlite;Version=3;");
            conn1.Open();

            string sql_command1 = "DROP TABLE IF EXISTS MSCategory;"
                  + "CREATE TABLE IF NOT EXISTS MSCategory ("
                  + "MSCategoryId INTEGER PRIMARY KEY AUTOINCREMENT, "
                  + "MSIO         TEXT, "
                  + "MSName       TEXT, "
                  + "MSAccountId  INTEGER, "
                  + "MSColor      INTEGER,"
                  + "MSImage      TEXT);";

            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command1, conn1);
            command.ExecuteNonQuery();

            sql_command1 = "DROP TABLE IF EXISTS MSAccount;"
            + "CREATE TABLE IF NOT EXISTS MSAccount ( "
            + "MSAccountId	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, "
            + "MSIO	TEXT NOT NULL, "
            + "MSName	TEXT NOT NULL, "
            + "MSColor	INTEGER NOT NULL, "
            + "MSImage	TEXT NOT NULL );";
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
            
            string sql_command1 = "INSERT INTO MSCategory (MSIO,     MSName, MSAccountId, MSColor, MSImage) "
                                                   + "VALUES ('+', 'Зарплата',            1,      1,    ';)');";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command1, conn1);

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
