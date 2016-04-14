using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxBase.Helper
{
    public class DispatcherHelper
    {
        public static string GetKey<T>()
        {
            Type t = typeof(T);
            return t.Name;
        }
    }
}
