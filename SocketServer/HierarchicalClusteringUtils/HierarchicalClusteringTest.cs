using Aglomera;
using Aglomera.Linkage;
using gg.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.HierarchicalClusteringUtils
{
    class HierarchicalClusteringTest
    {
        private const double Dissimilarity_CompleteLinkage = 4;

        public static List<Vertex> Cluster(List<Vertex> vertices, int typeBombMine = 0)
        {
            Console.WriteLine("=========================== HierarchicalClustering ===========================");
            List<Vertex> result = new List<Vertex>();

            var metric = new DissimilarityMetric();
            var linkage = new AverageLinkage<Vertex>(metric);
            var algorithm = new AgglomerativeClusteringAlgorithm<Vertex>(linkage);
            ClusteringResult<Vertex> clusteringResult = algorithm.GetClustering(vertices.ToHashSet());

            foreach (ClusterSet<Vertex> clusterSet in clusteringResult)
            {
                Console.WriteLine("clusterSet.Dissimilarity: " + clusterSet.Dissimilarity);
                int count = 0;
                foreach (Cluster<Vertex> cluster in clusterSet)
                {
                    count++;
                    Console.WriteLine("Cluster " + count);
                    Console.WriteLine("cluster.Dissimilarity: " + cluster.Dissimilarity);
                    foreach (Vertex dataPoint in cluster)
                    {
                        Console.WriteLine(dataPoint.id + "\tX: " + dataPoint.X + "\tY: " + dataPoint.Y);
                    }
                }


                Console.WriteLine("");
            }
            Console.WriteLine("=========================== End HierarchicalClustering ===========================");
            return result;
        }
    }
}
