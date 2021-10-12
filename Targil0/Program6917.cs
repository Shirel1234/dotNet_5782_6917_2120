using System;

namespace Targil0
{
   partial class Program
    {
        static void Main(string[] args)
        {
            Welcome6917();
            Welcome2120();
            Console.ReadKey();
        }
        static partial void Welcome2120();
        private static void Welcome6917()
        {
            Console.WriteLine("Enter your first name: ");
            string nameF = Console.ReadLine();
            Console.WriteLine("{0}, Welcome to my first console application", nameF);
        }
    }
}
