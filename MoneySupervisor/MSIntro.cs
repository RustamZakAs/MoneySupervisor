using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MoneySupervisor
{
    public static class MSIntro
    {
        public static void Show()
        {
            int DA = 244;
            int V = 212;
            int ID = 255;
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine();
                Console.WriteLine();
                Colorful.Console.WriteAscii(" RUSTAM ZAK", Color.FromArgb(DA, V, ID));
                Colorful.Console.WriteAscii("   PRESENT ", Color.FromArgb(DA, V, ID));

                DA -= 18;
                V -= 36;
            }
            System.Threading.Thread.Sleep(2000);
        }
    }
}