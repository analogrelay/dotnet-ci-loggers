using System;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public void WriteLength(string? foo)
        {
            Console.WriteLine(foo.Length);
        }

        public void Invalid()
        {
            foo;
        }
    }
}
