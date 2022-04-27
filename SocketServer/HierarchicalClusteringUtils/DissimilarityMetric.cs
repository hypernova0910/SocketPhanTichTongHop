using Aglomera;
using gg.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.HierarchicalClusteringUtils
{
    class DissimilarityMetric : IDissimilarityMetric<Vertex>
    {
        public double Calculate(Vertex instance1, Vertex instance2)
        {
            return Math.Sqrt(Math.Pow(instance1.X - instance2.X, 2) + Math.Pow(instance1.Y - instance2.Y, 2));
        }
    }
}
