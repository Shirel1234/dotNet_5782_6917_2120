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
            Console.WriteLine("Enter your name: ");
            string firstName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", firstName);
        }
    }
}
