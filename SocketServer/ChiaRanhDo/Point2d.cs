using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay
{
    public class Point2d
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2d()
        {

        }

        public Point2d(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
