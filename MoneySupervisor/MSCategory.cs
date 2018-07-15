using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneySupervisor
{
    class MSCategory
    {
        public int          MSCategoryId { get; set; }
        public char         MSIO { get; set; }
        public string       MSName { get; set; }
        public int          MSAccountId { get; set; }
        public ConsoleColor MSColor { get; set; }
        public string       MSImage { get; set; }

        public MSCategory()
        {
        }

        public MSCategory(int mSCategoryId, 
                          char mSIO, 
                          string mSName, 
                          int mSAccountId, 
                          ConsoleColor mSColor, 
                          string mSImage)
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

        public void Add ()
        {

        }
    }
}
