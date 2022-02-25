using System;
using System.Threading;
using System.Threading.Tasks;

namespace _03_InterLocked
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() =>
            {
                long total = 0;
                long result = 0;

                Console.WriteLine("Counting!...");

                Parallel.For(0, 10, x =>
                {
                    for (int j = 0; j < 1000000; j++)
                    {
                        Interlocked.Increment(ref total);
                        result++;
                    }
                });
                Console.WriteLine($"Atomy result should be:{10 * 1000000}");
                Console.WriteLine($"Atomy result is:{total}");
                Console.WriteLine($"Increment result is:{result}");
            });
            Console.ReadLine();
        }
    }
}
