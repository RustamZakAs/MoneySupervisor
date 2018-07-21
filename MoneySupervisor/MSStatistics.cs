using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MoneySupervisor
{
    class MSStatistics
    {
        public static void TextStatistic()
        {
            char[] arr_str = null;
            using (StreamReader rfile = new StreamReader("file.txt"))
            {
                arr_str = rfile.ReadToEnd().ToCharArray();
            }
            Console.WriteLine("Исходная строка:\n >{0}< \n", new String(arr_str));
            var count = from p in arr_str
                        group p by p
                        into charGroup
                        orderby charGroup.Key
                        select charGroup;
            Console.WriteLine("Всего символов:\n {0} \n", arr_str.Length);
            foreach (var el in count)
            {
                double per = Convert.ToDouble((el.Count() * 100)) / arr_str.Length;
                Console.WriteLine("Char: {0} {1} {2:f2}%",
                    el.Key,
                    el.Count(),
                    per);
            }
            Console.ReadLine();
        }

        public static void Show(ref List<MSAccount>     accounts,
                                ref List<MSCategory>    categories,
                                ref List<MSTransaction> transactions)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            var grouped = transactions.GroupBy(x => x.MSAccountId)
                     .Select(g => new
                     {
                         Id = g.Key,
                         Sum = g.Sum(x => x.MSValue)
                     });
            foreach (var item in grouped)
            {
                Console.WriteLine($"{MSAccount.GetName(item.Id)} {item.Sum}");
            }

            foreach (var item_ac in accounts)
            {
                Console.Write($"{item_ac.MSName} {grouped}");
                foreach (var item_ca in categories)
                {

                }
            }
        }
    }
}
