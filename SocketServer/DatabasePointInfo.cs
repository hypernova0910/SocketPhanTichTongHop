using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delaunay
{
    class DatabasePointInfo
    {
        public string _id { get; set; }
        public string project_id { get; set; }
        public double the_value { get; set; }
        public double lat_value { get; set; }
        public double long_value { get; set; }
        public string update_time { get; set; }
        public string time_action { get; set; }
        public string code { get; set; }
        public string bit_sens { get; set; }
        public string isMachineBom { get; set; }
        public string machineBomCode { get; set; }

    }
}
