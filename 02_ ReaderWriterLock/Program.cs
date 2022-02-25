using System;
using System.Threading;

namespace _02__ReaderWriterLock
{
    class Program
    {
        private static ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private static long count = 0;
        static void Main(string[] args)
        {
            Thread read1 = new Thread(new ThreadStart(ReadSth));
            read1.Start();
            Console.WriteLine("{0} Create Thread ID {1} , Start ReadSomething", DateTime.Now.ToString("hh:mm:ss fff"), read1.GetHashCode());

            Thread read2 = new Thread(new ThreadStart(ReadSth));
            read2.Start();
            Console.WriteLine("{0} Create Thread ID {1} , Start ReadSomething", DateTime.Now.ToString("hh:mm:ss fff"), read2.GetHashCode());

            Thread write1 = new Thread(new ThreadStart(WriteSth));
            write1.Start();
            Console.WriteLine("{0} Create Thread ID {1} , Start WriteSomething", DateTime.Now.ToString("hh:mm:ss fff"), write1.GetHashCode());

            Thread.Sleep(5000);
            Thread read3 = new Thread(new ThreadStart(ReadSth));
            read3.Start();
            Console.WriteLine("{0} Create Thread ID {1} , Start ReadSomething", DateTime.Now.ToString("hh:mm:ss fff"), read3.GetHashCode());

            Console.ReadLine();
        }
        public static void ReadSth()
        {
            //Console.WriteLine("{0} Thread Id {1} Begin EnterReadLock", DateTime.Now.ToString("hh:mm:ss fff"), Thread.CurrentThread.GetHashCode());
            cacheLock.EnterReadLock();
            try
            {
                Console.WriteLine("{0} Thread Id {1} Reading,count is {2}", DateTime.Now.ToString("hh:mm:ss fff"), Thread.CurrentThread.GetHashCode(),count);
                Thread.Sleep(5000);
                Console.WriteLine("{0} Thread Id {1} Reading End", DateTime.Now.ToString("hh:mm:ss fff"), Thread.CurrentThread.GetHashCode());
            }
            finally
            {
                cacheLock.ExitReadLock();
                Console.WriteLine("{0} Thread Id {1} ExitReadLock...", DateTime.Now.ToString("hh:mm:ss fff"), Thread.CurrentThread.GetHashCode());
            }
        }
        public static void WriteSth()
        {
            //Console.WriteLine("{0} Thread Id {1} Begin EnterWriteLock", DateTime.Now.ToString("hh:mm:ss fff"), Thread.CurrentThread.GetHashCode());
            cacheLock.EnterWriteLock();
            try
            {
                Console.WriteLine("{0} Thread Id {1} Writing...", DateTime.Now.ToString("hh:mm:ss fff"), Thread.CurrentThread.GetHashCode());
                while(count<20)
                {
                    Thread.Sleep(100);
                    count++;
                }
                Console.WriteLine("{0} Thread Id {1} Writing End", DateTime.Now.ToString("hh:mm:ss fff"), Thread.CurrentThread.GetHashCode());
            }
            finally
            {
                cacheLock.ExitWriteLock();
                Console.WriteLine("{0} Thread Id {1} ExitWriteLock...", DateTime.Now.ToString("hh:mm:ss fff"), Thread.CurrentThread.GetHashCode());
            }
        }
    }

}
