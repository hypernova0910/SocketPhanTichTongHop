using gg.Mesh;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Delaunay
{
    class PhanTichMin
    {
        public List<Vertex> Set;

        //SỐ LƯỢNG ĐIỂM TỐI THIỂU PHÂN TÍCH
        public static int MIN_POINT = 200;
        //KHOẢNG GIÁ TRỊ TỪ TRƯỜNG NHỎ NHẤT
        public static Double Z_MIN = 0;
        //KHOẢNG GIÁ TRỊ TỪ TRƯỜNG LỚN NHẤT
        public static Double Z_MAX = 12;
        //KHOẢNG GIÁ TRỊ TỪ TRƯỜNG CHÊNH LỆCH MAX MIN
        public static Double INTERVAL_MAX_MIN = 0.1;
        //THỜI GIAN TỐI ĐA ĐỂ PHÂN TÍHC
        public static double MIN_TIME = 15;

        public PhanTichMin()
        {
            Set = new List<Vertex>();
        }
        public List<Vertex> phanTichBomMin(float minxRec, float minyRec, float maxxRec, float maxyRec, int khoangPT)
        {
            DateTime start_time = DateTime.Now;
            Console.WriteLine("khoangPT: " + khoangPT);
            //List<double> lstX = new List<double>();
            //List<double> lstY = new List<double>();
            //foreach (var vn in lstPolygonInput)
            //{
            //    lstX.Add(vn.X);
            //    lstY.Add(vn.Y);
            //}
            //lstX.Sort();
            //lstY.Sort();
            Console.WriteLine("Trước lọc mine: " + Set.Count);
            if (Set != null)
            {
                Set = Set.Where((vn) => vn.Z <= Z_MAX && vn.Z >= Z_MIN).ToList();
                if (Set != null && Set.Count < MIN_POINT)
                {
                    return new List<Vertex>();
                }
            }
            else
            {
                return new List<Vertex>();
            }
            //Thread.Sleep(20000);
            Console.WriteLine("Sau lọc mine: " + Set.Count);
            //if ((Set != null && Set.Count < 50) || Set == null)
            //{
            //    return new List<Vertex>();
            //}
            var delta = 0.5;
            //Sắp xếp list độ sâu lấy minZ, maxZ. minZ = lst[0], maxZ = lst[count-1]
            List<Double> lstDoSau = new List<Double>();

            foreach (var vn in Set)
            {
                //Xác định các điểm có Z trong khoảng [-0.5, 0.5] thì đặt z = 0
                //if (vn.Z >= -1 * Math.Abs(delta) && vn.Z <= Math.Abs(delta))
                //{
                //    vn.Z = 0;
                //}
                lstDoSau.Add(vn.Z);
            }
            lstDoSau.Sort();
            //Độ cao trung bình ở giữa làm mốc
            double mediumZ = 0;
            if (lstDoSau != null && lstDoSau.Count > 0)
            {
                mediumZ = (lstDoSau[0] + lstDoSau[lstDoSau.Count - 1]) / 2;
            }
            //foreach(var x in Set)
            //{
            //    x.Z = x.Z - mediumZ;
            //}

            ////Console.WriteLine("Số Điểm  " + Set.Count);
            ////Console.WriteLine("Toa do X " + (float)(lstX[0]) + "/" + (float)(lstX[lstX.Count - 1] - lstX[0] + 100));
            ////Console.WriteLine("Toa do Y " + (float)(lstY[0]) + "/" + (float)(lstY[lstY.Count - 1] - lstY[0] + 100));
            ////Console.WriteLine("ĐỘ SÂU MAX  " + lstDoSau[0]);
            ////Console.WriteLine("ĐỘ SÂU MIN  " + lstDoSau[lstDoSau.Count - 1]);
            ////Console.WriteLine("DỊCH TỌA ĐỘ  " + mediumZ);

            System.DateTime start = System.DateTime.Now;
            //Phân tích ra tam giác
            Mesh m = new Mesh();
            //m.Recursion = (int)numericUpDown3.Value;
            //m.Compute(Set, new RectangleF((float)(lstX[0] - 1000), (float)(lstY[0] - 1000), (float)(lstX[lstX.Count - 1] - lstX[0] + 1000), (float)(lstY[lstY.Count - 1] - lstY[0] + 1000)));
            m.Compute(Set, new RectangleF((float)minxRec, (float)minyRec, (float)maxxRec - (float)minxRec, (float)maxyRec - (float)minyRec));
            //List tam giác surface
            List<Triangle> lstTriangleAfter = m.Facets;
            Console.WriteLine("Số tam giác = " + lstTriangleAfter.Count);
            //MessageBox.Show("SỐ TAM GIÁC =   " + lstTriangleAfter.Count);
            //Lấy list độ sâu để chia
            ////Console.WriteLine("\r\n Thời gian xây dựng bề mặt = " + System.DateTime.Now.Subtract(start).TotalMilliseconds.ToString() + " msec");
            //timeBeMat = System.DateTime.Now.Subtract(start).TotalMilliseconds.ToString() + " msec";
            //Double minZ = -7.03785228728182;
            //Double maxZ = 6.284435987475;
            //Double deltaZ = maxZ - minZ;
            //Double distanceZ = deltaZ / 3;

            Double minZ = lstDoSau[0];
            Double maxZ = lstDoSau[lstDoSau.Count - 1];
            Double deltaZ = maxZ - minZ;
            //Console.WriteLine("MaxZ = " + maxZ + " / minZ = " + minZ);
            if (deltaZ < INTERVAL_MAX_MIN)
            {
                return new List<Vertex>();
            }

            Double distanceZ = deltaZ / khoangPT;
            //Khoảng 1: [minZ, minZ + distanceZ]
            //Khoảng 2: [minZ + distanceZ, maxZ - distanceZ] ~ Vùng an toàn
            //Khoảng 3: [maxZ - distanceZ, maxZ            ]
            Double contourLineZ1 = (minZ + minZ + distanceZ) / 2;
            Double contourLineZ2 = (maxZ - distanceZ + maxZ) / 2;
            //Console.WriteLine("MaxZ = " + maxZ + " / minZ = " + minZ);
            Console.WriteLine("contourLineZ1 = " + contourLineZ1 + " / contourLineZ2 = " + contourLineZ2);
            //Double contourLineZ1 = minZ + distanceZ;
            //Double contourLineZ2 = maxZ - distanceZ;

            ////////////////////Tam giác liền kề////////////////////////////////
            ///List polygon vùng 1
            List<List<Vertex>> lstPolygonArea1 = new List<List<Vertex>>();
            //List polygon vùng 2
            List<List<Vertex>> lstPolygonArea2 = new List<List<Vertex>>();

            //Lấy các tam giác giao với vùng 1 và vùng 3
            List<Triangle> lstTriangleIntersecArea1 = new List<Triangle>();
            List<Triangle> lstTriangleIntersecArea2 = new List<Triangle>();
            foreach (var triangleN in lstTriangleAfter)
            {
                List<Double> lstZTriangle = new List<Double>();
                lstZTriangle.Add(triangleN.A.Z);
                lstZTriangle.Add(triangleN.B.Z);
                lstZTriangle.Add(triangleN.C.Z);
                lstZTriangle.Sort();
                Double maxZTriangle = lstZTriangle[2];
                Double minZTriangle = lstZTriangle[0];
                //Kiểm tra xem countour có đi qua tam giác này không
                if (minZTriangle <= contourLineZ1 && maxZTriangle >= contourLineZ1)
                {
                    //MP "z=contourLineZ1" cắt tam giác
                    lstTriangleIntersecArea1.Add(triangleN);
                }
                if (minZTriangle <= contourLineZ2 && maxZTriangle >= contourLineZ2)
                {
                    //MP "z=contourLineZ2" cắt tam giác
                    lstTriangleIntersecArea2.Add(triangleN);
                }
            }

            System.DateTime start1 = System.DateTime.Now;
            List<Triangle> exitsTrianglePolygon = new List<Triangle>();
            if (lstTriangleIntersecArea1 != null && lstTriangleIntersecArea1.Count > 0)
            {
                ////Console.WriteLine("BẮT ĐẦU TÌM POLYGON MẶT 1");
                //Tìm các vùng polygon 1 tương ứng
                for (int i = 0; i < lstTriangleIntersecArea1.Count; i++)
                {
                    //if(i<lstTriangleIntersecArea1.Count)
                    bool checkPolygon = false;
                    foreach (var itemtriangleExits in exitsTrianglePolygon)
                    {
                        if (itemtriangleExits.Index == lstTriangleIntersecArea1[i].Index)
                        {
                            checkPolygon = true;
                            break;
                        }
                    }
                    if (checkPolygon == false)
                    {
                        Triangle startTriangle = new Triangle();
                        Triangle flagTriangle = new Triangle();
                        Vertex startVertex = new Vertex();
                        Vertex flagVertex = new Vertex();
                        List<Vertex> lstPolygonSub1 = new List<Vertex>();

                        List<Triangle> triangleTemp = lstTriangleIntersecArea1;
                        startTriangle = lstTriangleIntersecArea1[i];
                        flagTriangle = findPolygonTriangle(startTriangle, contourLineZ1, lstTriangleIntersecArea1, ref lstPolygonSub1);

                        triangleTemp = lstTriangleIntersecArea1;
                        //DateTime start_time = DateTime.Now;
                        while (flagTriangle.Index != startTriangle.Index)
                        {
                            triangleTemp = lstTriangleIntersecArea1;
                            triangleTemp = removeTriagleFromList(triangleTemp, flagTriangle);
                            flagTriangle = findPolygonTriangle(flagTriangle, contourLineZ1, triangleTemp, ref lstPolygonSub1);
                            exitsTrianglePolygon.Add(flagTriangle);
                            if (DateTime.Now.Subtract(start_time).TotalSeconds > MIN_TIME)
                            {
                                Console.WriteLine("Overtime");
                                break;
                            }
                        }
                        if (DateTime.Now.Subtract(start_time).TotalSeconds > MIN_TIME)
                        {
                            Console.WriteLine("Overtime");
                            break;
                        }
                        lstPolygonArea1.Add(lstPolygonSub1);
                    }
                }
            }
            ////Console.WriteLine("Số polygon giao mặt 1 = " + lstPolygonArea1.Count);
            ////////////////////////////////////////////////////


            //Tìm các vùng polygon 2 tương ứng
            exitsTrianglePolygon = new List<Triangle>();
            if (lstTriangleIntersecArea2 != null && lstTriangleIntersecArea2.Count > 0)
            {
                //Console.WriteLine("BẮT ĐẦU TÌM POLYGON MẶT 2");
                for (int i = 0; i < lstTriangleIntersecArea2.Count; i++)
                {
                    bool checkPolygon = false;
                    foreach (var itemtriangleExits in exitsTrianglePolygon)
                    {
                        if (itemtriangleExits.Index == lstTriangleIntersecArea2[i].Index)
                        {
                            checkPolygon = true;
                            break;
                        }
                    }
                    if (checkPolygon == false)
                    {
                        Triangle startTriangle = new Triangle();
                        Triangle flagTriangle = new Triangle();
                        Vertex startVertex = new Vertex();
                        Vertex flagVertex = new Vertex();
                        List<Vertex> lstPolygonSub2 = new List<Vertex>();
                        List<Triangle> triangleTemp = lstTriangleIntersecArea2;
                        startTriangle = lstTriangleIntersecArea2[i];
                        flagTriangle = findPolygonTriangle(startTriangle, contourLineZ2, lstTriangleIntersecArea2, ref lstPolygonSub2);
                        triangleTemp = lstTriangleIntersecArea2;
                        //DateTime start_time = DateTime.Now;
                        while (flagTriangle.Index != startTriangle.Index)
                        {
                            triangleTemp = lstTriangleIntersecArea2;
                            triangleTemp = removeTriagleFromList(triangleTemp, flagTriangle);
                            flagTriangle = findPolygonTriangle(flagTriangle, contourLineZ2, triangleTemp, ref lstPolygonSub2);
                            exitsTrianglePolygon.Add(flagTriangle);
                            //double time = DateTime.Now.Subtract(start_time).TotalSeconds;
                            //if (time > 30)
                            //{
                            //    break;
                            //}
                            if (DateTime.Now.Subtract(start_time).TotalSeconds > MIN_TIME)
                            {
                                Console.WriteLine("Overtime");
                                break;
                            }
                        }
                        lstPolygonArea2.Add(lstPolygonSub2);
                    }
                    if (DateTime.Now.Subtract(start_time).TotalSeconds > MIN_TIME)
                    {
                        Console.WriteLine("Overtime");
                        break;
                    }
                }
                //Console.WriteLine("Số polygon giao mặt 2 = " + lstPolygonArea2.Count);
            }
            //Console.WriteLine("\r\n Thời gian tìm các polygon = " + System.DateTime.Now.Subtract(start1).TotalMilliseconds.ToString() + " msec");
            //timePolygon = System.DateTime.Now.Subtract(start1).TotalMilliseconds.ToString() + " msec";
            //Lọc polygon trùng nhau
            List<List<Vertex>> lstPolygonArea1Last = new List<List<Vertex>>();
            List<Vertex> lstTemp = new List<Vertex>();
            foreach (var item in lstPolygonArea1)
            {
                if (lstTemp != null)
                {
                    bool isExits = false;
                    Vertex center = findCenter(item);
                    foreach (var itemX in lstTemp)
                    {
                        if (itemX.DistanceXY(center) >= 5)
                        {
                            isExits = false;
                        }
                        else
                        {
                            isExits = true;
                        }
                    }
                    if (isExits == false)
                    {
                        lstTemp.Add(center);
                        lstPolygonArea1Last.Add(item);
                    }
                }
                else
                {
                    lstTemp.Add(findCenter(item));
                    lstPolygonArea1Last.Add(item);
                }
            }
            Console.WriteLine("Polygon SAU KHI LOC KHOANG 1: " + lstPolygonArea1Last.Count);

            List<List<Vertex>> lstPolygonArea2Last = new List<List<Vertex>>();
            lstTemp = new List<Vertex>();
            foreach (var item in lstPolygonArea2)
            {
                if (lstTemp != null)
                {
                    bool isExits = false;
                    Vertex center = findCenter(item);
                    foreach (var itemX in lstTemp)
                    {
                        if (itemX.DistanceXY(center) >= 5)
                        {
                            isExits = false;
                        }
                        else
                        {
                            isExits = true;
                        }
                    }
                    if (isExits == false)
                    {
                        lstTemp.Add(center);
                        lstPolygonArea2Last.Add(item);
                    }
                }
                else
                {
                    lstTemp.Add(findCenter(item));
                    lstPolygonArea2Last.Add(item);
                }
            }
            Console.WriteLine("Polygon SAU KHI LOC KHOANG 3 : " + lstPolygonArea2Last.Count);

            lstPolygonArea1 = lstPolygonArea1Last;
            lstPolygonArea2 = lstPolygonArea2Last;

            List<Vertex> lstCenter = new List<Vertex>();
            foreach (var item in lstPolygonArea1)
            {
                Vertex temp = findCenter(item);
                if (findVertexInList(lstCenter, temp) == false)
                {
                    //Tính cường độ từ trường của bom
                    List<Vertex> lstVertexInPolygon = new List<Vertex>();
                    foreach (var set in Set)
                    {
                        if (IsPointInPolygon4(item, set) == true)
                        {
                            lstVertexInPolygon.Add(set);
                        }
                    }
                    double distanceKey = 10000;
                    Vertex vertexKey = new Vertex();
                    foreach (var lstVertexInPolygonTemp in lstVertexInPolygon)
                    {
                        Double distance = Math.Sqrt((lstVertexInPolygonTemp.X - temp.X) * (lstVertexInPolygonTemp.X - temp.X)
                            + (lstVertexInPolygonTemp.Y - temp.Y) * (lstVertexInPolygonTemp.Y - temp.Y)
                            + (lstVertexInPolygonTemp.Z - temp.Z) * (lstVertexInPolygonTemp.Z - temp.Z));
                        if (distance < distanceKey)
                        {
                            vertexKey = lstVertexInPolygonTemp;
                        }
                    }
                    temp.Z = vertexKey.Z;


                    //Tính độ sâu
                    Vertex center = temp;
                    List<Vertex> polygonIntersecD = new List<Vertex>();
                    for (int i = 0; i < item.Count; i++)
                    {
                        Vertex M1 = new Vertex(); ;
                        Vertex M2 = new Vertex(); ;
                        if (i == item.Count - 1)
                        {
                            M1 = item[i];
                            M2 = item[0];
                        }
                        else
                        {
                            M1 = item[i];
                            M2 = item[i + 1];
                        }
                        Vertex vectorPhapTuyen = new Vertex();
                        try
                        {
                            Vertex vectorChiPhuong = new Vertex();
                            vectorChiPhuong.X = double.Parse(M2.X.ToString("n4")) - double.Parse(M1.X.ToString("n4"));
                            vectorChiPhuong.Y = double.Parse(M2.Y.ToString("n4")) - double.Parse(M1.Y.ToString("n4"));
                            vectorPhapTuyen.X = (-1) * vectorChiPhuong.Y;
                            vectorPhapTuyen.Y = vectorChiPhuong.X;
                        }
                        catch (Exception ex)
                        {
                            Vertex vectorChiPhuong = new Vertex();
                            vectorChiPhuong.X = M2.X - M1.X;
                            vectorChiPhuong.Y = M2.Y - M1.Y;
                            vectorPhapTuyen.X = (-1) * vectorChiPhuong.Y;
                            vectorPhapTuyen.Y = vectorChiPhuong.X;
                        }

                        if (vectorPhapTuyen.Y != 0)
                        {
                            double cLineH = (-1) * (vectorPhapTuyen.X * M1.X) + (-1) * (vectorPhapTuyen.Y * M1.Y);
                            double intersecX = center.X;
                            double intersecY = (((-1) * cLineH) - (vectorPhapTuyen.X * intersecX)) / vectorPhapTuyen.Y;
                            //double intersecX = ((-1) * cLineH - vectorPhapTuyen.Y * center.Y) / vectorPhapTuyen.X;
                            //double intersecY = center.Y;
                            if ((M1.X <= intersecX && M2.X >= intersecX) || M2.X <= intersecX && M1.X >= intersecX)
                            {
                                polygonIntersecD.Add(new Vertex(intersecX, intersecY, M1.Z, Vertex.TYPE_MINE));
                            }
                        }
                    }
                    if (polygonIntersecD.Count == 2)
                    {
                        double distanceBP = ((polygonIntersecD[0].X - polygonIntersecD[1].X) * (polygonIntersecD[0].X - polygonIntersecD[1].X) + (polygonIntersecD[0].Y - polygonIntersecD[1].Y) * (polygonIntersecD[0].Y - polygonIntersecD[1].Y));
                        double distanceAB = Math.Sqrt(distanceBP);
                        double doSauBom = 0;
                        doSauBom = distanceAB * 1.25;
                        temp.depth = doSauBom;
                        //Console.WriteLine("ĐO SAU BOM X = " + doSauBom);
                    }
                    else
                    {
                        temp.depth = 0;
                    }
                    temp.Type = Vertex.BOM;
                    lstCenter.Add(temp);
                }
            }
            foreach (var item in lstPolygonArea2)
            {
                Vertex temp = findCenter(item);

                Vertex center = temp;
                List<Vertex> polygonIntersecD = new List<Vertex>();
                for (int i = 0; i < item.Count; i++)
                {
                    Vertex M1 = new Vertex(); ;
                    Vertex M2 = new Vertex(); ;
                    if (i == item.Count - 1)
                    {
                        M1 = item[i];
                        M2 = item[0];
                    }
                    else
                    {
                        M1 = item[i];
                        M2 = item[i + 1];
                    }
                    Vertex vectorPhapTuyen = new Vertex();
                    try
                    {
                        Vertex vectorChiPhuong = new Vertex();
                        vectorChiPhuong.X = double.Parse(M2.X.ToString("n4")) - double.Parse(M1.X.ToString("n4"));
                        vectorChiPhuong.Y = double.Parse(M2.Y.ToString("n4")) - double.Parse(M1.Y.ToString("n4"));
                        vectorPhapTuyen.X = (-1) * vectorChiPhuong.Y;
                        vectorPhapTuyen.Y = vectorChiPhuong.X;
                    }
                    catch (Exception ex)
                    {
                        Vertex vectorChiPhuong = new Vertex();
                        vectorChiPhuong.X = M2.X - M1.X;
                        vectorChiPhuong.Y = M2.Y - M1.Y;
                        vectorPhapTuyen.X = (-1) * vectorChiPhuong.Y;
                        vectorPhapTuyen.Y = vectorChiPhuong.X;
                    }

                    if (vectorPhapTuyen.Y != 0)
                    {
                        double cLineH = (-1) * (vectorPhapTuyen.X * M1.X) + (-1) * (vectorPhapTuyen.Y * M1.Y);
                        double intersecX = center.X;
                        double intersecY = (((-1) * cLineH) - (vectorPhapTuyen.X * intersecX)) / vectorPhapTuyen.Y;
                        //double intersecX = ((-1) * cLineH - vectorPhapTuyen.Y * center.Y) / vectorPhapTuyen.X;
                        //double intersecY = center.Y;
                        if ((M1.X <= intersecX && M2.X >= intersecX) || M2.X <= intersecX && M1.X >= intersecX)
                        {
                            polygonIntersecD.Add(new Vertex(intersecX, intersecY, M1.Z, Vertex.TYPE_MINE));
                        }
                    }
                }
                if (polygonIntersecD.Count == 2)
                {
                    double distanceBP = ((polygonIntersecD[0].X - polygonIntersecD[1].X) * (polygonIntersecD[0].X - polygonIntersecD[1].X) + (polygonIntersecD[0].Y - polygonIntersecD[1].Y) * (polygonIntersecD[0].Y - polygonIntersecD[1].Y));
                    double distanceAB = Math.Sqrt(distanceBP);
                    double doSauBom = 0;
                    doSauBom = distanceAB * 1.25;
                    temp.depth = doSauBom;
                    //Console.WriteLine("ĐO SAU BOM Y = " + doSauBom);

                }
                else
                {
                    temp.depth = 0;
                }
                if (findVertexInList(lstCenter, temp) == false)
                {
                    temp.Type = Vertex.BOM;
                    lstCenter.Add(temp);
                }
            }
            //Console.WriteLine("lstCenterlstCenterlstCenter  = " + lstCenter.Count);
            return lstCenter;

            //List<Vertex> lstPolygon1 = lstPolygonArea1[0];
            //List<Vertex> lstPolygon2 = lstPolygonArea2[0];
            ////////////////////////////////////////////////////

            //List<Vertex> lstBomb = new List<Vertex>();
            //if (lstPolygonArea1 != null && lstPolygonArea1.Count > 0)
            //{
            //    foreach (var item in lstPolygonArea1)
            //    {
            //        Vertex center = FindCentroid(item);
            //        List<Vertex> polygonIntersecD = new List<Vertex>();
            //        for (int i = 0; i < item.Count; i++)
            //        {
            //            Vertex M1 = new Vertex(); ;
            //            Vertex M2 = new Vertex(); ;
            //            if (i == item.Count - 1)
            //            {
            //                M1 = item[i];
            //                M2 = item[0];
            //            }
            //            else
            //            {
            //                M1 = item[i];
            //                M2 = item[i + 1];
            //            }
            //            Vertex vectorPhapTuyen = new Vertex();
            //            try
            //            {
            //                Vertex vectorChiPhuong = new Vertex();
            //                vectorChiPhuong.X = double.Parse(M2.X.ToString("n4")) - double.Parse(M1.X.ToString("n4"));
            //                vectorChiPhuong.Y = double.Parse(M2.Y.ToString("n4")) - double.Parse(M1.Y.ToString("n4"));
            //                vectorPhapTuyen.X = (-1) * vectorChiPhuong.Y;
            //                vectorPhapTuyen.Y = vectorChiPhuong.X;
            //            }
            //            catch (Exception ex)
            //            {
            //                Vertex vectorChiPhuong = new Vertex();
            //                vectorChiPhuong.X = M2.X - M1.X;
            //                vectorChiPhuong.Y = M2.Y - M1.Y;
            //                vectorPhapTuyen.X = (-1) * vectorChiPhuong.Y;
            //                vectorPhapTuyen.Y = vectorChiPhuong.X;
            //            }

            //            if (vectorPhapTuyen.Y != 0)
            //            {
            //                double cLineH = (-1) * (vectorPhapTuyen.X * M1.X) + (-1) * (vectorPhapTuyen.Y * M1.Y);
            //                double intersecX = center.X;
            //                double intersecY = (((-1) * cLineH) - (vectorPhapTuyen.X * intersecX)) / vectorPhapTuyen.Y;
            //                //double intersecX = ((-1) * cLineH - vectorPhapTuyen.Y * center.Y) / vectorPhapTuyen.X;
            //                //double intersecY = center.Y;
            //                if ((M1.X <= intersecX && M2.X >= intersecX) || M2.X <= intersecX && M1.X >= intersecX)
            //                {
            //                    polygonIntersecD.Add(new Vertex(intersecX, intersecY, M1.Z));
            //                }
            //            }
            //        }
            //        if (polygonIntersecD.Count == 2)
            //        {
            //            double distanceBP = ((polygonIntersecD[0].X - polygonIntersecD[1].X) * (polygonIntersecD[0].X - polygonIntersecD[1].X) + (polygonIntersecD[0].Y - polygonIntersecD[1].Y) * (polygonIntersecD[0].Y - polygonIntersecD[1].Y));
            //            double distanceAB = Math.Sqrt(distanceBP);
            //            double doSauBom = 0;
            //            doSauBom = distanceAB * 1.25;
            //            if (findVertexInListBomb(lstBomb, new Vertex(center.X, center.Y, (double)doSauBom)) == false)
            //            {
            //                lstBomb.Add(new Vertex(center.X, center.Y, (double)doSauBom));
            //            }

            //        }
            //    }
            //}
            //if (lstPolygonArea2 != null && lstPolygonArea2.Count > 0)
            //{
            //    foreach (var item in lstPolygonArea2)
            //    {
            //        //foreach (var temp in item)
            //        //{
            //        //    //Console.WriteLine("POLYGON  " + temp.ToString());
            //        //}
            //        Vertex center = FindCentroid(item);
            //        List<Vertex> polygonIntersecD = new List<Vertex>();
            //        for (int i = 0; i < item.Count; i++)
            //        {
            //            Vertex M1 = new Vertex(); ;
            //            Vertex M2 = new Vertex(); ;
            //            if (i == item.Count - 1)
            //            {
            //                M1 = item[i];
            //                M2 = item[0];
            //            }
            //            else
            //            {
            //                M1 = item[i];
            //                M2 = item[i + 1];
            //            }
            //            Vertex vectorPhapTuyen = new Vertex();
            //            try
            //            {
            //                Vertex vectorChiPhuong = new Vertex();
            //                vectorChiPhuong.X = double.Parse(M2.X.ToString("n4")) - double.Parse(M1.X.ToString("n4"));
            //                vectorChiPhuong.Y = double.Parse(M2.Y.ToString("n4")) - double.Parse(M1.Y.ToString("n4"));
            //                vectorPhapTuyen.X = (-1) * vectorChiPhuong.Y;
            //                vectorPhapTuyen.Y = vectorChiPhuong.X;
            //            }
            //            catch (Exception ex)
            //            {
            //                Vertex vectorChiPhuong = new Vertex();
            //                vectorPhapTuyen.X = M2.X - M1.X;
            //                vectorPhapTuyen.Y = M2.Y - M1.Y;
            //                vectorPhapTuyen.X = (-1) * vectorChiPhuong.Y;
            //                vectorPhapTuyen.Y = vectorChiPhuong.X;
            //            }

            //            if (vectorPhapTuyen.Y != 0)
            //            {
            //                double cLineH = (-1) * (vectorPhapTuyen.X * M1.X) + (-1) * (vectorPhapTuyen.Y * M1.Y);
            //                double intersecX = center.X;
            //                double intersecY = (((-1) * cLineH) - (vectorPhapTuyen.X * intersecX)) / vectorPhapTuyen.Y;
            //                //double intersecX = ((-1) * cLineH - vectorPhapTuyen.Y * center.Y) / vectorPhapTuyen.X;
            //                //double intersecY = center.Y;
            //                if ((M1.X <= intersecX && M2.X >= intersecX) || M2.X <= intersecX && M1.X >= intersecX)
            //                {
            //                    polygonIntersecD.Add(new Vertex(intersecX, intersecY, M1.Z));
            //                }
            //            }
            //        }
            //        if (polygonIntersecD.Count == 2)
            //        {
            //            double distanceBP = ((polygonIntersecD[0].X - polygonIntersecD[1].X) * (polygonIntersecD[0].X - polygonIntersecD[1].X) + (polygonIntersecD[0].Y - polygonIntersecD[1].Y) * (polygonIntersecD[0].Y - polygonIntersecD[1].Y));
            //            double distanceAB = Math.Sqrt(distanceBP);
            //            double doSauBom = 0;
            //            doSauBom = distanceAB * 1.25;
            //            if (findVertexInListBomb(lstBomb, new Vertex(center.X, center.Y, (double)doSauBom)) == false)
            //            {
            //                lstBomb.Add(new Vertex(center.X, center.Y, (double)doSauBom));
            //            }

            //        }
            //    }
            //}
            ////Console.WriteLine("SỐ LƯỢNG BOM = " + lstBomb.Count);



            //////Lấy trọng tâm polygon 2 và polygon 1
            ////// X, Y trọng tâm là X, Y Quả bom/ mìn
            ////Vertex center1 = FindCentroid(lstPolygon1);
            ////Vertex center2 = FindCentroid(lstPolygon2);
            //////Công việc làm trên mặt phẳng polygon -> Quy về MP 2 chiều
            //////Đường thẳng d song song trục bắc - nam và đi qua trọng tâm polygon
            //////-> Đường thẳng có vector chỉ phương (0,1) và đi qua trọng tâm
            //////-> Tìm được PT đường thẳng d : 1(x-center.X) + 0(y-center.Y) = 0
            //////-> x - center.X = 0 (PT đường thẳng d)
            //////Tìm các điểm giao giữa các polygon và đường thẳng d

            ////List<Vertex> polygonIntersecD1 = new List<Vertex>();
            ////List<Vertex> polygonIntersecD2 = new List<Vertex>();

            //////Đường thẳng có vector pháp tuyến n(v2.x-v1.x, v2.y-v1.y), đi qua M(v1.x, v1.y)
            //////PT đường thẳng h: n.X*(x-v1.x) + n.Y*(y-v1.Y) = 0 <->  n.X*X + n.Y*y + (-n.X*v1.x - n.Y*v1.Y) = 0
            ////for (int i = 0; i < lstPolygon2.Count; i++)
            ////{
            ////    Vertex M1 = new Vertex(); ;
            ////    Vertex M2 = new Vertex(); ;
            ////    if (i == lstPolygon2.Count - 1)
            ////    {
            ////        M1 = lstPolygon2[i];
            ////        M2 = lstPolygon2[0];
            ////    }
            ////    else
            ////    {
            ////        M1 = lstPolygon2[i];
            ////        M2 = lstPolygon2[i + 1];
            ////    }
            ////    Vertex vectorPhapTuyen = new Vertex();
            ////    //vectorPhapTuyen.X = M2.X - M1.X;
            ////    //vectorPhapTuyen.Y = M2.Y - M1.Y;

            ////    try
            ////    {
            ////        vectorPhapTuyen.X = double.Parse(M2.X.ToString("n4")) - double.Parse(M1.X.ToString("n4"));
            ////        vectorPhapTuyen.Y = double.Parse(M2.Y.ToString("n4")) - double.Parse(M1.Y.ToString("n4"));
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        vectorPhapTuyen.X = M2.X - M1.X;
            ////        vectorPhapTuyen.Y = M2.Y - M1.Y;
            ////    }

            ////    double cLineH = (-1) * (vectorPhapTuyen.X * M1.X) + (-1) * (vectorPhapTuyen.Y * M1.Y);
            ////    double intersecX = ((-1) * cLineH - vectorPhapTuyen.Y * center2.Y) / vectorPhapTuyen.X;
            ////    double intersecY = center2.Y;
            ////    if ((M1.X <= intersecX && M2.X >= intersecX) || M2.X <= intersecX && M1.X >= intersecX)
            ////    {
            ////        polygonIntersecD2.Add(new Vertex(intersecX, intersecY, M1.Z));
            ////    }
            ////    //Console.WriteLine("Điểm A = " + M1.ToString() + "   Điểm B = " + M2.ToString() + "   Pháp tuyến = " + vectorPhapTuyen.ToString());
            ////    //Console.WriteLine("Điểm giao = " + intersecX + "/" + intersecY);
            ////}

            //////Đường thẳng có vector pháp tuyến n(v2.x-v1.x, v2.y-v1.y), đi qua M(v1.x, v1.y)
            //////PT đường thẳng h: n.X*(x-v1.x) + n.Y*(y-v1.Y) = 0 <->  n.X*X + n.Y*y + (-n.X*v1.x - n.Y*v1.Y) = 0
            //////for (int i = 0; i < lstPolygon1.Count; i++)
            //////{
            //////    Vertex M1 = new Vertex(); ;
            //////    Vertex M2 = new Vertex(); ;
            //////    if (i == lstPolygon1.Count - 1)
            //////    {
            //////        M1 = lstPolygon1[i];
            //////        M2 = lstPolygon1[0];
            //////    }
            //////    else
            //////    {
            //////        M1 = lstPolygon1[i];
            //////        M2 = lstPolygon1[i + 1];
            //////    }
            //////    Vertex vectorPhapTuyen = new Vertex();
            //////    //vectorPhapTuyen.X = M2.X - M1.X;
            //////    //vectorPhapTuyen.Y = M2.Y - M1.Y;

            //////    try
            //////    {
            //////        vectorPhapTuyen.X = double.Parse(M2.X.ToString("n4")) - double.Parse(M1.X.ToString("n4"));
            //////        vectorPhapTuyen.Y = double.Parse(M2.Y.ToString("n4")) - double.Parse(M1.Y.ToString("n4"));
            //////    }
            //////    catch (Exception ex)
            //////    {
            //////        vectorPhapTuyen.X = M2.X - M1.X;
            //////        vectorPhapTuyen.Y = M2.Y - M1.Y;
            //////    }

            //////    double cLineH = (-1) * (vectorPhapTuyen.X * M1.X) + (-1) * (vectorPhapTuyen.Y * M1.Y);
            //////    double intersecX = ((-1) * cLineH - vectorPhapTuyen.Y * center2.Y) / vectorPhapTuyen.X;
            //////    double intersecY = center1.Y;
            //////    if ((M1.X <= intersecX && M2.X >= intersecX) || M2.X <= intersecX && M1.X >= intersecX)
            //////    {
            //////        polygonIntersecD1.Add(new Vertex(intersecX, intersecY, M1.Z));
            //////    }
            //////    //Console.WriteLine("Điểm A = " + M1.ToString() + "   Điểm B = " + M2.ToString() + "   Pháp tuyến = " + vectorPhapTuyen.ToString());
            //////    //Console.WriteLine("Điểm giao = " + intersecX + "/" + intersecY);
            //////}

            //////Tính độ sâu của bom
            ////double doSauBom2 = 0;
            ////if (polygonIntersecD2.Count == 2)
            ////{
            ////    double distanceBP2 = ((polygonIntersecD2[0].X - polygonIntersecD2[1].X) * (polygonIntersecD2[0].X - polygonIntersecD2[1].X) + (polygonIntersecD2[0].Y - polygonIntersecD2[1].Y) * (polygonIntersecD2[0].Y - polygonIntersecD2[1].Y));
            ////    double distanceAB2 = Math.Sqrt(distanceBP2);
            ////    doSauBom2 = distanceAB2 * 1.25;
            ////}
            //////double doSauBom1 = 0;
            //////if (polygonIntersecD1.Count == 2)
            //////{
            //////    double distanceBP1 = ((polygonIntersecD1[0].X - polygonIntersecD1[1].X) * (polygonIntersecD1[0].X - polygonIntersecD1[1].X) + (polygonIntersecD1[0].Y - polygonIntersecD1[1].Y) * (polygonIntersecD1[0].Y - polygonIntersecD1[1].Y));
            //////    double distanceAB1 = Math.Sqrt(distanceBP1);
            //////    doSauBom1 = distanceAB1 * 1.25;
            //////}
        }

        private Vertex findCenter(List<Vertex> poly)
        {
            //double tongX = 0;
            //double tongY = 0;
            //double z = 0;
            //foreach (var item in polygon)
            //{
            //    tongX += item.X;
            //    tongY += item.Y;
            //    z = item.Z;
            //}
            //return new Vertex(tongX / polygon.Count, tongY / polygon.Count, z, Vertex.TYPE_MINE);
            double accumulatedArea = 0.0f;
            double centerX = 0.0f;
            double centerY = 0.0f;
            double z = 0;

            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                double temp = poly[i].X * poly[j].Y - poly[j].X * poly[i].Y;
                accumulatedArea += temp;
                centerX += (poly[i].X + poly[j].X) * temp;
                centerY += (poly[i].Y + poly[j].Y) * temp;
                z = poly[i].Z;
            }

            if (Math.Abs(accumulatedArea) < 1E-7f)
                return new Vertex(0, 0, 0, Vertex.TYPE_BOMB);  // Avoid division by zero

            accumulatedArea *= 3f;
            return new Vertex(centerX / accumulatedArea, centerY / accumulatedArea, z, Vertex.TYPE_MINE);
        }

        //Tìm polygon theo tam giác liền kề
        private Triangle findPolygonTriangle(Triangle triangleSelected, double z, List<Triangle> lstTriangleIntersecArea1, ref List<Vertex> lstPolygonSub1)
        {
            List<int> intersectStatus = new List<int>();
            List<Vertex> lstTest = getIntersectVertexNew(triangleSelected, z, ref intersectStatus);
            //Lấy điểm giao 1 và cạnh đó để tìm tam giác liền kề
            //intersectStatus: 1- Cắt AB, 2- Cắt BC, 3- Cắt CA
            Triangle nearTriangle = new Triangle();
            if (intersectStatus != null && intersectStatus.Count > 1)
            {
                bool isClose = false;
                int index = 0;
                for (int i = 0; i < lstTest.Count; i++)
                {
                    if (findVertexInList(lstPolygonSub1, lstTest[i]) == false)
                    {
                        index = i;
                    }
                }
                ////Console.WriteLine("Kiểm tra điểm trong polygon  " + findVertexInList(lstPolygonSub1, lstTest[0]) + "/" + findVertexInList(lstPolygonSub1, lstTest[1]));
                if (findVertexInList(lstPolygonSub1, lstTest[0]) == true && findVertexInList(lstPolygonSub1, lstTest[1]) == true)
                {
                    index = findIndexVertexInList(lstTest, lstPolygonSub1[0]);
                }

                if (intersectStatus[index] == 1)
                {
                    findNearTriangle(lstTriangleIntersecArea1, triangleSelected, triangleSelected.A, triangleSelected.B, ref nearTriangle);
                }
                if (intersectStatus[index] == 2)
                {
                    findNearTriangle(lstTriangleIntersecArea1, triangleSelected, triangleSelected.B, triangleSelected.C, ref nearTriangle);
                }
                if (intersectStatus[index] == 3)
                {
                    findNearTriangle(lstTriangleIntersecArea1, triangleSelected, triangleSelected.C, triangleSelected.A, ref nearTriangle);
                }
                lstPolygonSub1.Add(lstTest[index]);
            }
            ////Console.WriteLine("Triangle 1 =" + triangleSelected.ToString());
            ////Console.WriteLine("Triangle near =" + nearTriangle.Index);
            ////Console.WriteLine("Điểm giao: (" + lstTest[0].ToString() + "), (" + lstTest[1] + ")");
            return nearTriangle;
        }

        //Tìm tam giác liền kề
        private bool findNearTriangle(List<Triangle> lstTriangleInput, Triangle triangleSeleted, Vertex v1Input, Vertex v2Input, ref Triangle result)
        {
            List<Triangle> lstTriangleInputNew = new List<Triangle>();
            foreach (var item in lstTriangleInput)
            {
                if (equalTriagle(triangleSeleted, item) == false)
                {
                    lstTriangleInputNew.Add(item);
                }
            }
            foreach (var item in lstTriangleInputNew)
            {
                if (
                    ((item.A.X == v1Input.X && item.A.Y == v1Input.Y && item.A.Z == v1Input.Z) && (item.B.X == v2Input.X && item.B.Y == v2Input.Y && item.B.Z == v2Input.Z))
                    || ((item.A.X == v2Input.X && item.A.Y == v2Input.Y && item.A.Z == v2Input.Z) && (item.B.X == v1Input.X && item.B.Y == v1Input.Y && item.B.Z == v1Input.Z))
                    || ((item.A.X == v1Input.X && item.A.Y == v1Input.Y && item.A.Z == v1Input.Z) && (item.C.X == v2Input.X && item.C.Y == v2Input.Y && item.C.Z == v2Input.Z))
                    || ((item.A.X == v2Input.X && item.A.Y == v2Input.Y && item.A.Z == v2Input.Z) && (item.C.X == v1Input.X && item.C.Y == v1Input.Y && item.C.Z == v1Input.Z))
                    || ((item.B.X == v1Input.X && item.B.Y == v1Input.Y && item.B.Z == v1Input.Z) && (item.C.X == v2Input.X && item.C.Y == v2Input.Y && item.C.Z == v2Input.Z))
                    || ((item.B.X == v2Input.X && item.B.Y == v2Input.Y && item.B.Z == v2Input.Z) && (item.C.X == v1Input.X && item.C.Y == v1Input.Y && item.C.Z == v1Input.Z))
                    )
                {
                    result = item;
                    return true;
                }
            }
            return false;
        }

        //Remove tam giác trong list
        private List<Triangle> removeTriagleFromList(List<Triangle> lstTriangleInput, Triangle triangleSeleted)
        {
            List<Triangle> lstTriangleInputNew = new List<Triangle>();
            foreach (var item in lstTriangleInput)
            {
                if (equalTriagle(triangleSeleted, item) == false)
                {
                    lstTriangleInputNew.Add(item);
                }
            }
            return lstTriangleInputNew;
        }

        //Tìm điểm trong list
        private bool findVertexInList(List<Vertex> lst, Vertex v)
        {
            foreach (var item in lst)
            {
                if (item.X.ToString("n4") == v.X.ToString("n4") && item.Y.ToString("n4") == v.Y.ToString("n4") && item.Z.ToString("n4") == v.Z.ToString("n4"))
                {
                    return true;
                }
            }
            return false;
        }

        private bool findVertexInListBomb(List<Vertex> lst, Vertex v)
        {
            foreach (var item in lst)
            {
                if (item.X.ToString("n1") == v.X.ToString("n1") && item.Y.ToString("n1") == v.Y.ToString("n1") && item.Z.ToString("n1") == v.Z.ToString("n1"))
                {
                    return true;
                }
            }
            return false;
        }

        //Tìm vị trí điểm trong list
        private int findIndexVertexInList(List<Vertex> lst, Vertex v)
        {
            int result = 0;
            for (var i = 0; i < lst.Count; i++)
            {
                if (lst[i].X == v.X && lst[i].Y == v.Y && lst[i].Z == v.Z)
                {
                    result = i;
                }
            }
            return result;
        }

        private bool equalTriagle(Triangle t1, Triangle t2)
        {
            if (t1.A != null && t1.B != null && t1.C != null && t2.A != null && t2.B != null && t2.C != null)
            {
                if (
                    t1.A.X == t2.A.X && t1.A.Y == t2.A.Y && t1.A.Z == t2.A.Z
                    && t1.B.X == t2.B.X && t1.B.Y == t2.B.Y && t1.B.Z == t2.B.Z
                    && t1.C.X == t2.C.X && t1.C.Y == t2.C.Y && t1.C.Z == t2.C.Z
                    )
                {
                    return true;
                }
            }
            return false;
        }

        //input Tam giác và mặt phẳng song song Oxy
        //output List điểm mặt phẳng cắt tam giác
        private List<Vertex> getIntersectVertex(Triangle triangle, double z)
        {
            //indexVertexA = 0 ~ Không cắt
            //indexVertexA = 1 ~ Có cắt
            int indexVertexA = 0;
            int indexVertexB = 0;
            int indexVertexC = 0;
            List<Vertex> result = new List<Vertex>();
            //Nếu trong 3 đỉnh có đỉnh nào thuộc thì cắt tam giác tại đỉnh đó
            if (triangle.A.Z == z)
            {
                result.Add(triangle.A);
                indexVertexA = 1;
            }
            if (triangle.B.Z == z)
            {
                result.Add(triangle.B);
                indexVertexB = 1;
            }
            if (triangle.C.Z == z)
            {
                result.Add(triangle.C);
                indexVertexC = 1;
            }
            //Nếu không cắt đỉnh nào, hoặc 1 đỉnh của tam giác thì tìm điểm giao
            if (result.Count == 1)
            {
                if (indexVertexA == 1)
                {
                    Vertex vertexTemp = getIntersectVertexLineWithPlane(triangle.B, triangle.C, z);
                    if (vertexTemp != null)
                    {
                        result.Add(vertexTemp);
                    }
                }
                if (indexVertexB == 1)
                {
                    Vertex vertexTemp = getIntersectVertexLineWithPlane(triangle.A, triangle.C, z);
                    if (vertexTemp != null)
                    {
                        result.Add(vertexTemp);
                    }
                }
                if (indexVertexC == 1)
                {
                    Vertex vertexTemp = getIntersectVertexLineWithPlane(triangle.A, triangle.B, z);
                    if (vertexTemp != null)
                    {
                        result.Add(vertexTemp);
                    }
                }
            }
            if (result.Count == 0)
            {
                Vertex vertexTempAB = getIntersectVertexLineWithPlane(triangle.A, triangle.B, z);
                if (vertexTempAB != null)
                {
                    result.Add(vertexTempAB);
                }
                Vertex vertexTempBC = getIntersectVertexLineWithPlane(triangle.C, triangle.B, z);
                if (vertexTempBC != null)
                {
                    result.Add(vertexTempBC);
                }
                Vertex vertexTempAC = getIntersectVertexLineWithPlane(triangle.A, triangle.C, z);
                if (vertexTempAC != null)
                {
                    result.Add(vertexTempAC);
                }
            }

            return result;
        }

        private List<Vertex> getIntersectVertexNew(Triangle triangle, double z, ref List<int> intersectStatus)
        {
            //intersectStatus: 1- Cắt AB, 2- Cắt BC, 3- Cắt CA

            //indexVertexA = 0 ~ Không cắt
            //indexVertexA = 1 ~ Có cắt
            int indexVertexA = 0;
            int indexVertexB = 0;
            int indexVertexC = 0;
            List<Vertex> result = new List<Vertex>();
            //Nếu trong 3 đỉnh có đỉnh nào thuộc thì cắt tam giác tại đỉnh đó
            if (triangle.A != null)
            {
                if (triangle.A.Z == z)
                {
                    result.Add(triangle.A);
                    indexVertexA = 1;
                }
                if (triangle.B.Z == z)
                {
                    result.Add(triangle.B);
                    indexVertexB = 1;
                }
                if (triangle.C.Z == z)
                {
                    result.Add(triangle.C);
                    indexVertexC = 1;
                }
                //Nếu không cắt đỉnh nào, hoặc 1 đỉnh của tam giác thì tìm điểm giao
                if (result.Count == 1)
                {
                    if (indexVertexA == 1)
                    {
                        Vertex vertexTemp = getIntersectVertexLineWithPlane(triangle.B, triangle.C, z);
                        if (vertexTemp != null)
                        {
                            result.Add(vertexTemp);
                        }
                    }
                    if (indexVertexB == 1)
                    {
                        Vertex vertexTemp = getIntersectVertexLineWithPlane(triangle.A, triangle.C, z);
                        if (vertexTemp != null)
                        {
                            result.Add(vertexTemp);
                        }
                    }
                    if (indexVertexC == 1)
                    {
                        Vertex vertexTemp = getIntersectVertexLineWithPlane(triangle.A, triangle.B, z);
                        if (vertexTemp != null)
                        {
                            result.Add(vertexTemp);
                        }
                    }
                }
                if (result.Count == 0)
                {
                    Vertex vertexTempAB = getIntersectVertexLineWithPlane(triangle.A, triangle.B, z);
                    if (vertexTempAB != null && vertexTempAB.Z != -999999)
                    {
                        result.Add(vertexTempAB);
                        intersectStatus.Add(1);
                    }
                    Vertex vertexTempBC = getIntersectVertexLineWithPlane(triangle.C, triangle.B, z);
                    if (vertexTempBC != null && vertexTempBC.Z != -999999)
                    {
                        result.Add(vertexTempBC);
                        intersectStatus.Add(2);
                    }
                    Vertex vertexTempAC = getIntersectVertexLineWithPlane(triangle.A, triangle.C, z);
                    if (vertexTempAC != null && vertexTempAC.Z != -999999)
                    {
                        result.Add(vertexTempAC);
                        intersectStatus.Add(3);
                    }
                }
            }
            return result;
        }

        private Vertex getIntersectVertexLineWithPlane(Vertex a, Vertex b, double z)
        {
            Vertex result = new Vertex();
            //PT đường thẳng có dạng
            //---
            //-x = x0 + at
            //-y = y0 + bt
            //-z = z0 + ct
            //---
            Vertex vector = new Vertex(a.X - b.X, a.Y - b.Y, a.Z - b.Z, Vertex.TYPE_MINE);
            double t = ((double)z - a.Z) / vector.Z;
            result.X = a.X + vector.X * t;
            result.Y = a.Y + vector.Y * t;
            result.Z = (double)z;
            //Kiểm tra xem điểm cắt có nằm trong ab không
            if ((result.X < a.X && result.X > b.X) || (result.X < b.X && result.X > a.X))
            {
                if ((result.Y < a.Y && result.Y > b.Y) || (result.Y < b.Y && result.Y > a.Y))
                {
                    return result;
                }
            }
            return new Vertex(0, 0, -999999, Vertex.TYPE_MINE);
        }

        private bool checkEqualVertex(Vertex a, Vertex b)
        {
            bool result = false;
            if (a.X.ToString("n2") == b.X.ToString("n2") && a.Y.ToString("n2") == b.Y.ToString("n2") && a.Z.ToString("n2") == b.Z.ToString("n2"))
                result = true;
            return result;
        }

        public Vertex FindCentroid(List<Vertex> Points)
        {
            // Add the first point at the end of the array.
            int num_points = Points.Count;
            List<Vertex> pts = new List<Vertex>();
            pts = Points;
            pts.Add(Points[0]);
            // Find the centroid.
            double X = 0;
            double Y = 0;
            double second_factor;
            for (int i = 0; i < num_points; i++)
            {
                second_factor =
                    pts[i].X * pts[i + 1].Y -
                    pts[i + 1].X * pts[i].Y;
                X += (pts[i].X + pts[i + 1].X) * second_factor;
                Y += (pts[i].Y + pts[i + 1].Y) * second_factor;
            }
            var area2 = SignedPolygonArea(Points);
            // Divide by 6 times the polygon's area.
            double polygon_area = area2;
            X /= (6 * polygon_area);
            Y /= (6 * polygon_area);

            // If the values are negative, the polygon is
            // oriented counterclockwise so reverse the signs.
            if (X < 0)
            {
                X = -X;
                Y = -Y;
            }

            return new Vertex(X, Y, Points[0].Z, Vertex.TYPE_MINE);
        }

        private double SignedPolygonArea(List<Vertex> Points)
        {
            // Add the first point to the end.
            int num_points = Points.Count;
            List<Vertex> pts = new List<Vertex>();
            pts = Points;
            pts.Add(Points[0]);

            // Get the areas.
            double area = 0;
            for (int i = 0; i < num_points; i++)
            {
                area +=
                    (pts[i + 1].X - pts[i].X) *
                    (pts[i + 1].Y + pts[i].Y) / 2;
            }

            // Return the result.
            return Math.Abs(area);
        }

        //public static void ToLatLon(double utmX, double utmY, ref double latt, ref double longt, string utmZone)
        //{
        //    bool isNorthHemisphere = utmZone.Last() >= 'N';

        //    var zone = int.Parse(utmZone.Remove(utmZone.Length - 1));
        //    var c_sa = 6378137.000000;
        //    var c_sb = 6356752.314245;
        //    var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
        //    var e2cuadrada = Math.Pow(e2, 2);
        //    var c = Math.Pow(c_sa, 2) / c_sb;
        //    var x = utmX - 500000;
        //    var y = isNorthHemisphere ? utmY : utmY - 10000000;

        //    var s = ((zone * 6.0) - 183.0);
        //    var lat = y / (6366197.724 * 0.9996); // Change c_sa for 6366197.724
        //    var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
        //    var a = x / v;
        //    var a1 = Math.Sin(2 * lat);
        //    var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
        //    var j2 = lat + (a1 / 2.0);
        //    var j4 = ((3 * j2) + a2) / 4.0;
        //    var j6 = (5 * j4 + a2 * Math.Pow((Math.Cos(lat)), 2)) / 3.0; // saque a2 de multiplicar por el coseno de lat y elevar al cuadrado
        //    var alfa = (3.0 / 4.0) * e2cuadrada;
        //    var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
        //    var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
        //    var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
        //    var b = (y - bm) / v;
        //    var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
        //    var eps = a * (1 - (epsi / 3.0));
        //    var nab = (b * (1 - epsi)) + lat;
        //    var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
        //    var delt = Math.Atan(senoheps / (Math.Cos(nab)));
        //    var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

        //    longt = ((delt / Math.PI) * 180 + s);
        //    latt = ((((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat))) / Math.PI) * 180); // era incorrecto el calculo
        //}

        public static bool IsPointInPolygon4(List<Vertex> polygon, Vertex testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static void ToLatLon(double utmX, double utmY, ref double latt, ref double longt, string utmZone)
        {
            bool isNorthHemisphere = utmZone.Last() >= 'N';

            var zone = int.Parse(utmZone.Remove(utmZone.Length - 1));
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (6366197.724 * 0.9996); // Change c_sa for 6366197.724
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = (5 * j4 + a2 * Math.Pow((Math.Cos(lat)), 2)) / 3.0; // saque a2 de multiplicar por el coseno de lat y elevar al cuadrado
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longt = ((delt / Math.PI) * 180 + s);
            latt = ((((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat))) / Math.PI) * 180); // era incorrecto el calculo
        }
    }
}
