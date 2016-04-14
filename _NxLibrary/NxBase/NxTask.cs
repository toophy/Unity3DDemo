using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxBase
{
    public interface ITaskItem
    {
        bool CanNext();
    }

    /// <summary>
    /// 
    /// </summary>
    public class WaitForEndOfFrame : ITaskItem
    {
        public WaitForEndOfFrame()
        {

        }

        public bool CanNext()
        {
            return true;
        }
    }

    public class WaitForSecond : ITaskItem
    {
        DateTime _initTime = DateTime.Now;
        TimeSpan _waitTime;
        public WaitForSecond(TimeSpan waitTime)
        {
            _waitTime = waitTime;
        }

        public bool CanNext()
        {
            if ((_initTime + _waitTime) < DateTime.Now)
                return true;
            return false;
        }
    }

    /// <summary>
    ///  协程管理类
    /// </summary>
    public class NxTask : NxSingleton<NxTask>
    {
        List<IEnumerator> _taskList = new List<IEnumerator>();

        public void AddTask(IEnumerator newTask)
        {
            _taskList.Add(newTask);
        }

        public void DoTask()
        {
            for (int i = _taskList.Count - 1; i >= 0; --i )
            {
                var item = _taskList[i];
                var curTask = (ITaskItem)item.Current;
                if (curTask != null)
                {
                    if (curTask.CanNext())
                    {
                        if (!item.MoveNext())
                        {
                            _taskList.Remove(item);
                        }
                    }
                }
                else
                {
                    if (!item.MoveNext())
                    {
                        _taskList.Remove(item);
                    }
                }
            }
        }
    }
}
