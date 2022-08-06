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
        private const double Dissimilarity_All = 0.5;

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


        public static List<Vertex> Cluster(List<Vertex> vertices, int minClusterSize, int typeBombMine = 0, int typeBomCamCo = Vertex.BOM)
        {
            Console.WriteLine("=========================== HierarchicalClustering ===========================");
            Console.WriteLine("minClusterSize: " + minClusterSize);
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
                    if(cluster.Count >= minClusterSize)
                    {
                        Vertex vertex = GetCentroid(cluster.ToList());
                        vertex.TypeBombMine = typeBombMine;
                        vertex.Type = typeBomCamCo;
                        vertex.MachineCode = cluster.ToList()[0].MachineCode;
                        result.Add(vertex);
                    }
                }
            }
            
            Console.WriteLine("=========================== End HierarchicalClustering ===========================");
            return result;
        }

        public static List<Vertex> ClusterAll(List<Vertex> vertices)
        {
            //Console.WriteLine("=========================== HierarchicalClustering ===========================");
            List<Vertex> result = new List<Vertex>();

            var metric = new DissimilarityMetric();
            var linkage = new CompleteLinkage<Vertex>(metric);
            var algorithm = new AgglomerativeClusteringAlgorithm<Vertex>(linkage);
            ClusteringResult<Vertex> clusteringResult = algorithm.GetClustering(vertices.ToHashSet());

            if (clusteringResult.Count > 0)
            {
                ClusterSet<Vertex> clusterSet = clusteringResult[0];
                for (int i = 0; i < clusteringResult.Count; i++)
                //foreach (ClusterSet<Vertex> clusterSet in clusteringResult)
                {
                    //int count = 0;
                    if (clusteringResult[i].Dissimilarity > Dissimilarity_All)
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
                    bool isSuspectPoint = false;
                    bool isFlag = false;
                    bool isBomb = false;
                    bool isMine = false;
                    string machineCode = "";
                    foreach(Vertex v in cluster.ToList())
                    {
                        if (!string.IsNullOrEmpty(v.MachineCode))
                        {
                            machineCode = v.MachineCode;
                        }
                        if(v.Type == Vertex.BOM)
                        {
                            isSuspectPoint = true;
                        }
                        if(v.Type == Vertex.CAMCO)
                        {
                            isFlag = true;
                        }
                        if(v.TypeBombMine == Vertex.TYPE_BOMB)
                        {
                            isBomb = true;
                        }
                        if(v.TypeBombMine == Vertex.TYPE_MINE)
                        {
                            isMine = true;
                        }

                    }
                    vertex.MachineCode = machineCode;
                    if (isSuspectPoint && isFlag)
                    {
                        vertex.Type = Vertex.BOM_CAMCO;
                    }
                    else
                    {
                        if (isSuspectPoint)
                        {
                            vertex.Type = Vertex.BOM;
                        }
                        else if(isFlag)
                        {
                            vertex.Type = Vertex.CAMCO;
                        }
                    }
                    if (isBomb && isMine)
                    {
                        vertex.TypeBombMine = Vertex.TYPE_BOMB_MINE;
                    }
                    else
                    {
                        if (isBomb)
                        {
                            vertex.TypeBombMine = Vertex.TYPE_BOMB;
                        }
                        else if (isMine)
                        {
                            vertex.TypeBombMine = Vertex.TYPE_MINE;
                        }
                    }
                    result.Add(vertex);
                }
            }

            Console.WriteLine("=========================== End HierarchicalClustering ===========================");
            return result;
        }
    }
}
