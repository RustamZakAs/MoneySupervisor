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
    class MSCategory
    {
        //[DataMember]
        public int MSCategoryId { get; set; }
        //[DataMember]
        public char MSIO { get; set; }
        //[DataMember]
        public string MSName { get; set; }
        //[DataMember]
        public int MSAccountId { get; set; }
        //[DataMember]
        public ConsoleColor MSColor { get; set; }
        //[DataMember]
        public string MSImage { get; set; }

        public MSCategory()
        {
        }

        public MSCategory(int msCategoryId,
                          char msIO,
                          string msName,
                          int msAccountId,
                          ConsoleColor msColor,
                          string msImage)
        {
            MSCategoryId = msCategoryId;
            MSIO = msIO;
            MSName = msName;
            MSAccountId = msAccountId;
            MSColor = msColor;
            MSImage = msImage;
        }

        public MSCategory(MSCategory MScategory)
        {
            MSCategoryId = MScategory.MSCategoryId;
            MSIO = MScategory.MSIO;
            MSName = MScategory.MSName;
            MSAccountId = MScategory.MSAccountId;
            MSColor = MScategory.MSColor;
            MSImage = MScategory.MSImage;
        }

        public void ConsoleAdd(int categoryId, char msIO, int left = 0)
        {
            left = Console.CursorLeft;
            int top = Console.CursorTop;
            MSCategoryId = categoryId;

            if (msIO == '+' | msIO == '-') MSIO = msIO;
            else
            {
                bool xreplace = true;
                do
                {
                    Console.SetCursorPosition(left, top);
                    Console.WriteLine("Введите тип категории (+,-): ");
                    string temp = Console.ReadLine();
                    if (temp[0] == '+' | temp[0] == '-')
                    {
                        MSIO = temp[0];
                        xreplace = false;
                    }
                } while (xreplace);
            }

            Console.WriteLine("Введите наименование категории: ");
            MSName = Console.ReadLine();
            Console.WriteLine("Выберите аккаунт: ");
            if (Program.accounts.Count > 0)
                MSAccountId = MSAccount.ChooseAccount(ref Program.accounts);
            else
            {
                Program.account.ConsoleAdd(Program.accounts.Count + 1, '+');
                Program.accounts.Add(Program.account);
            }
            Console.WriteLine("Выберите цвет категории: ");
            MSColor = MSIntro.ChooseColor();
            Console.WriteLine("Введите символ категории: ");
            MSImage = Console.ReadLine();
        }

        public static int ChooseCategory(ref List<MSCategory> msCategoryList, int msAccountId)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            List<MSCategory> TmsCategoryList = new List<MSCategory>();

            for (int i = 0; i < msCategoryList.Count; i++)
            {
                if (msCategoryList[i].MSAccountId == msAccountId)
                {
                    TmsCategoryList.Add(new MSCategory(msCategoryList[i]));
                }
            }

            int maxLen = 0;
            string xSynbol = "↓ "; // ↓   ↑   ↓↑
            if (TmsCategoryList.Count > 0)
            {
                maxLen = TmsCategoryList.Max(s => s.MSName).Length;
                if (TmsCategoryList.Count == 1) xSynbol = "  ";
            }
            else
            {
                int tCatCount = Program.categories.Count;
                do
                {
                    tCatCount++;
                    if (!TmsCategoryList.Exists(x => x.MSCategoryId == tCatCount)) //Error?
                    {
                        Console.WriteLine();
                        int maxWidth = 40, maxheight = 65;
                        Console.SetWindowSize(maxWidth, 25);
                        Console.SetBufferSize(maxWidth, maxheight);
                        Program.category.ConsoleAdd(tCatCount, Program.transactionSymbol);
                        Program.categories.Add(new MSCategory(Program.category));
                        MSCategory.SQLiteInsertCategoryInDatabase(Program.category);
                        break;
                    }
                } while (true);
            }

            if (TmsCategoryList.Exists(x => x.MSAccountId == msAccountId))
                Console.WriteLine($"{TmsCategoryList[msAccountId].MSImage} {TmsCategoryList[msAccountId].MSName} {xSynbol}");
            else
            {
                int tCatCount = Program.categories.Count;
                do
                {
                    tCatCount++;
                    if (!TmsCategoryList.Exists(x => x.MSCategoryId == tCatCount))
                    {
                        Console.WriteLine();
                        int maxWidth = 40, maxheight = 65;
                        Console.SetWindowSize(maxWidth, 25);
                        Console.SetBufferSize(maxWidth, maxheight);
                        Program.category.ConsoleAdd(tCatCount, Program.transactionSymbol);
                        Program.categories.Add(new MSCategory(Program.category));
                        MSCategory.SQLiteInsertCategoryInDatabase(Program.category);
                        break;
                    }
                } while (true);
            }

            int changeAccountId = 0;
            do
            {
                //if (Console.KeyAvailable)
                //{
                Program.cki = Console.ReadKey();
                //}
                switch (Program.cki.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (++changeAccountId >= TmsCategoryList.Count) changeAccountId = TmsCategoryList.Count - 1;
                        for (int i = 0; i < maxLen + 3; i++)
                        {
                            Console.Write(" ");
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (--changeAccountId < 0) changeAccountId = 0;
                        for (int i = 0; i < maxLen + 3; i++)
                        {
                            Console.Write(" ");
                        }
                        break;
                    case ConsoleKey.Enter:
                        Program.cki = default(ConsoleKeyInfo);
                        Console.SetCursorPosition(0, top + 1);
                        return TmsCategoryList[changeAccountId].MSCategoryId;
                    default:
                        break;
                }
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"{TmsCategoryList[changeAccountId].MSImage} {TmsCategoryList[changeAccountId].MSName} {xSynbol}");
            } while (true);
        }

        public static string GetName(int MSCategoryId)
        {
            for (int i = 0; i < Program.categories.Count; i++)
            {
                if (Program.categories[i].MSCategoryId == MSCategoryId)
                    return Program.categories[i].MSName;
            }
            return "";
        }

        static public void SQLiteInsertCategoryInDatabase(MSCategory c)
        {
            Program.conn.Open();
            string sql_command = "INSERT INTO MSCategories (MSCategoryId, " +
                                                    "MSIO,     " +
                                                    "MSName, " +
                                                    "MSAccountId, " +
                                                    "MSColor, " +
                                                    "MSImage) "
                                          + $"VALUES ({c.MSCategoryId}," +
                                            $"'{c.MSIO}'," +
                                            $"'{c.MSName}'," +
                                            $" {c.MSCategoryId}," +
                                            $" {(int)c.MSColor}," +
                                            $"'{c.MSImage})');";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql_command, Program.conn);
            command.ExecuteNonQuery();
            Program.conn.Close();
        }
    }
}
