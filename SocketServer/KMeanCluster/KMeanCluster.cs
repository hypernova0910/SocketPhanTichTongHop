﻿using gg.Mesh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.KMeanCluster
{
    class KMeanCluster
    {
      //  public static int[] Cluster(List<Vertex> rawData, int numClusters)
      //  {
      //      List<Vertex> data = Normalized(rawData);
      //      bool changed = true; bool success = true;
      //      int[] clustering = InitClustering(data.Count, numClusters, 0);
      //      double[][] means = Allocate(numClusters, data[0].Length);
      //      int maxCount = data.Length * 10;
      //      int ct = 0;
      //      while (changed == true && success == true && ct < maxCount)
      //      {
      //          ++ct;
      //          success = UpdateMeans(data, clustering, means);
      //          changed = UpdateClustering(data, clustering, means);
      //      }
      //      return clustering;
      //  }

      //  private static List<Vertex> Normalized(List<Vertex> rawData)
      //  {
      //      //double[][] result = new double[rawData.Length][];
      //      //for (int i = 0; i < rawData.Count; ++i)
      //      //{
      //      //    //result[i] = new double[rawData[i].Length];
      //      //    Array.Copy(rawData[i], result[i], rawData[i].Length);
      //      //}
      //      List<Vertex> result = new List<Vertex>(rawData);

      //      //for (int j = 0; j < result[0].Count; ++j)
      //      //{
      //      //    double colSum = 0.0;
      //      //    for (int i = 0; i < result.Count; ++i)
      //      //        colSum += result[i][j];
      //      //    double mean = colSum / result.Length;
      //      //    double sum = 0.0;
      //      //    for (int i = 0; i < result.Length; ++i)
      //      //        sum += (result[i][j] - mean) * (result[i][j] - mean);
      //      //    double sd = sum / result.Length;
      //      //    for (int i = 0; i < result.Length; ++i)
      //      //        result[i][j] = (result[i][j] - mean) / sd;
      //      //}
      //      double colSum = 0.0;
      //      for (int i = 0; i < result.Count; ++i)
      //          colSum += result[i].X;
      //      double mean = colSum / result.Count;
      //      double sum = 0.0;
      //      for (int i = 0; i < result.Count; ++i)
      //          sum += (result[i].X - mean) * (result[i].X - mean);
      //      double sd = sum / result.Count;
      //      for (int i = 0; i < result.Count; ++i)
      //          result[i].X = (result[i].X - mean) / sd;

      //      double colsumYY = 0.0;
      //      for (int i = 0; i < result.Count; ++i)
      //          colsumYY += result[i].Y;
      //      double meanY = colsumYY / result.Count;
      //      double sumY = 0.0;
      //      for (int i = 0; i < result.Count; ++i)
      //          sumY += (result[i].Y - meanY) * (result[i].Y - meanY);
      //      double sdY = sumY / result.Count;
      //      for (int i = 0; i < result.Count; ++i)
      //          result[i].Y = (result[i].Y - meanY) / sdY;

      //      return result;
      //  }
      //  private static int[] InitClustering(int numTuples, int numClusters, int seed)
      //  {
      //      Random random = new Random(seed);
      //      int[] clustering = new int[numTuples];
      //      for (int i = 0; i < numClusters; ++i)
      //          clustering[i] = i;
      //      for (int i = numClusters; i < clustering.Length; ++i)
      //          clustering[i] = random.Next(0, numClusters);
      //      return clustering;
      //  }
      //  private static double[][] Allocate(int numClusters, int numColumns)
      //  {
      //      double[][] result = new double[numClusters][];
      //      for (int k = 0; k < numClusters; ++k)
      //          result[k] = new double[numColumns];
      //      return result;
      //  }
      //  private static bool UpdateMeans(double[][] data, int[] clustering, double[][] means)
      //  {
      //      int numClusters = means.Length;
      //      int[] clusterCounts = new int[numClusters];
      //      for (int i = 0; i < data.Length; ++i)
      //      {
      //          int cluster = clustering[i];
      //          ++clusterCounts[cluster];
      //      }

      //      for (int k = 0; k < numClusters; ++k)
      //          if (clusterCounts[k] == 0)
      //              return false;

      //      for (int k = 0; k < means.Length; ++k)
      //          for (int j = 0; j < means[k].Length; ++j)
      //              means[k][j] = 0.0;

      //      for (int i = 0; i < data.Length; ++i)
      //      {
      //          int cluster = clustering[i];
      //          for (int j = 0; j < data[i].Length; ++j)
      //              means[cluster][j] += data[i][j]; // accumulate sum
      //      }

      //      for (int k = 0; k < means.Length; ++k)
      //          for (int j = 0; j < means[k].Length; ++j)
      //              means[k][j] /= clusterCounts[k]; // danger of div by 0
      //      return true;
      //  }

      //  private static bool UpdateClustering(double[][] data, int[] clustering, double[][] means)
      //  {
      //      int numClusters = means.Length;
      //      bool changed = false;

      //      int[] newClustering = new int[clustering.Length];
      //      Array.Copy(clustering, newClustering, clustering.Length);

      //      double[] distances = new double[numClusters];

      //      for (int i = 0; i < data.Length; ++i)
      //      {
      //          for (int k = 0; k < numClusters; ++k)
      //              distances[k] = Distance(data[i], means[k]);

      //          int newClusterID = MinIndex(distances);
      //          if (newClusterID != newClustering[i])
      //          {
      //              changed = true;
      //              newClustering[i] = newClusterID;
      //          }
      //      }

      //      if (changed == false)
      //          return false;

      //      int[] clusterCounts = new int[numClusters];
      //      for (int i = 0; i < data.Length; ++i)
      //      {
      //          int cluster = newClustering[i];
      //          ++clusterCounts[cluster];
      //      }

      //      for (int k = 0; k < numClusters; ++k)
      //          if (clusterCounts[k] == 0)
      //              return false;

      //      Array.Copy(newClustering, clustering, newClustering.Length);
      //      return true; // no zero-counts and at least one change
      //  }
      //  private static double Distance(double[] tuple, double[] mean)
      //  {
      //      double sumSquaredDiffs = 0.0;
      //      for (int j = 0; j < tuple.Length; ++j)
      //          sumSquaredDiffs += Math.Pow((tuple[j] - mean[j]), 2);
      //      return Math.Sqrt(sumSquaredDiffs);
      //  }
      //  private static int MinIndex(double[] distances)
      //  {
      //      int indexOfMin = 0;
      //      double smallDist = distances[0];
      //      for (int k = 0; k < distances.Length; ++k)
      //      {
      //          if (distances[k] < smallDist)
      //          {
      //              smallDist = distances[k];
      //              indexOfMin = k;
      //          }
      //      }
      //      return indexOfMin;
      //  }


      //  static void ShowData(double[][] data, int decimals,
      //bool indices, bool newLine)
      //  {
      //      for (int i = 0; i < data.Length; ++i)
      //      {
      //          if (indices) Console.Write(i.ToString().PadLeft(3) + " ");
      //          for (int j = 0; j < data[i].Length; ++j)
      //          {
      //              if (data[i][j] >= 0.0) Console.Write(" ");
      //              Console.Write(data[i][j].ToString("F" + decimals) + " ");
      //          }
      //          Console.WriteLine("");
      //      }
      //      if (newLine) Console.WriteLine("");
      //  }
      //  static void ShowVector(int[] vector, bool newLine)
      //  {
      //      for (int i = 0; i < vector.Length; ++i)
      //          Console.Write(vector[i] + " ");
      //      if (newLine) Console.WriteLine("\n");
      //  }
      //  static void ShowClustered(double[][] data, int[] clustering,
      //int numClusters, int decimals)
      //  {
      //      for (int k = 0; k < numClusters; ++k)
      //      {
      //          Console.WriteLine("===================");
      //          for (int i = 0; i < data.Length; ++i)
      //          {
      //              int clusterID = clustering[i];
      //              if (clusterID != k) continue;
      //              Console.Write(i.ToString().PadLeft(3) + " ");
      //              for (int j = 0; j < data[i].Length; ++j)
      //              {
      //                  if (data[i][j] >= 0.0) Console.Write(" ");
      //                  Console.Write(data[i][j].ToString("F" + decimals) + " ");
      //              }
      //              Console.WriteLine("");
      //          }
      //          Console.WriteLine("===================");
      //      } // k
      //  }
    }
}
