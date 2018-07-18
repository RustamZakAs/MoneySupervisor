using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MoneySupervisor
{
    [DataContract]
    class MSAccount
    {
        [DataMember]
        public int          MSAccountId { get; set; }
        [DataMember]
        public char         MSIO { get; set; }
        [DataMember]
        public string       MSName { get; set; }
        [DataMember]
        public ConsoleColor MSColor { get; set; }
        [DataMember]
        public string       MSImage { get; set; }

        public MSAccount()
        {

        }

        public MSAccount(int          mSAccountId, 
                         char         mSIO, 
                         string       mSName, 
                         ConsoleColor mSColor, 
                         string       mSImage)
        {
            MSAccountId = mSAccountId;
            MSIO        = mSIO;
            MSName      = mSName;
            MSColor     = mSColor;
            MSImage     = mSImage;
        }

        public MSAccount(MSAccount account)
        {
            MSAccountId = account.MSAccountId;
            MSIO        = account.MSIO;
            MSName      = account.MSName;
            MSColor     = account.MSColor;
            MSImage     = account.MSImage;
        }

        public void ConsoleAdd()
        {

        }

        public static int ChooseAccount(ref List<MSAccount> accountList)
        {
            int left = Console.CursorLeft;
            int top  = Console.CursorTop;
            int maxLen = 0;
            if (accountList.Count > 0)
                maxLen = accountList.Max(s => s.MSName).Length;
            int accountId = 0;
            do
            {
                Console.SetCursorPosition(left, top);
                if (Console.KeyAvailable)
                {
                    Program.cki = Console.ReadKey();
                }
                switch (Program.cki.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (++accountId >= accountList.Count) accountId = accountList.Count-1;
                        break;
                    case ConsoleKey.UpArrow:
                        if (--accountId < 0) accountId = 0;
                        break;
                    case ConsoleKey.Enter:
                        return accountId;
                    default:
                        break;
                }
                Console.WriteLine($"/{accountList[accountId].MSImage}/ {accountList[accountId].MSName}");
            } while (true);
        }
    }
}
