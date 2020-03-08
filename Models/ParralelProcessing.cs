using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Models
{
    static class Parralel
    {
        static public void For(int start, int end, Action<int> action)
        {
            for (int i = start; i < end; i++)
            {
                BackgroundWorker b = new BackgroundWorker();
                b.DoWork += new DoWorkEventHandler((sender,e) => action.Invoke(i));
                b.RunWorkerAsync();
            }
        }

        static public void Foreach<T>(IEnumerable<T> container, Action<T> action)
        {
            foreach(T value in container)
            {
                BackgroundWorker b = new BackgroundWorker();
                b.DoWork += new DoWorkEventHandler((sender, e) => action.Invoke(value));
                b.RunWorkerAsync();
            }
        }
    }
}
