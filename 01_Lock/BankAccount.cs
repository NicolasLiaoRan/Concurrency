using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _01_Lock
{
    class BankAccount
    {
        private long id;
        private decimal m_balance = 0.0M;

        private object m_balanceLock = new object();
        //存放
        public void Deposit(decimal delta)
        {
            lock(m_balanceLock)
            {
                m_balance += delta;
            }
        }
        //取出
        public void Withdraw(decimal delta)
        {
            lock(m_balanceLock)
            {
                if (m_balance < delta)
                    throw new Exception("Insufficient funds");
                m_balance -= delta;
            }
        }
        //查询
        public void Query()
        {
            Console.WriteLine($"Account:{id}");
        }
        public static void ErrorTransfer(BankAccount a,BankAccount b,decimal delta)
        {
            a.Withdraw(delta);
            b.Deposit(delta);
        }
        public static void DangerTransfer(BankAccount a,BankAccount b,decimal delta)
        {
            lock(a.m_balanceLock)
            {
                lock(b.m_balanceLock)
                {
                    a.Withdraw(delta);
                    b.Deposit(delta);
                }
            }
        }
        public static void RightTransfer(BankAccount a,BankAccount b,decimal delta)
        {
            if(a.id<b.id)
            {
                Monitor.Enter(a.m_balanceLock);
                Monitor.Enter(b.m_balanceLock);
            }
            else
            {
                Monitor.Enter(b.m_balanceLock);
                Monitor.Enter(a.m_balanceLock);
            }
            try
            {
                a.Withdraw(delta);
                b.Deposit(delta);
            }
            finally
            {
                Monitor.Exit(a.m_balanceLock);
                Monitor.Exit(b.m_balanceLock);
            }
        }
    }
}
