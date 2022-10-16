using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay
{
    public class InfoConnect
    {
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string code { get; set; }
        public long project_id { get; set; }
        public string machineBomCode { get; set; }
        public double lat_value { get; set; }
        public double long_value { get; set; }

        public GeoJsonPoint<GeoJson2DCoordinates> coordinate { get; set; }
        public double the_value { get; set; }
        public DateTime update_time { get; set; }
        public DateTime time_action { get; set; }
        public int bit_sens { get; set; }
        public bool isMachineBom { get; set; }

        public double dilution { get; set; }

        public int satelliteCount { get; set; }
    }
}
