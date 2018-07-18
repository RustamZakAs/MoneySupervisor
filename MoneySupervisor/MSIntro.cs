using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MoneySupervisor
{
    public static class MSIntro
    {
        public static void Show()
        {
            int DA = 244;
            int V  = 212;
            int ID = 255;
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine();
                Console.WriteLine();
                Colorful.Console.WriteAscii(" RUSTAM ZAK", Color.FromArgb(DA, V, ID));
                Colorful.Console.WriteAscii("   PRESENT ", Color.FromArgb(DA, V, ID));

                DA -= 18;
                V  -= 36;
            }
            System.Threading.Thread.Sleep(2000);
        }

        public static void Param(int id, int left, int top, ConsoleColor backColor, ConsoleColor fontColor)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = fontColor;
            string[] str = {"  .--.             ",
                            " /.-. '----------. ",
                            " \'-'.--\"--\"\"-\" - ' ",
                            "  '--'             "};
            string str0 = " Параметры ";
            for (int j = 0; j < str.Length; j++)
            {
                Console.SetCursorPosition(left, top++);
                Console.Write(str[j]);
                if (id == 3 & j == 3)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (j == 3)
                {
                    Console.SetCursorPosition(left+8, top-1);
                    Console.Write(str0);
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }
       
        public static void Plus(int id, int left, int top, ConsoleColor backColor, ConsoleColor fontColor)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = fontColor;
            string[] str = {"   __   ",
                            " _|  |_ ",
                            "|_    _|",
                            "  |__|  ",
                            " Доход  "};
            for (int j = 0; j < str.Length; j++)
            {
                Console.SetCursorPosition(left, top++);
                Console.Write(str[j]);
                if (id == 0 & j == 3)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Minus(int id, int left, int top, ConsoleColor backColor, ConsoleColor fontColor)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = fontColor;
            string[] str = {"        ",
                            " ______ ",
                            "|______|",
                            "        ",
                            " Расход "};
            for (int j = 0; j < str.Length; j++)
            {
                Console.SetCursorPosition(left, top++);
                Console.Write(str[j]);
                if (id == 1 & j == 3)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Transfer(int id, int left, int top, ConsoleColor backColor, ConsoleColor fontColor)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = fontColor;
            string[] str = {"        ",
                            " >____> ",
                            "        ",
                            " <____< ",
                            " Перевод"};
            for (int j = 0; j < str.Length; j++)
            {
                Console.SetCursorPosition(left, top++);
                Console.Write(str[j]);
                if (id == 2 & j == 3)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void NotFound(int id, int left, int top, ConsoleColor backColor, ConsoleColor fontColor)
        {
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = fontColor;
            string[] str = {@" _         _ ",
                            @"  \_('_')_/  ",
                            @"     | |     ",
                            @"     | \     " };
            string str0 = " Help";
            for (int j = 0; j < str.Length; j++)
            {
                Console.SetCursorPosition(left, top++);
                Console.Write(str[j]);
                if (id == 4 & j == 3)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (j == 3)
                {
                    Console.SetCursorPosition(left , top - 1);
                    Console.Write(str0);
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static ConsoleColor ChooseColor()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            ConsoleColor cc = new ConsoleColor();
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
                        cc = (ConsoleColor)Program.random.Next(0, 16);
                        break;
                    case ConsoleKey.UpArrow:
                        cc  = (ConsoleColor)Program.random.Next(0, 16);
                        break;
                    case ConsoleKey.LeftArrow:
                        cc = (ConsoleColor)Program.random.Next(0, 16);
                        break;
                    case ConsoleKey.RightArrow:
                        cc = (ConsoleColor)Program.random.Next(0, 16);
                        break;
                    case ConsoleKey.Enter:
                        return cc;
                    default:
                        break;
                }
            } while (true);
        }
    }
}