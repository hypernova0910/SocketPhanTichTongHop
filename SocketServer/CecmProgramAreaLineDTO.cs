using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay
{
    public class CecmProgramAreaLineDTO
    {
        public long gid { get; set; }
        public string code { get; set; }
        public int? corner_num { get; set; }
        public double start_x { get; set; }
        public double start_y { get; set; }
        public double end_x { get; set; }
        public double end_y { get; set; }
        public long? cecmprogramareasub_id { get; set; }
        public long? cecmprogramareamap_id { get; set; }
        public long? cecmprogram_id { get; set; }

    }
}
