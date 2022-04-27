///////////////////////////////////////////////////////////////////////////////
//
//  Vertex.cs
//
//  By Philip R. Braica (HoshiKata@aol.com, VeryMadSci@gmail.com)
//
//  Distributed under the The Code Project Open License (CPOL)
//  http://www.codeproject.com/info/cpol10.aspx
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

// Namespace
namespace gg.Mesh
{
    

    /// <summary>
    /// Vertex class
    /// </summary>
    public class Vertex : IComparable<Vertex>
    {
        public static int BOM = 1;
        public static int CAMCO = 2;

        public static int TYPE_BOMB = 1;
        public static int TYPE_MINE = 2;
        /// <summary>
        /// Vertex default constructor.
        /// </summary>
        public Vertex()
        {
            X = 0;
            Y = 0;
            Z = 0;
            Type = Vertex.BOM;
            depth = 0;
            id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Vertex constructor.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// 

        public Vertex(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            Type = Vertex.BOM;
            depth = 0;
            id = Guid.NewGuid().ToString();
        }

        public Vertex(double x, double y, double z, int type)
        {
            X = x;
            Y = y;
            Z = z;
            depth = 0;
            Type = Vertex.BOM;
            TypeBombMine = type;
            id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// X coordinate.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z coordinate.
        /// </summary>
        public double Z { get; set; }

        public double depth { get; set; }

        /// <summary>
        /// Index of this vertex.
        /// </summary>
        public int Index { get; set; }

        public int Type { get; set; }

        public int TypeBombMine { get; set; }

        public int BitSent { get; set; }

        public string MachineCode { get; set; }

        public string id { get; set; }

        /// <summary>
        /// Delta distance squared between this and other vertex t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public double DeltaSquaredXY(Vertex t)
        {
            double dx = (X - t.X);
            double dy = (Y - t.Y);
            return (dx * dx) + (dy * dy);
        }

        /// <summary>
        /// Delta distance squared between this and other vertex t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public double DeltaSquared(Vertex t)
        {
            double dx = (X - t.X);
            double dy = (Y - t.Y);
            double dz = (Z - t.Z);
            return (dx * dx) + (dy * dy) + (dz * dz);
        }

        /// <summary>
        /// The distance between this and t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public double DistanceXY(Vertex t)
        {
            return (double)System.Math.Sqrt(DeltaSquaredXY(t));
        }

        /// <summary>
        /// The distance between this and t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public double Distance(Vertex t)
        {
            return (double)System.Math.Sqrt(DeltaSquared(t));
        }

        /// <summary>
        /// Is this rectangle within the region.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool InsideXY(RectangleF region)
        {
            if (X < region.Left) return false;
            if (X > region.Right) return false;
            if (Y < region.Top) return false;
            if (Y > region.Bottom) return false;
            return true;
        }

        /// <summary>
        /// To string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }

        public int CompareTo(Vertex other)
        {
            return string.Compare(this.id, other.id);
        }
    }
}
