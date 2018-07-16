using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneySupervisor
{
    class MSAccount
    {
        public int          MSAccountId { get; set; }
        public char         MSIO { get; set; }
        public string       MSName { get; set; }
        public ConsoleColor MSColor { get; set; }
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

        public void Add()
        {

        }

        public int ChooseAccount(ref List<MSAccount> accountList)
        {
            int left = Console.CursorLeft;
            int top  = Console.CursorTop;
            int maxLen = accountList.Max(s => s.MSName).Length;
            do
            {
                Console.SetCursorPosition(left, top);

                return 0;
            } while (true);
        }
    }
}
