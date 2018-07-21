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

        public void ConsoleAdd(int categoryId, char msIO)
        {
            int left = Console.CursorLeft;
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
            List<MSCategory> TmsCategoryList = new List<MSCategory>();
            for (int i = 0; i < msCategoryList.Count; i++)
            {
                if (msCategoryList[i].MSAccountId == msAccountId)
                {
                    TmsCategoryList.Add(new MSCategory(msCategoryList[i]));
                }
            }
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            int maxLen = 0;
            if (TmsCategoryList.Count > 0)
                maxLen = TmsCategoryList.Max(s => s.MSName).Length;
            int accountId = 0;
            string xSynbol = "↓ "; // ↓   ↑   ↓↑
            if (TmsCategoryList.Count == 1) xSynbol = "  ";
            if (TmsCategoryList.Exists(x => x.MSAccountId == accountId))
                Console.WriteLine($"{TmsCategoryList[accountId].MSImage} {TmsCategoryList[accountId].MSName} {xSynbol}");
            else
            {
                int tCatCount = Program.categories.Count;
                do
                {
                    tCatCount++;
                    if (!TmsCategoryList.Exists(x => x.MSCategoryId == tCatCount))
                        Program.category.ConsoleAdd(tCatCount, Program.transactionSymbol);
                } while (true);
            }
            do
            {
                //if (Console.KeyAvailable)
                //{
                Program.cki = Console.ReadKey();
                //}
                switch (Program.cki.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (++accountId >= TmsCategoryList.Count) accountId = TmsCategoryList.Count - 1;
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
                        return TmsCategoryList[accountId].MSCategoryId;
                    default:
                        break;
                }
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"{TmsCategoryList[accountId].MSImage} {TmsCategoryList[accountId].MSName} {xSynbol}");
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
    }
}
