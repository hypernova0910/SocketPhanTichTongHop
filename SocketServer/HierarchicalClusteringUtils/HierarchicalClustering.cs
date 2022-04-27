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
    class HierarchicalClustering
    {
        private const double Dissimilarity_CompleteLinkage = 4;

        private static Vertex GetCentroid(List<Vertex> poly)
        {
            double tongX = 0;
            double tongY = 0;
            double z = 0;
            foreach (var item in poly)
            {
                tongX += item.X;
                tongY += item.Y;
                z = item.Z;
            }
            return new Vertex(tongX / poly.Count, tongY / poly.Count, z, Vertex.TYPE_BOMB);
        }


        public static List<Vertex> Cluster(List<Vertex> vertices, int typeBombMine = 0)
        {
            Console.WriteLine("=========================== HierarchicalClustering ===========================");
            List<Vertex> result = new List<Vertex>();
            
            var metric = new DissimilarityMetric();
            var linkage = new CompleteLinkage<Vertex>(metric);
            var algorithm = new AgglomerativeClusteringAlgorithm<Vertex>(linkage);
            ClusteringResult<Vertex> clusteringResult = algorithm.GetClustering(vertices.ToHashSet());

            if(clusteringResult.Count > 0)
            {
                ClusterSet<Vertex> clusterSet = clusteringResult[0];
                for (int i = 0; i < clusteringResult.Count; i++)
                //foreach (ClusterSet<Vertex> clusterSet in clusteringResult)
                {
                    //int count = 0;
                    if (clusteringResult[i].Dissimilarity > Dissimilarity_CompleteLinkage)
                    {
                        
                        break;
                    }
                    else
                    {
                        clusterSet = clusteringResult[i];
                    }
                    Console.WriteLine("clusterSet.Dissimilarity: " + clusterSet.Dissimilarity);
                    Console.WriteLine("");
                }
                foreach (Cluster<Vertex> cluster in clusterSet)
                {
                    Vertex vertex = GetCentroid(cluster.ToList());
                    vertex.TypeBombMine = typeBombMine;
                    result.Add(vertex);
                }
            }
            
            Console.WriteLine("=========================== End HierarchicalClustering ===========================");
            return result;
        }
    }
}
