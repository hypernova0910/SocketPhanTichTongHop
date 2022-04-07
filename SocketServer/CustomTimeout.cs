using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Delaunay
{
    class CustomTimeout : Timer
    {
        public CustomTimeout(Action action, double delay)
        {
            this.AutoReset = false;
            this.Interval = delay;
            this.Elapsed += (sender, args) => action();
            this.Start();
        }
    }
}
