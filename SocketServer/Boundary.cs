using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay
{
    //in UTM
    class Boundary
    {
        public double minLat { get; set; }
        public double minLong { get; set; }
        public double maxLat { get; set; }
        public double maxLong { get; set; }

        public List<CecmProgramAreaLineDTO> lstRanhDo { get; set; }

        public DateTime timeSent { get; set; }

        public int khoangPT { get; set; }

        public double nguongBom { get; set; }
        public double nguongMin { get; set; }
        public int minClusterSize { get; set; }

        public Boundary()
        {
            minLat = -1;
            minLong = -1;
            maxLat = -1;
            maxLong = -1;
            khoangPT = 0;
            lstRanhDo = new List<CecmProgramAreaLineDTO>();
        }

        public Boundary(double minLat_, double minLong_, double maxLat_, double maxLong_, int khoangPT_)
        {
            minLat = minLat_;
            minLong = minLong_;
            maxLat = maxLat_;
            maxLong = maxLong_;
            khoangPT = khoangPT_;
            lstRanhDo = new List<CecmProgramAreaLineDTO>();
        }
    }
}
