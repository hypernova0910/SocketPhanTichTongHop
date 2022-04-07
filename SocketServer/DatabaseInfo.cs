using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay
{
    class DatabaseInfo
    {
        //public MongoDB.Bson.ObjectId _id { get; set; }

        [BsonElement("project_id")]
        public string project_id { get; set; }

        [BsonElement("the_value")]
        public string the_value { get; set; }

        [BsonElement("lat_value")]
        public string lat_value { get; set; }

        [BsonElement("long_value")]
        public string long_value { get; set; }

        [BsonElement("update_time")]
        public string update_time { get; set; }

        [BsonElement("time_action")]
        public string time_action { get; set; }

        [BsonElement("code")]
        public string code { get; set; }

        [BsonElement("bit_sens")]
        public string bit_sens { get; set; }

        [BsonElement("isMachineBom")]
        public string isMachineBom { get; set; }

        [BsonElement("machineBomCode")]
        public string machineBomCode { get; set; }
    }
}
