using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace MoneySupervisor
{
    [DataContract]
    class MSCategory
    {
        [DataMember]
        public int          MSCategoryId { get; set; }
        [DataMember]
        public char         MSIO { get; set; }
        [DataMember]
        public string       MSName { get; set; }
        [DataMember]
        public int          MSAccountId { get; set; }
        [DataMember]
        public ConsoleColor MSColor { get; set; }
        [DataMember]
        public string       MSImage { get; set; }

        public MSCategory()
        {
        }

        public MSCategory(int          mSCategoryId, 
                          char         mSIO, 
                          string       mSName, 
                          int          mSAccountId, 
                          ConsoleColor mSColor, 
                          string       mSImage)
        {
            MSCategoryId = mSCategoryId;
            MSIO         = mSIO;
            MSName       = mSName;
            MSAccountId  = mSAccountId;
            MSColor      = mSColor;
            MSImage      = mSImage;
        }

        public MSCategory(MSCategory MScategory)
        {
            MSCategoryId = MScategory.MSCategoryId;
            MSIO         = MScategory.MSIO;
            MSName       = MScategory.MSName;
            MSAccountId  = MScategory.MSAccountId;
            MSColor      = MScategory.MSColor;
            MSImage      = MScategory.MSImage;
        }

        public void ConsoleAdd(int categoryId)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            MSCategoryId = categoryId;
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
            Console.WriteLine("Введите наименование категории: ");
            MSName = Console.ReadLine();
            Console.WriteLine("Выберите аккаунт: ");
            if (Program.accounts.Count > 0)
                MSAccountId = MSAccount.ChooseAccount(ref Program.accounts);
            else
            {
                Program.account.Add();
                Program.accounts.Add(Program.account);
            }
            Console.WriteLine("Введите символ: ");
            MSColor = MSIntro.ChooseColor();
            Console.WriteLine("Введите символ: ");
            MSImage = Console.ReadLine();
        }
    }
}
