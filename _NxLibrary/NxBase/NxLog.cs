using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxBase
{
    public class NxLog
    {
        static public void v<T>(T classType,String content)
        {
            Type t = typeof(T);
            Console.WriteLine("时间：" + DateTime.Now.ToLongTimeString() + "类" + t.Name + ":" + content);
        }

        static public void v<T>(String content)
        {
            Type t = typeof(T);
            Console.WriteLine("时间：" + DateTime.Now.ToLongTimeString() + "类" + t.Name + ":" + content);
        }
    }
}
