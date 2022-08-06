using Delaunay;
using gg.Mesh;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SocketServer.HierarchicalClusteringUtils;
using SocketServer.KMeanCluster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    class Program
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 9999;

        public static MongoClient mgCon = new MongoClient("mongodb://localhost:27017");

        static ASCIIEncoding encoding = new ASCIIEncoding();

        static PhanTichBom ptb = new PhanTichBom();
        static PhanTichMin ptm = new PhanTichMin();

        static bool CheckCamCo(int aValue, out bool isButton1Press)
        {
            // Button 1 la diem cam co, button 2 la dung de do do sau (A An)
            isButton1Press = false;
            bool isButton1 = ((aValue & 8) > 0);   // khong thay doi
            bool isButton2 = ((aValue & 2) > 0);  // sua lai
            bool isButton3 = ((aValue & 4) > 0); // sua lai
            bool isButton4 = ((aValue & 1) > 0); // khong sua

            if (isButton1)
                isButton1Press = true;

            if (isButton1 || isButton2)
                return true;
            else
                return false;
        }
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            
            var server = new UdpListener();
            
            //var database2 = mgCon.GetDatabase("db_cecm");

            ////List<Vertex> lstInput = new List<Vertex>();
            //using (StreamReader r = new StreamReader(@"C:\temp\dataRPBM.json"))
            //{
            //    string json2 = r.ReadToEnd();
            //    DataRPBM data = JsonConvert.DeserializeObject<DataRPBM>(json2);
            //    foreach (MachineBomCodePoint machineBomPointDatas in data.MachineBomPointDatas)
            //    {
            //        foreach (var item in machineBomPointDatas.DatabasePointInfo)
            //        {
            //            //if (item.isMachineBom == "True")
            //            //{
            //            //    lstInput.Add(new Vertex(double.Parse(item.lat_value), double.Parse(item.long_value), double.Parse(item.the_value)));
            //            //    //Console.WriteLine("true");
            //            //}
            //            if (database2 != null)
            //            {
            //                var collection = database2.GetCollection<BsonDocument>("cecm_data");
            //                var document = new BsonDocument {
            //                    { "code", item.code },
            //                    { "project_id", "103" },
            //                    { "machineBomCode", item.machineBomCode },
            //                    { "lat_value", item.lat_value},
            //                    { "long_value", item.long_value},
            //                    { "the_value", item.the_value},
            //                    { "update_time", item.update_time},
            //                    { "time_action", item.time_action},
            //                    { "bit_sens", item.bit_sens},
            //                    { "isMachineBom", item.isMachineBom}
            //                };
            //                collection.InsertOne(document);
            //            }
            //        }
            //    }
            //}
            //start listening for messages and copy the messages back to the client
            Task.Factory.StartNew(async () => {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                while (true)
                {
                    try
                    {
                        List<Vertex> lstInputBomb = new List<Vertex>();
                        List<Vertex> lstInputMine = new List<Vertex>();
                        var received = await server.Receive();
                        Thread thread = new Thread(() =>
                        {

                            List<Vertex> lstCenter = new List<Vertex>();
                            //List<Vertex> lstInput = new List<Vertex>();
                            //using (StreamReader r = new StreamReader(@"C:\temp\vertexes.json"))
                            //{
                            //    string json2 = r.ReadToEnd();
                            //    lstInput = JsonConvert.DeserializeObject<List<Vertex>>(json2);
                            //}

                            if (received.Message.Trim() != "")
                            {
                                Console.WriteLine(received.Message);
                                Boundary boundary = JsonConvert.DeserializeObject<Boundary>(received.Message);
                                //ptbm.Set.Clear();
                                Console.WriteLine("BAT DAU PHAN TICH GOI:" + boundary.timeSent.ToString("dd/MM/yyyy HH:mm:ss"));
                                lstInputBomb.Clear();
                                lstCenter.Clear();
                                var database = mgCon.GetDatabase("db_cecm");
                                if (database != null)
                                {
                                    var collection = database.GetCollection<InfoConnect>("cecm_data");
                                    //var builder = Builders<InfoConnect>.Filter;
                                    //var filter = builder.And();
                                    //var filter1 = Builders<InfoConnect>.Filter
                                    //.Gt(x => double.Parse(x.lat_value), boundary.minLat);
                                    //var filter2 = Builders<InfoConnect>.Filter
                                    //.Lt(x => double.Parse(x.lat_value), boundary.maxLat);
                                    //var filter3 = Builders<InfoConnect>.Filter
                                    //.Gt(x => double.Parse(x.long_value), boundary.minLong);
                                    //var filter4 = Builders<InfoConnect>.Filter
                                    //.Lt(x => double.Parse(x.long_value), boundary.maxLong);
                                    //var filter5 = Builders<InfoConnect>.Filter
                                    //.Eq(x => x.isMachineBom, "True");
                                    //var filter = filter1 & filter2 & filter3 & filter4 & filter5;
                                    //var filter = Builders<InfoConnect>.Filter.Eq(x => double.Parse(x.lat_value), boundary.minLat);
                                    //collection.Find()
                                    var docs = collection.Find(
                                        doc => doc.lat_value > boundary.minLat &&
                                            doc.lat_value < boundary.maxLat &&
                                            doc.long_value > boundary.minLong &&
                                            doc.long_value > boundary.minLong).ToList();
                                    int count = 0;
                                    foreach (InfoConnect doc in docs)
                                    {
                                        count++;
                                        Vertex vertex = new Vertex();
                                        vertex.X = doc.lat_value;
                                        vertex.Y = doc.long_value;
                                        vertex.Z = doc.the_value;
                                        vertex.MachineCode = doc.code;
                                        vertex.BitSent = doc.bit_sens;
                                        bool isCamCo = CheckCamCo(vertex.BitSent, out bool isButton1Press);
                                        //if (
                                        //    vertex.X > boundary.minLat &&
                                        //    vertex.X < boundary.maxLat &&
                                        //    vertex.Y > boundary.minLong &&
                                        //    vertex.Y > boundary.minLong
                                        //    //doc.GetValue("isMachineBom").AsBoolean == false
                                        //)
                                        //{
                                            if (isCamCo)
                                            {
                                                vertex.Type = Vertex.CAMCO;
                                                lstCenter.Add(vertex);
                                            }
                                            if(doc.isMachineBom)
                                            {
                                                lstInputBomb.Add(vertex);
                                            }
                                            else
                                            {
                                                lstInputMine.Add(vertex);
                                            }
                                        //}
                                        //lstInput.Add(vertex);
                                    }
                                    if (lstCenter.Count > 0)
                                    {
                                        lstCenter = HierarchicalClustering.Cluster(lstCenter, 1, 0, Vertex.CAMCO);
                                    }
                                    //collection.InsertOne(document);
                                }
                                //List<Vertex> lstPolygonInput = JsonConvert.DeserializeObject<List<Vertex>>(received.Message);
                                ptb.Set = lstInputBomb;
                                ptm.Set = lstInputMine;
                                //ptbm.Set = lstPolygonInput;
                                float minxRec = 0;
                                float minyRec = 0;
                                float maxxRec = 0;
                                float maxyRec = 0;
                                if (boundary.minLat != -1)
                                {
                                    minxRec = (float)boundary.minLat;
                                }
                                if (boundary.minLong != -1)
                                {
                                    minyRec = (float)boundary.minLong;
                                }
                                if (boundary.maxLat != -1)
                                {
                                    maxxRec = (float)boundary.maxLat;
                                }
                                if (boundary.maxLong != -1)
                                {
                                    maxyRec = (float)boundary.maxLong;
                                }
                                //List<Vertex> lst_bom = ptb.phanTichBom(minxRec, minyRec, maxxRec, maxyRec, boundary.khoangPT);
                                //Console.WriteLine("lst_bom.Count: " + lst_bom.Count);
                                //List<Vertex> lst_min = ptm.phanTichMin(minxRec, minyRec, maxxRec, maxyRec, boundary.khoangPT);
                                //Console.WriteLine("lst_min.Count: " + lst_min.Count);
                                List<Vertex> lst_bom = ptb.phanTichBomNew(minxRec, minyRec, maxxRec, maxyRec, boundary.khoangPT, boundary.lstRanhDo, boundary.nguongBom);
                                Console.WriteLine("lst_bom.Count: " + lst_bom.Count);
                                if (lst_bom.Count > 0)
                                {
                                    lst_bom = HierarchicalClustering.Cluster(lst_bom, boundary.minClusterSize, Vertex.TYPE_BOMB);
                                }
                                List<Vertex> lst_min = ptm.phanTichMinNew(minxRec, minyRec, maxxRec, maxyRec, boundary.khoangPT, boundary.lstRanhDo, boundary.nguongMin);
                                Console.WriteLine("lst_min.Count: " + lst_min.Count);
                                if (lst_min.Count > 0)
                                {
                                    lst_min = HierarchicalClustering.Cluster(lst_min, boundary.minClusterSize, Vertex.TYPE_MINE);
                                }
                                List<Vertex> lstKQPT = new List<Vertex>();
                                lstKQPT.AddRange(lst_bom);
                                lstKQPT.AddRange(lst_min);

                                lstCenter.AddRange(lstKQPT);
                                lstCenter = HierarchicalClustering.ClusterAll(lstCenter);
                                Console.WriteLine("lstCenter.Count: " + lstCenter.Count);
                                string json = JsonConvert.SerializeObject(lstCenter);
                                server.Reply(json, received.Sender);
                                Console.WriteLine("KET THUC PHAN TICH GOI:" + boundary.timeSent.ToString("dd/MM/yyyy HH:mm:ss"));
                            }
                        });
                        thread.IsBackground = true;
                        CustomTimeout timeout = new CustomTimeout(() =>
                        {
                            thread.Abort();
                        }, 30000);
                        thread.Start();

                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                    }
                    
                    
                    //if (received.Message == "quit")
                    //    break;
                }
            });

            while (true) { }
        }
    }
}
