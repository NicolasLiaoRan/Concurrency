using System;
using System.Threading;
using System.Threading.Tasks;

namespace _01_Lock
{
    class Program
    {
        public static object locker = new object();
        //private readonly static object locker = new object();

        private static long index;
        static void Main(string[] args)
        {
            #region 读写锁不同步
            /*
            Thread addThread = new Thread(new ParameterizedThreadStart(AddIndex));
            addThread.Start("Add Begin");

            Thread getThread = new Thread(new ParameterizedThreadStart(GetIndex));
            getThread.Start("Get Begin");
            */
            #endregion

            #region 读写撕裂
            TestAtomicity();
            #endregion

            #region 粒度错误（待完善）
            #endregion

            Console.ReadLine();
        }
        public static void AddIndex(object state)
        {
            lock (locker)
            {
                Console.WriteLine(state.ToString());//以此表示一些无意义操作
                while (true)
                {
                    Thread.Sleep(20);
                    index++;
                    if (index > 100)
                    {
                        index = 0;
                    }
                }
            }
        }
        public static void GetIndex(object state)
        {
            Console.WriteLine(state.ToString());
            while (true)
            {
                //读没有加锁，因此读操作可以在任何时候访问共享的index变量，也就会出现index++，index=101的一瞬间被读出
                //然后在执行下一步操作时，index又被重置为0
                if (index == 101)
                    Console.WriteLine($"101 should not exists:{index}");
            }
        }
        public static void TestAtomicity()
        {
            long test = 0;
            long breakFlag = 0;
            int index = 0;
            Task.Run(() =>
            {
                Console.WriteLine($"Start Cycle:Write Data");
                while (true)
                {
                    test = (index % 2 == 0) ? 0x0 : 0x1234567890abcdef;
                    index++;
                    if (Interlocked.Read(ref breakFlag) > 0)
                        break;
                }
                Console.WriteLine($"Exit Cycle:Write Data");
            });
            Task.Run(() =>
            {
                Console.WriteLine($"Start Cycle:Read Data");
                while (true)
                {
                    long temp = test;
                    if (temp != 0 && temp != 0x1234567890abcdef)
                    {
                        Interlocked.Increment(ref breakFlag);
                        Console.WriteLine($"ReadWrite laceration:{Convert.ToString(temp, 16)}");
                        break;
                    }
                }
                Console.WriteLine($"Exit Cycle:Read Data");
            });
        }

    }
}
