using CoordinateSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Common
{
    class Functions
    {
        public static double[] ConverLatLongToUTM(double latt, double longt)
        {
            //DateTime start = DateTime.Now;
            Coordinate coordinate = new Coordinate(latt, longt);
            double xUTM = coordinate.UTM.Easting;
            double yUTM = coordinate.UTM.Northing;
            //DateTime end = DateTime.Now;
            //MessageBox.Show((end - start).TotalMilliseconds.ToString());
            return new double[] { xUTM, yUTM };
        }
    }
}
