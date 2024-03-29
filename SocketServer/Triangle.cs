﻿///////////////////////////////////////////////////////////////////////////////
//
//  Triangle.cs
//
//  By Philip R. Braica (HoshiKata@aol.com, VeryMadSci@gmail.com)
//
//  Distributed under the The Code Project Open License (CPOL)
//  http://www.codeproject.com/info/cpol10.aspx
///////////////////////////////////////////////////////////////////////////////

namespace gg.Mesh
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Triangle class.
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// Reset the index, indexing gives each triangle an ID
        /// unique since construction of the mesh.
        /// </summary>
        public static void ResetIndex() {m_index = 0;}

        #region Construcors: (), (Triangle src), (Vertex a, Vertex b, Vertex c)
        /// <summary>
        /// Constructor.
        /// </summary>
        public Triangle() 
        {
            Index = m_index;
            m_index++;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="src"></param>
        public Triangle(Triangle src)
        {
            A = src.A;
            B = src.B;
            C = src.C;
            AB = src.AB;
            BC = src.BC;
            CA = src.CA;
            Index = m_index;
            m_index++;
        }

        /// <summary>
        /// Constructor by vertex.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public Triangle(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;
            Index = m_index;
            m_index++;
        }
        #endregion

        #region Protected data.

        // Vertexes.
        protected Vertex m_a = null;
        protected Vertex m_b = null;
        protected Vertex m_c = null;
        
        // Lengths.
        protected double m_abLen = 0;
        protected double m_bcLen = 0;
        protected double m_caLen = 0;
        protected bool m_abLenCalcd = false;
        protected bool m_bcLenCalcd = false;
        protected bool m_caLenCalcd = false;

        // Side determinations.
        protected bool m_abDet = false;
        protected bool m_bcDet = false;
        protected bool m_caDet = false;
        protected bool m_abDetCalcd = false;
        protected bool m_bcDetCalcd = false;
        protected bool m_caDetCalcd = false;

        /// <summary>
        /// Index of this triangle for debug.
        /// </summary>
        protected static int m_index = 0;

        // Sides
        protected Triangle m_ab = null;
        protected Triangle m_bc = null;
        protected Triangle m_ca = null;

        // Center
        protected bool m_centerComputed = false;
        protected Vertex m_center = null;
        
        #endregion

        /// <summary>
        /// Index.
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// Which search region it is in.
        /// </summary>
        public int RegionCode { get; set; }

        /// <summary>
        /// To string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Index + "   A(" + A.ToString() + ") => B(" + B.ToString() + ") => C(" + C.ToString() + ")";
        }

        /// <summary>
        /// Compute the center
        /// </summary>
        public Vertex Center
        {
            get
            {
                if (m_centerComputed) return m_center;
                m_center = new Vertex(
                    (float)(A.X + B.X + C.X) / 3f,
                    (float)(A.Y + B.Y + C.Y) / 3f,
                    (float)(A.Z + B.Z + C.Z) / 3f);

                double delta = m_center.DeltaSquaredXY(A);
                double tmp = m_center.DeltaSquaredXY(B);
                delta = delta > tmp ? delta : tmp;
                tmp = m_center.DeltaSquaredXY(C);
                delta = delta > tmp ? delta : tmp;
                FarthestFromCenter = delta;
                m_centerComputed = true;

                return m_center;
            }
        }

        /// <summary>
        /// Farthest distance a point is from center is distance squared.
        /// </summary>
        public double FarthestFromCenter { get; protected set; }

        /// <summary>
        /// Vertex A
        /// </summary>
        public Vertex A
        {
            get { return m_a; }
            set 
            {
                if (m_a == value) return;
                m_abDetCalcd = false;
                m_caDetCalcd = false;
                m_abLenCalcd = false;
                m_caLenCalcd = false;
                m_centerComputed = false;
                m_a = value;
            }
        }

        /// <summary>
        /// Vertex B
        /// </summary>
        public Vertex B
        {
            get { return m_b; }
            set
            {
                if (m_b == value) return;
                m_abDetCalcd = false;
                m_bcDetCalcd = false;
                m_abLenCalcd = false;
                m_bcLenCalcd = false;
                m_centerComputed = false;
                m_b = value;
            }
        }

        /// <summary>
        /// Vertex C
        /// </summary>
        public Vertex C
        {
            get { return m_c; }
            set
            {
                if (m_c == value) return;
                m_caDetCalcd = false;
                m_bcDetCalcd = false;
                m_caLenCalcd = false;
                m_bcLenCalcd = false;
                m_centerComputed = false;
                m_c = value;
            }
        }

        /// <summary>
        /// Triangle AB shares side AB.
        /// </summary>
        public Triangle AB { get { return m_ab; } set { m_ab = value; } }

        /// <summary>
        /// Triangle BC shares side BC.
        /// </summary>
        public Triangle BC { get { return m_bc; } set { m_bc = value; } }

        /// <summary>
        /// Triangle CA shares side CA.
        /// </summary>
        public Triangle CA { get { return m_ca; } set { m_ca = value; } }


        /// <summary>
        /// AB det.
        /// </summary>
        protected bool abDet
        {
            get
            {
                if (!m_abDetCalcd)
                {
                    m_abDet = vertexTest(A, B, C);
                }
                return m_abDet;
            }
        }

        /// <summary>
        /// BC det.
        /// </summary>
        protected bool bcDet
        {
            get
            {
                if (!m_bcDetCalcd)
                {
                    m_bcDet = vertexTest(B, C, A);
                }
                return m_bcDet;
            }
        }

        /// <summary>
        /// CA det.
        /// </summary>
        protected bool caDet
        {
            get
            {
                if (!m_caDetCalcd)
                {
                    m_caDet = vertexTest(C, A, B);
                }
                return m_caDet;
            }
        }

        /// <summary>
        /// Vertex sidedness test.
        /// </summary>
        /// <param name="la"></param>
        /// <param name="lb"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        protected bool vertexTest(Vertex la, Vertex lb, Vertex t)
        {
            // y = mx + b
            if (la.X == lb.X)
            {
                // Vertical at X.
                return t.X > la.X;
            }
            if (la.Y == lb.Y)
            {
                return t.Y > la.Y;
            }
            double m = (la.Y - lb.Y)/(la.X - lb.X);
            double b = la.Y - (m * la.X);
            return (m * t.X + b - t.Y) > 0;
        }

        /// <summary>
        /// Does this contain t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Contains(Vertex t)
        {
            double delta = t.DeltaSquaredXY(Center);
            if (delta > FarthestFromCenter) return false;
            if (abDet != vertexTest(A, B, t)) return false;
            if (bcDet != vertexTest(B, C, t)) return false;
            if (caDet != vertexTest(C, A, t)) return false;
            return true;
        }

        /// <summary>
        /// Length of AB, cached and lazy calculated.
        /// </summary>
        public double AB_Length
        {
            get
            {
                if (m_abLenCalcd == true)
                {
                    return m_abLen;
                }
                if ((A == null) || (B == null)) return -1;
                m_abLen = A.DeltaSquaredXY(B);
                m_abLenCalcd = true;
                return m_abLen;
            }
        }

        /// <summary>
        /// Length of BC, cached and lazy calculated.
        /// </summary>
        public double BC_Length
        {
            get
            {
                if (m_bcLenCalcd == true)
                {
                    return m_bcLen;
                }
                if ((B == null) || (C == null)) return -1;
                m_bcLen = B.DeltaSquaredXY(C);
                m_bcLenCalcd = true;
                return m_bcLen;
            }
        }

        /// <summary>
        /// Length of CA, cached and lazy calculated.
        /// </summary>
        public double CA_Length
        {
            get
            {
                if (m_caLenCalcd == true)
                {
                    return m_caLen;
                }
                if ((C == null) || (A == null)) return -1;
                m_caLen = C.DeltaSquaredXY(A);
                m_caLenCalcd = true;
                return m_caLen;
            }
        }

        /// <summary>
        /// Area of the triangle.
        /// </summary>
        public double Area
        {
            get
            {
                double a = AB_Length;
                double b = BC_Length;
                double c = CA_Length;
                a = (double)System.Math.Sqrt(a);
                b = (double)System.Math.Sqrt(b);
                c = (double)System.Math.Sqrt(c);

                // Herons formula.
                double s = 0.5f * (a + b + c);
                return (double)System.Math.Sqrt(s * (s - a) * (s - b) * (s - c));
            }
        }

        /// <summary>
        /// Return the indexed edge length;
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double Edge_Length(int i)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            return i == 0 ? AB_Length : i == 1 ? BC_Length : CA_Length;
        }

        /// <summary>
        /// Return the oposite of the edge.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vertex OpositeOfEdge(int i)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            return i == 0 ? C : i == 1 ? A : B;
        }
        
        /// <summary>
        /// Set the vertex by index.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="v"></param>
        public void SetVertex(int i, Vertex v)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            if (i == 0) A = v;
            if (i == 1) B = v;
            if (i == 2) C = v;
        }

        /// <summary>
        /// Get the cosine angle associated with a vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double VertexCosineAngle(int i)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            double dx1 = 0;
            double dx2 = 0;
            double dy1 = 0; 
            double dy2 = 0;
            if (i == 0)
            {
                dx1 = B.X - A.X;
                dy1 = B.Y - A.Y;
                dx2 = C.X - A.X;
                dy2 = C.Y - A.Y;
            }
            else
            {
                if (i == 1)
                {
                    dx1 = C.X - B.X;
                    dy1 = C.Y - B.Y;
                    dx2 = A.X - B.X;
                    dy2 = A.Y - B.Y;
                }
                else
                {
                    dx1 = A.X - C.X;
                    dy1 = A.Y - C.Y;
                    dx2 = B.X - C.X;
                    dy2 = B.Y - C.Y;
                }
            }
            double mag1 = (dx1 * dx1) + (dy1 * dy1);
            double mag2 = (dx2 * dx2) + (dy2 * dy2);
            double mag = (double)System.Math.Sqrt(mag1 * mag2);
            double dot = (double)((dx1 * dx2) + (dy1 * dy2)) / mag;

            // dot is 0 to 1 result of the cosine.
            return dot;
        }

        
        /// <summary>
        /// Get the angle of a vertex in radians.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double VertexAngleRadians(int i)
        {
            return (double)System.Math.Acos(VertexCosineAngle(i));
        }

        /// <summary>
        /// Is this rectangle within the region.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool Inside(System.Drawing.RectangleF region)
        {
            if (!A.InsideXY(region)) return false;
            if (!B.InsideXY(region)) return false;
            if (!C.InsideXY(region)) return false;
            return true;
        }

        /// <summary>
        /// Repair any Edge links, both ways.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public void RepairEdges(Triangle a)
        {
            // Check if a.AB is in this.
            if (this.Index == a.Index) return;
            if (bothIn(a, a.A, a.B)) { a.AB = this; return; }
            if (bothIn(a, a.B, a.C)) { a.BC = this; return; }
            if (bothIn(a, a.C, a.A)) { a.CA = this; return; }    
        }

        /// <summary>
        /// Are both vertexes in?
        /// </summary>
        /// <param name="t"></param>
        /// <param name="vt"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected bool bothIn(Triangle t, Vertex a, Vertex b)
        {
            if (a == A) 
            {
                if (b == B) { AB = t; return true; }
                if (b == C) { CA = t; return true; }
            }
            if (a == B)
            {
                if (b == A) { AB = t; return true; }
                if (b == C) { BC = t; return true; }
            }
            if (a == C)
            {
                if (b == A) { CA = t; return true; }
                if (b == B) { BC = t; return true; }
            }
            return false;
        }

        /// <summary>
        /// Vertex.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vertex GetVertex(int i)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            if (i == 0) return A;
            if (i == 1) return B;
            return C;
        }

        /// <summary>
        /// Set the edge by index.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="t"></param>
        public void SetEdge(int i, Triangle t)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            if (i == 0) AB = t;
            if (i == 1) BC = t;
            if (i == 2) CA = t;
        }

        /// <summary>
        /// Get the indexed edge.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Triangle Edge(int i)
        {
            return i == 0 ? AB : i == 1 ? BC : CA;
        }
    }
}
