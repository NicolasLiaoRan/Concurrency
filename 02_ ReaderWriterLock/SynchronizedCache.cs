using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _02__ReaderWriterLock
{
    public class SynchronizedCache
    {
        private ReaderWriterLockSlim cacheLocker = new ReaderWriterLockSlim();
        private Dictionary<int, string> innerCache = new Dictionary<int, string>();

        public int Count { get { return innerCache.Count; } }

        public string Read(int key)
        {
            cacheLocker.EnterReadLock();
            try
            {
                return innerCache[key];
            }
            finally
            {
                cacheLocker.ExitReadLock();
            }
        }
        public void Add(int key,string value)
        {
            cacheLocker.EnterWriteLock();
            try
            {
                innerCache.Add(key, value);
            }
            finally
            {
                cacheLocker.ExitWriteLock();
            }
        }
    }
}
