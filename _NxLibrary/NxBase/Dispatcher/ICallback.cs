using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxBase.Dispatcher
{
    public interface ICallback<out T>
    {
        T GetData(byte[] data);
        void Exce(byte[] data);
    }
}
