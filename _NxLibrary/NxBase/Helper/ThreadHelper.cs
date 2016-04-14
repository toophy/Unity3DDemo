using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NxBase.Helper
{
    public class ThreadHelper
    {
        public static void Ctreate(ref Thread thread, Action threadMethod)
        {
            if (thread == null)
            {
                thread = new Thread(new ThreadStart(threadMethod));
                thread.Start();
            }
        }

        public static void Destroy(ref Thread thread)
        {
            if(thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        public static void Reset(ref Thread thread, Action threadMethod)
        {
            ThreadHelper.Destroy(ref thread);
            ThreadHelper.Ctreate(ref thread, threadMethod);
        }
    }
}
