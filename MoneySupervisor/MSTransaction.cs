using System;
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
        public string   MSСurrencyCode  { get; set; }
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
        
        public void ConsoleAdd(int msTransactionId)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            MSTransactionId = msTransactionId;
            bool xreplace = true;
            do
            {
                Console.SetCursorPosition(left, top);
                Console.WriteLine("Введите тип транзакции (+,-): ");
                string temp = Console.ReadLine();
                if (temp[0] == '+' | temp[0] == '-')
                {
                    MSIO = temp[0];
                    xreplace = false;
                }
            } while (xreplace);
            Console.WriteLine("Введите значение (сумма): ");
            MSValue = float.Parse(Console.ReadLine());
            Console.WriteLine("Выберите тип валюты: ");
            //MSСurrency = Console.ReadLine();
            MSСurrencyCode = MSСurrency.ChooseСurrency(ref Program.currencies);
            Console.WriteLine("Выберите аккаунт: ");
            if (Program.accounts.Count > 0)
                MSAccountId = MSAccount.ChooseAccount(ref Program.accounts);
            Console.WriteLine("Выберите категорию: ");
            if (Program.categories.Count > 0)
                MSCategoryId = MSCategory.ChooseCategory(ref Program.categories);
            Console.WriteLine("Введите заметку: ");
            MSNote = Console.ReadLine();
            Console.WriteLine("Введите дату и время (DD.MM.YYYY hh.mm.ss): ");
            MSDateTime = DateTime.Parse(Console.ReadLine());
            if (MSСurrencyCode == "ALL")
            {
                MSMulticurrency = true;
            }
            else MSMulticurrency = false;
        }
    }
}
