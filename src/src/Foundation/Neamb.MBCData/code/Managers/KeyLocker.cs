using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    public class KeyLocker : IDisposable
    {
        private object padlock;

        public KeyLocker(object locker)
        {
            padlock = locker;
        }

        public void Dispose()
        {
            // when this falls out of scope (after a using {...} ), release the lock
            Monitor.Exit(padlock);
        }
    }
}