using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxNetwork
{
    using DWORD = System.UInt32;
    using TCHAR = System.Byte;
    using BYTE = System.Byte;
    using LONGLONG = System.Int64;
    using LONG = System.Int32;
    using BOOL = System.Int32;
    using WORD = System.UInt16;
    using COLORREF = System.UInt32;

    class GlobalVar
    {
        public const DWORD HEAD_SIZE = 4;         //消息头
        public const DWORD ID_SIZE = 2;           //消息号ID长度
        public const DWORD ORDER_SIZE = 4;        //消息顺序ID长度
        public const DWORD TIMESTAMP_SIZE = 4;    //时间戳长度

        public const DWORD MAX_NAMESIZE = 32;     //昵称最大长度
        public const DWORD MAX_ACCNAMESIZE = 48;  //账户最大长度
        public const DWORD MAX_PASSWORDSIZE = 16; //密码最大长度

        public const DWORD BUFFER_LENGTH = 1024 * 64;           //消息byte数组最大长度
        public const DWORD BUFFER_LENGTH_MINI = 1024 * 4;       //消息byte最大长度
        public const DWORD CACHE_QUEUE_MAX = 100;               //缓存消息的最大长度
    } 
}
