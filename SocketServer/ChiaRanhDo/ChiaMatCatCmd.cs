using Delaunay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
//using AutoCADApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace DieuHanhCongTruong.Command
{
    internal class ChiaMatCatCmd
    {
        //public void ExecuteCmd()
        //{
        //    var db = AutoCADApp.DocumentManager.MdiActiveDocument.Database;
        //    using (Transaction ts = AutoCADApp.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
        //    {
        //        ChiaMatCat frm = new ChiaMatCat();
        //        frm.ShowDialog();
        //        if (frm.DialogResult != System.Windows.Forms.DialogResult.OK)
        //        {
        //            ts.Abort();
        //            return;
        //        }

        //        bool huongDo = frm.HuongDo;
        //        double khoangCach = frm.DistanceVal;
        //        int soRanh = (int)frm.SoRanhDo;

        //        Line line = LineJigger.Jig(false);
        //        var allExt = GetAllLine(line, huongDo, khoangCach, soRanh);

        //        allExt = allExt.OrderBy(x => x.StartPoint.X).ToList();

        //        var lstLineCreated = CreateSectionLine(allExt, khoangCach, null);

        //        ObjectLinkManager.Instance.Load(ts, db);
        //        ObjectLinkManager.Instance.Attach(AppUtils.m_Dictionary_KhoangChia, khoangCach.ToString(), db.NamedObjectsDictionaryId);
        //        ObjectLinkManager.Instance.Save(ts, db);

        //        ts.Commit();
        //    }
        //}

        //public List<ObjectId> TaoOLuoiO(Transaction ts, ChiaMatCatOLuoiFrm frm, ObjectId oidOluoiSelected)
        //{
        //    var db = AutoCADApp.DocumentManager.MdiActiveDocument.Database;

        //    var data = frm.dataDuLieu;

        //    List<ObjectId> lstOLuoi = new List<ObjectId>();
        //    Point3d ptTemp = new Point3d();
        //    double distanceTemp = 0;
        //    byte colorTemp = 0;

        //    if (data.kieuChia == 1)
        //    {
        //        var oidAll = DBUtils.GetAllRec();
        //        foreach (ObjectId oidOLuoi in oidAll)
        //            lstOLuoi.Add(oidOLuoi);
        //    }
        //    else if (data.kieuChia == 2)
        //    {
        //        var oidDuongBao = DBUtils.PromptForEntity(typeof(MgdAcDbVNTerrainBaseRegion), "\n Chọn đường bao dự án", "\n Không phải đường bao dự án");
        //        MgdAcDbVNTerrainBaseRegion duongBao = oidDuongBao.GetObject(OpenMode.ForRead) as MgdAcDbVNTerrainBaseRegion;
        //        if (duongBao == null)
        //            return lstOLuoi;

        //        var oidAll = DBUtils.GetAllRec();

        //        AcArxPolygonUtil pPolygonArx = new AcArxPolygonUtil();
        //        if (pPolygonArx.Create(duongBao.Id, ">@]x3X~9msjW.)g") == false)
        //            return lstOLuoi;

        //        foreach (ObjectId oidOLuoi in oidAll)
        //        {
        //            MgdAcDbVNTerrainRectangle rectan = oidOLuoi.GetObject(OpenMode.ForRead) as MgdAcDbVNTerrainRectangle;
        //            if (rectan == null)
        //                continue;

        //            rectan.GetGeometry(ref ptTemp, ref distanceTemp, ref colorTemp);

        //            InPolyTypeMgd typeMgd = InPolyTypeMgd.INPOLY_MGD_ERROR;
        //            typeMgd = pPolygonArx.PointInPolygon(new Point2d(ptTemp.X, ptTemp.Y));
        //            if (typeMgd == InPolyTypeMgd.INPOLY_MGD_INSIDE || typeMgd == InPolyTypeMgd.INPOLY_MGD_VERTEX || typeMgd == InPolyTypeMgd.INPOLY_MGD_EDGE)
        //                lstOLuoi.Add(oidOLuoi);
        //        }
        //    }
        //    else
        //    {
        //        if (oidOluoiSelected.IsValid == false)
        //            oidOluoiSelected = DBUtils.PromptForEntity(typeof(MgdAcDbVNTerrainRectangle), "Chọn ô lưới", "Đối tượng không phải là ô lưới");

        //        lstOLuoi.Add(oidOluoiSelected);
        //    }

        //    ObjectLinkManager.Instance.Load(ts, db);
        //    foreach (var oidOLuoi in lstOLuoi)
        //    {
        //        MgdAcDbVNTerrainRectangle rectan = oidOLuoi.GetObject(OpenMode.ForRead) as MgdAcDbVNTerrainRectangle;
        //        if (rectan == null)
        //            continue;


        //        var lstLineCreated = DrawLineJigGocPhanTu(rectan, data);


        //        if (lstLineCreated.Count != 0)
        //        {
        //            if (ObjectLinkManager.Instance.IsDictionaryNameExist(AppUtils.m_Dictionary_MatCatTuTruongTrongOLuoi, oidOLuoi))
        //            {
        //                var mapLink = ObjectLinkManager.Instance.GetLink(AppUtils.m_Dictionary_MatCatTuTruongTrongOLuoi, oidOLuoi);

        //                var xdataValue = mapLink.ChildString;
        //                var xdataValue1 = mapLink.ChildObjectIdList;

        //                foreach (var oid in xdataValue1)
        //                {
        //                    try
        //                    {
        //                        if (oid.IsValid == false)
        //                            continue;

        //                        Entity ent = oid.GetObject(OpenMode.ForWrite) as Entity;
        //                        if (ent == null)
        //                            continue;

        //                        if (ent.IsErased)
        //                            continue;

        //                        ent.Erase();
        //                    }
        //                    catch (System.Exception ex)
        //                    {
        //                        var mess = ex.Message;
        //                    }
        //                }
        //            }
        //            if (TaoDiemDoCMD.oLuoi__id.ContainsKey(rectan.ObjectId))
        //            {
        //                data.oluoi_id = TaoDiemDoCMD.oLuoi__id[rectan.ObjectId].gid;
        //            }
        //            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        //            ObjectLinkManager.Instance.Attach(AppUtils.m_Dictionary_MatCatTuTruongTrongOLuoi, oidOLuoi, lstLineCreated);
        //            ObjectLinkManager.Instance.Attach(AppUtils.m_Dictionary_MatCatTuTruongTrongOLuoi, jsonString, rectan.ObjectId);
        //            if (TaoDiemDoCMD.oLuoi__id.ContainsKey(oidOLuoi))
        //            {
        //                string oLuoi_json = JsonConvert.SerializeObject(TaoDiemDoCMD.oLuoi__id[oidOLuoi]);
        //                foreach (ObjectId line in lstLineCreated)
        //                {
        //                    ObjectLinkManager.Instance.Attach(AppUtils.m_Dictionary_MatCatTuTruongTrongOLuoi, oLuoi_json, line);
        //                }
        //            }
        //        }
        //    }

        //    ObjectLinkManager.Instance.Save(ts, db);

        //    return lstOLuoi;
        //}

        //public void ExecuteTheoOCmd()
        //{
        //    using (Transaction ts = AutoCADApp.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
        //    {
        //        ChiaMatCatOLuoiFrm frm = new ChiaMatCatOLuoiFrm(false);
        //        frm.ShowDialog();
        //        if (frm.DialogResult != System.Windows.Forms.DialogResult.OK)
        //        {
        //            ts.Abort();
        //            return;
        //        }

        //        TaoOLuoiO(ts, frm, ObjectId.Null);

        //        ts.Commit();
        //        LuuRanhDoCmd cmd2 = new LuuRanhDoCmd();
        //        cmd2.ExecuteCmd();
        //    }
        //}

        //public void ExecuteChinhSuaRanhDoTheoOCmd()
        //{
        //    var db = AutoCADApp.DocumentManager.MdiActiveDocument.Database;
        //    var editor = AutoCADApp.DocumentManager.MdiActiveDocument.Editor;
        //    var acCurDb = editor.Document.Database;

        //    using (Transaction acTrans = AutoCADApp.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
        //    {
        //        var oidOLuoi = DBUtils.PromptForEntity(typeof(MgdAcDbVNTerrainRectangle), "Chọn ô lưới", "Đối tượng không phải là ô lưới");

        //        MgdAcDbVNTerrainRectangle rectan = oidOLuoi.GetObject(OpenMode.ForRead) as MgdAcDbVNTerrainRectangle;
        //        if (rectan == null)
        //            return;

        //        // get all name
        //        ObjectLinkManager.Instance.Load(acTrans, acCurDb);
        //        if (ObjectLinkManager.Instance.IsDictionaryNameExist(AppUtils.m_Dictionary_MatCatTuTruongTrongOLuoi, rectan.ObjectId))
        //        {
        //            var mapLink = ObjectLinkManager.Instance.GetLink(AppUtils.m_Dictionary_MatCatTuTruongTrongOLuoi, rectan.ObjectId);

        //            var xdataValue = mapLink.ChildString;
        //            var xdataValue1 = mapLink.ChildObjectIdList;

        //            var dataDuLieu = JsonConvert.DeserializeObject<ChiaMatCatOLuoiData>(xdataValue);

        //            ChiaMatCatOLuoiFrm frm = new ChiaMatCatOLuoiFrm(true);
        //            frm.dataDuLieu = dataDuLieu;
        //            frm.ShowDialog();
        //            if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
        //            {
        //                var lstOLuoi = TaoOLuoiO(acTrans, frm, rectan.Id);
        //                LuuRanhDoCmd cmd2 = new LuuRanhDoCmd();
        //                cmd2.ExecuteCmd();
        //            }
        //        }

        //        acTrans.Commit();
        //    }
        //}

        //private List<ObjectId> DrawLineJigGocPhanTu(MgdAcDbVNTerrainRectangle rectangle, ChiaMatCatOLuoiData data)
        //{
        //    List<ObjectId> retVal = new List<ObjectId>();

        //    Point3d ptOrigin = new Point3d();
        //    double distanceP = 0;
        //    byte colorIndex = 0;

        //    Line lineGoc1 = null, lineGoc2 = null, lineGoc3 = null, lineGoc4 = null;
        //    int soRanh1 = 1, soRanh2 = 1, soRanh3 = 1, soRanh4 = 1;

        //    rectangle.GetGeometry(ref ptOrigin, ref distanceP, ref colorIndex);
        //    var lstPDistance = GetPDistance(ptOrigin, distanceP);

        //    //var color1 = Autodesk.AutoCAD.Colors.Color.FromRgb(153, 204, 0);
        //    //var color2 = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 0, 255);

        //    var color1 = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 255, 255);
        //    var color2 = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 255, 255);

        //    // Line goc 1
        //    {
        //        if (data.isBacNamGoc1 == 3)
        //            lineGoc1 = new Line(lstPDistance[6], lstPDistance[0]);
        //        else
        //            lineGoc1 = new Line(lstPDistance[0], lstPDistance[4]);

        //        soRanh1 = (int)(lineGoc1.Length / data.khoangCachChia1);
        //        soRanh1 = soRanh1 + 1;

        //        List<Line> allExt = new List<Line>();
        //        if (data.isBacNamGoc1 == 1)
        //            allExt = GetLstExtRotate(lineGoc1, data.khoangCachChia1, soRanh1, data.gocTuyChon1, lstPDistance[0], lstPDistance[4], lstPDistance[8], lstPDistance[6]);
        //        else
        //            allExt = GetAllLine(lineGoc1, true, data.khoangCachChia1, soRanh1);

        //        //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
        //        var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia1, color1);
        //        retVal.AddRange(lstLineCreated);
        //    }

        //    // Line goc 2
        //    {
        //        if (data.isBacNamGoc2 == 3)
        //            lineGoc2 = new Line(lstPDistance[8], lstPDistance[4]);
        //        else
        //            lineGoc2 = new Line(lstPDistance[4], lstPDistance[3]);

        //        soRanh2 = (int)(lineGoc2.Length / data.khoangCachChia2);
        //        soRanh2 = soRanh2 + 1;
        //        List<Line> allExt = new List<Line>();
        //        if (data.isBacNamGoc2 == 1)
        //            allExt = GetLstExtRotate(lineGoc2, data.khoangCachChia2, soRanh2, data.gocTuyChon2, lstPDistance[4], lstPDistance[3], lstPDistance[7], lstPDistance[8]);
        //        else
        //            allExt = GetAllLine(lineGoc2, true, data.khoangCachChia2, soRanh2);
        //        //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
        //        var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia2, color2);
        //        retVal.AddRange(lstLineCreated);
        //    }

        //    // Line goc 3
        //    {
        //        if (data.isBacNamGoc3 == 3)
        //            lineGoc3 = new Line(lstPDistance[1], lstPDistance[6]);
        //        else
        //            lineGoc3 = new Line(lstPDistance[6], lstPDistance[8]);

        //        soRanh3 = (int)(lineGoc3.Length / data.khoangCachChia3);
        //        soRanh3 = soRanh3 + 1;
        //        List<Line> allExt = new List<Line>();
        //        if (data.isBacNamGoc3 == 1)
        //            allExt = GetLstExtRotate(lineGoc3, data.khoangCachChia3, soRanh3, data.gocTuyChon3, lstPDistance[6], lstPDistance[8], lstPDistance[5], lstPDistance[1]);
        //        else
        //            allExt = GetAllLine(lineGoc3, true, data.khoangCachChia3, soRanh3);
        //        //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
        //        var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia3, color2);
        //        retVal.AddRange(lstLineCreated);
        //    }

        //    // Line goc 4
        //    {
        //        if (data.isBacNamGoc4 == 3)
        //            lineGoc4 = new Line(lstPDistance[5], lstPDistance[8]);
        //        else
        //            lineGoc4 = new Line(lstPDistance[8], lstPDistance[7]);

        //        soRanh4 = (int)(lineGoc4.Length / data.khoangCachChia4);
        //        soRanh4 = soRanh4 + 1;
        //        List<Line> allExt = new List<Line>();
        //        if (data.isBacNamGoc4 == 1)
        //            allExt = GetLstExtRotate(lineGoc4, data.khoangCachChia4, soRanh4, data.gocTuyChon4, lstPDistance[8], lstPDistance[7], lstPDistance[2], lstPDistance[5]);
        //        else
        //            allExt = GetAllLine(lineGoc4, true, data.khoangCachChia4, soRanh4);
        //        //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
        //        var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia4, color1);
        //        retVal.AddRange(lstLineCreated);
        //    }

        //    return retVal;
        //}

        public List<CecmProgramAreaLineDTO> DrawLineJigGocPhanTu2(Boundary rectangle, ChiaMatCatOLuoiData data)
        {
            List<CecmProgramAreaLineDTO> retVal = new List<CecmProgramAreaLineDTO>();

            Point2d ptOrigin = new Point2d();
            double distanceP = 0;
            byte colorIndex = 0;

            CecmProgramAreaLineDTO lineGoc1 = new CecmProgramAreaLineDTO(), lineGoc2 = new CecmProgramAreaLineDTO(), lineGoc3 = new CecmProgramAreaLineDTO(), lineGoc4 = new CecmProgramAreaLineDTO();
            int soRanh1 = 1, soRanh2 = 1, soRanh3 = 1, soRanh4 = 1;

            //rectangle.GetGeometry(ref ptOrigin, ref distanceP, ref colorIndex);
            var lstPDistance = GetPDistance(rectangle);

            //var color1 = Autodesk.AutoCAD.Colors.Color.FromRgb(153, 204, 0);
            //var color2 = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 0, 255);

            // Line goc 1
            {
                if (data.isBacNamGoc1 == 3)
                {
                    lineGoc1 = new CecmProgramAreaLineDTO();
                    lineGoc1.start_x = lstPDistance[6].X;
                    lineGoc1.start_y = lstPDistance[6].Y;
                    lineGoc1.end_x = lstPDistance[0].X;
                    lineGoc1.end_y = lstPDistance[0].Y;
                    //lineGoc1 = new Line(lstPDistance[6], lstPDistance[0]);
                }

                else
                {
                    lineGoc1 = new CecmProgramAreaLineDTO();
                    lineGoc1.start_x = lstPDistance[0].X;
                    lineGoc1.start_y = lstPDistance[0].Y;
                    lineGoc1.end_x = lstPDistance[4].X;
                    lineGoc1.end_y = lstPDistance[4].Y;
                    //lineGoc1 = new Line(lstPDistance[0], lstPDistance[4]);
                }


                soRanh1 = (int)(lineGoc1.Length / data.khoangCachChia1);
                soRanh1 = soRanh1 + 1;

                List<CecmProgramAreaLineDTO> allExt = new List<CecmProgramAreaLineDTO>();
                if (data.isBacNamGoc1 == 1)
                {
                    //if (data.gocTuyChon1 % 180 == 0)
                    //{
                    //    allExt = GetAllLine(lineGoc1, true, data.khoangCachChia1, soRanh1, 0);
                    //}
                    //else if (data.gocTuyChon1 % 180 == 90)
                    //{
                    //    allExt = GetAllLine(lineGoc1, false, data.khoangCachChia1, soRanh1, Math.PI / 1);
                    //}
                    //else
                    //{
                        allExt = GetLstExtRotate(lineGoc1, data.khoangCachChia1, soRanh1, data.gocTuyChon1, lstPDistance[0], lstPDistance[4], lstPDistance[8], lstPDistance[6]);
                    //}
                }
                    
                else if (data.isBacNamGoc1 == 2)
                {
                    allExt = GetAllLine(lineGoc1, true, data.khoangCachChia1, soRanh1, 0);
                }
                else if (data.isBacNamGoc1 == 3)
                {
                    allExt = GetAllLine(lineGoc1, false, data.khoangCachChia1, soRanh1, Math.PI / 2);
                }

                //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
                //var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia1, color1);
                //retVal.AddRange(lstLineCreated);
                retVal.AddRange(allExt);
            }

            // Line goc 2
            {
                if (data.isBacNamGoc2 == 3)
                {
                    lineGoc2.start_x = lstPDistance[8].X;
                    lineGoc2.start_y = lstPDistance[8].Y;
                    lineGoc2.end_x = lstPDistance[4].X;
                    lineGoc2.end_y = lstPDistance[4].Y;
                    //lineGoc2 = new Line(lstPDistance[8], lstPDistance[4]);
                }

                else
                {
                    lineGoc2.start_x = lstPDistance[4].X;
                    lineGoc2.start_y = lstPDistance[4].Y;
                    lineGoc2.end_x = lstPDistance[3].X;
                    lineGoc2.end_y = lstPDistance[3].Y;
                    //lineGoc2 = new Line(lstPDistance[4], lstPDistance[3]);
                }
                    

                soRanh2 = (int)(lineGoc2.Length / data.khoangCachChia2);
                soRanh2 = soRanh2 + 1;
                List<CecmProgramAreaLineDTO> allExt = new List<CecmProgramAreaLineDTO>();
                if (data.isBacNamGoc2 == 1)
                {
                    //if (data.gocTuyChon2 % 180 == 0)
                    //{
                    //    allExt = GetAllLine(lineGoc2, true, data.khoangCachChia2, soRanh2, 0);
                    //}
                    //else if (data.gocTuyChon2 % 180 == 90)
                    //{
                    //    allExt = GetAllLine(lineGoc2, false, data.khoangCachChia2, soRanh2, Math.PI / 2);
                    //}
                    //else
                    //{
                        allExt = GetLstExtRotate(lineGoc2, data.khoangCachChia2, soRanh2, data.gocTuyChon2, lstPDistance[4], lstPDistance[3], lstPDistance[7], lstPDistance[8]);
                    //}
                }
                    
                else if (data.isBacNamGoc2 == 2)
                {
                    allExt = GetAllLine(lineGoc2, true, data.khoangCachChia1, soRanh2, 0);
                }
                else if (data.isBacNamGoc2 == 3)
                {
                    allExt = GetAllLine(lineGoc2, false, data.khoangCachChia1, soRanh2, Math.PI / 2);
                }
                //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
                //var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia2, color2);
                //retVal.AddRange(lstLineCreated);
                retVal.AddRange(allExt);
            }

            // Line goc 3
            {
                if (data.isBacNamGoc3 == 3)
                {
                    lineGoc3.start_x = lstPDistance[1].X;
                    lineGoc3.start_y = lstPDistance[1].Y;
                    lineGoc3.end_x = lstPDistance[6].X;
                    lineGoc3.end_y = lstPDistance[6].Y;
                    //lineGoc3 = new Line(lstPDistance[1], lstPDistance[6]);
                }

                else
                {
                    lineGoc3.start_x = lstPDistance[6].X;
                    lineGoc3.start_y = lstPDistance[6].Y;
                    lineGoc3.end_x = lstPDistance[8].X;
                    lineGoc3.end_y = lstPDistance[8].Y;
                    //lineGoc3 = new Line(lstPDistance[6], lstPDistance[8]);
                }
                    

                soRanh3 = (int)(lineGoc3.Length / data.khoangCachChia3);
                soRanh3 = soRanh3 + 1;
                List<CecmProgramAreaLineDTO> allExt = new List<CecmProgramAreaLineDTO>();
                if (data.isBacNamGoc3 == 1)
                {
                    //if (data.gocTuyChon3 % 180 == 0)
                    //{
                    //    allExt = GetAllLine(lineGoc3, true, data.khoangCachChia3, soRanh3, 0);
                    //}
                    //else if (data.gocTuyChon3 % 180 == 90)
                    //{
                    //    allExt = GetAllLine(lineGoc3, false, data.khoangCachChia3, soRanh3, Math.PI / 2);
                    //}
                    //else
                    //{
                        allExt = GetLstExtRotate(lineGoc3, data.khoangCachChia3, soRanh3, data.gocTuyChon3, lstPDistance[6], lstPDistance[8], lstPDistance[5], lstPDistance[1]);
                    //}
                }    
                    
                else if (data.isBacNamGoc3 == 2)
                {
                    allExt = GetAllLine(lineGoc3, true, data.khoangCachChia3, soRanh3, 0);
                }
                else if (data.isBacNamGoc3 == 3)
                {
                    allExt = GetAllLine(lineGoc3, false, data.khoangCachChia3, soRanh3, Math.PI / 2);
                }
                //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
                //var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia3, color2);
                //retVal.AddRange(lstLineCreated);
                retVal.AddRange(allExt);
            }

            // Line goc 4
            {
                if (data.isBacNamGoc4 == 3)
                {
                    lineGoc4.start_x = lstPDistance[5].X;
                    lineGoc4.start_y = lstPDistance[5].Y;
                    lineGoc4.end_x = lstPDistance[8].X;
                    lineGoc4.end_y = lstPDistance[8].Y;
                    //lineGoc4 = new Line(lstPDistance[5], lstPDistance[8]);
                }

                else
                {
                    lineGoc4.start_x = lstPDistance[8].X;
                    lineGoc4.start_y = lstPDistance[8].Y;
                    lineGoc4.end_x = lstPDistance[7].X;
                    lineGoc4.end_y = lstPDistance[7].Y;
                    //lineGoc4 = new Line(lstPDistance[8], lstPDistance[7]);
                }
                    

                soRanh4 = (int)(lineGoc4.Length / data.khoangCachChia4);
                soRanh4 = soRanh4 + 1;
                List<CecmProgramAreaLineDTO> allExt = new List<CecmProgramAreaLineDTO>();
                if (data.isBacNamGoc4 == 1)
                {
                    //if (data.gocTuyChon4 % 180 == 0)
                    //{
                    //    allExt = GetAllLine(lineGoc4, true, data.khoangCachChia4, soRanh4, 0);
                    //}
                    //else if (data.gocTuyChon4 % 180 == 90)
                    //{
                    //    allExt = GetAllLine(lineGoc4, false, data.khoangCachChia4, soRanh4, Math.PI / 2);
                    //}
                    //else
                    //{
                        allExt = GetLstExtRotate(lineGoc4, data.khoangCachChia4, soRanh4, data.gocTuyChon4, lstPDistance[8], lstPDistance[7], lstPDistance[2], lstPDistance[5]);
                    //}
                }
                    
                else if (data.isBacNamGoc4 == 2)
                {
                    allExt = GetAllLine(lineGoc4, true, data.khoangCachChia4, soRanh4, 0);
                }
                else if (data.isBacNamGoc4 == 3)
                {
                    allExt = GetAllLine(lineGoc4, false, data.khoangCachChia4, soRanh4, Math.PI / 2);
                }
                //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
                //var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia4, color1);
                //retVal.AddRange(lstLineCreated);
                retVal.AddRange(allExt);
            }

            return retVal;
        }

        public List<CecmProgramAreaLineDTO> DrawLineJigAll(Boundary rectangle, ChiaMatCatOLuoiData data)
        {
            List<CecmProgramAreaLineDTO> retVal = new List<CecmProgramAreaLineDTO>();

            Point2d ptOrigin = new Point2d();
            double distanceP = 0;
            byte colorIndex = 0;

            CecmProgramAreaLineDTO lineGoc1 = new CecmProgramAreaLineDTO();
            int soRanh1 = 1, soRanh2 = 1, soRanh3 = 1, soRanh4 = 1;

            //rectangle.GetGeometry(ref ptOrigin, ref distanceP, ref colorIndex);
            var lstPDistance = GetPDistance(rectangle);

            //var color1 = Autodesk.AutoCAD.Colors.Color.FromRgb(153, 204, 0);
            //var color2 = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 0, 255);

            // Line goc 1
            if (data.isBacNamGoc1 == 3)
            {
                lineGoc1.start_x = lstPDistance[1].X;
                lineGoc1.start_y = lstPDistance[1].Y;
                lineGoc1.end_x = lstPDistance[0].X;
                lineGoc1.end_y = lstPDistance[0].Y;
                //lineGoc1 = new Line(lstPDistance[1], lstPDistance[0]);
            }

            else
            {
                lineGoc1.start_x = lstPDistance[0].X;
                lineGoc1.start_y = lstPDistance[0].Y;
                lineGoc1.end_x = lstPDistance[3].X;
                lineGoc1.end_y = lstPDistance[3].Y;
                //lineGoc1 = new Line(lstPDistance[0], lstPDistance[3]);
            }
                

            soRanh1 = (int)(lineGoc1.Length / data.khoangCachChia1);
            soRanh1 = soRanh1 + 1;

            List<CecmProgramAreaLineDTO> allExt = new List<CecmProgramAreaLineDTO>();
            if (data.isBacNamGoc1 == 1)
            {
                //if(data.gocTuyChon1 % 180 == 0)
                //{
                //    allExt = GetAllLine(lineGoc1, true, data.khoangCachChia1, soRanh1, 0);
                //}
                //else if(data.gocTuyChon1 % 180 == 90)
                //{
                //    allExt = GetAllLine(lineGoc1, false, data.khoangCachChia1, soRanh1, Math.PI / 2);
                //}
                //else
                //{
                    allExt = GetLstExtRotate(lineGoc1, data.khoangCachChia1, soRanh1, data.gocTuyChon1, lstPDistance[0], lstPDistance[3], lstPDistance[2], lstPDistance[1]);
                //}
            }
            else if(data.isBacNamGoc1 == 2)
            {
                allExt = GetAllLine(lineGoc1, true, data.khoangCachChia1, soRanh1, 0);
            }
            else if(data.isBacNamGoc1 == 3)
            {
                allExt = GetAllLine(lineGoc1, false, data.khoangCachChia1, soRanh1, Math.PI / 2);
            }

            //allExt = allExt.OrderBy(x => x.MinPoint.X).ToList();
            //var lstLineCreated = CreateSectionLine(allExt, data.khoangCachChia1, color1);
            //retVal.AddRange(lstLineCreated);
            //retVal.AddRange(allExt);
            return allExt;
        }

        private CecmProgramAreaLineDTO RotateLine(CecmProgramAreaLineDTO line, double rad, Point2d center)
        {
            CecmProgramAreaLineDTO newLine = new CecmProgramAreaLineDTO();
            newLine.start_x = center.X + (line.start_x - center.X) * Math.Cos(rad) - (line.start_y - center.Y) * Math.Sin(rad);
            newLine.start_y = center.Y + (line.start_x - center.X) * Math.Sin(rad) + (line.start_y - center.Y) * Math.Cos(rad);
            newLine.end_x = center.X + (line.end_x - center.X) * Math.Cos(rad) - (line.end_y - center.Y) * Math.Sin(rad);
            newLine.end_y = center.Y + (line.end_x - center.X) * Math.Sin(rad) + (line.end_y - center.Y) * Math.Cos(rad);
            return newLine;
        }

        private CecmProgramAreaLineDTO Intersect(Point2d p1, Point2d p2, Point2d p3, Point2d p4, CecmProgramAreaLineDTO line)
        {
            //Phương trình đường thẳng của rãnh
            //xPT * x + yPT * y + heSoTuDo = 0
            double xPT = line.start_y - line.end_y;
            double yPT = -(line.start_x - line.end_x);
            double heSoTuDo = -xPT * line.start_x - yPT * line.start_y;
            //y = ax + b
            double a = -(xPT / yPT);
            double b = -(heSoTuDo / yPT);

            //Phương trình đường thẳng các đường bao ô
            double xPT_1 = p1.Y - p2.Y;
            double yPT_1 = -(p1.X - p2.X);
            double heSoTuDo_1 = -xPT_1 * p1.X - yPT_1 * p1.Y;
            double a_1 = -(xPT_1 / yPT_1);
            double b_1 = -(heSoTuDo_1 / yPT_1);

            double xPT_2 = p2.Y - p3.Y;
            double yPT_2 = -(p2.X - p3.X);
            double heSoTuDo_2 = -xPT_2 * p2.X - yPT_2 * p2.Y;
            double a_2 = -(xPT_2 / yPT_2);
            double b_2 = -(heSoTuDo_2 / yPT_2);

            double xPT_3 = p3.Y - p4.Y;
            double yPT_3 = -(p3.X - p4.X);
            double heSoTuDo_3 = -xPT_3 * p3.X - yPT_3 * p3.Y;
            double a_3 = -(xPT_3 / yPT_3);
            double b_3 = -(heSoTuDo_3 / yPT_3);

            double xPT_4 = p4.Y - p1.Y;
            double yPT_4 = -(p4.X - p1.X);
            double heSoTuDo_4 = -xPT_4 * p4.X - yPT_4 * p4.Y;
            double a_4 = -(xPT_4 / yPT_4);
            double b_4 = -(heSoTuDo_4 / yPT_4);

            List<Point2d> points = new List<Point2d>();
            //Rãnh cắt đoạn 1-2
            if ((xPT * p1.X + yPT * p1.Y + heSoTuDo) * (xPT * p2.X + yPT * p2.Y + heSoTuDo) < 0)
            {
                if(a - a_1 != 0)
                {
                    double x = (-b + b_1) / (a - a_1);
                    double y = a * x + b;
                    points.Add(new Point2d(x, y));
                }
            }
            //Rãnh cắt đoạn 2-3
            if ((xPT * p2.X + yPT * p2.Y + heSoTuDo) * (xPT * p3.X + yPT * p3.Y + heSoTuDo) < 0)
            {
                if (a - a_2 != 0)
                {
                    double x = (-b + b_2) / (a - a_2);
                    double y = a * x + b;
                    points.Add(new Point2d(x, y));
                }
            }
            //Rãnh cắt đoạn 3-4
            if ((xPT * p3.X + yPT * p3.Y + heSoTuDo) * (xPT * p4.X + yPT * p4.Y + heSoTuDo) < 0)
            {
                if (a - a_3 != 0)
                {
                    double x = (-b + b_3) / (a - a_3);
                    double y = a * x + b;
                    points.Add(new Point2d(x, y));
                }
            }
            //Rãnh cắt đoạn 4-1
            if ((xPT * p4.X + yPT * p4.Y + heSoTuDo) * (xPT * p1.X + yPT * p1.Y + heSoTuDo) < 0)
            {
                if (a - a_4 != 0)
                {
                    double x = (-b + b_4) / (a - a_4);
                    double y = a * x + b;
                    points.Add(new Point2d(x, y));
                }
            }
            if(points.Count == 2)
            {
                CecmProgramAreaLineDTO lineIntersect = new CecmProgramAreaLineDTO();
                lineIntersect.start_x = points[0].X;
                lineIntersect.start_y = points[0].Y;
                lineIntersect.end_x = points[1].X;
                lineIntersect.end_y = points[1].Y;
                return lineIntersect;
            }
            else
            {
                return null;
            }
        }

        private List<CecmProgramAreaLineDTO> GetLstExtRotate(CecmProgramAreaLineDTO line, double khoangCachChia, int soRanh, double rotate, Point2d p1, Point2d p2, Point2d p3, Point2d p4)
        {
            Point2d centerP = new Point2d((line.start_x + line.end_x) / 2, (line.start_y + line.end_y) / 2);
            double rad = (System.Math.PI / 180) * (180 - rotate);

            //line.TransformBy(Matrix3d.Rotation(rad,
            //                               Vector3d.ZAxis,
            //                               centerP));
            line = RotateLine(line, rad, centerP);

            //line = AppUtils.ExtendLine(line, line.Length * 5);

            var allExtTempLeft = GetAllLine(line, false, khoangCachChia, soRanh * 3, rad);
            var allExtTempRight = GetAllLine(line, true, khoangCachChia, soRanh * 3, rad);
            allExtTempRight.RemoveAt(0);

            List<CecmProgramAreaLineDTO> allExtTemp = new List<CecmProgramAreaLineDTO>();
            allExtTempLeft.Reverse();
            allExtTemp.AddRange(allExtTempLeft);
            allExtTemp.AddRange(allExtTempRight);

            //Polyline pline = new Polyline(4);
            //pline.AddVertexAt(0, new Point2d(p1.X, p1.Y), 0, 0, 0);
            //pline.AddVertexAt(1, new Point2d(p2.X, p2.Y), 0, 0, 0);
            //pline.AddVertexAt(2, new Point2d(p3.X, p3.Y), 0, 0, 0);
            //pline.AddVertexAt(3, new Point2d(p4.X, p4.Y), 0, 0, 0);
            //pline.Closed = true;

            List<CecmProgramAreaLineDTO> retval = new List<CecmProgramAreaLineDTO>();
            foreach (var lineTemp in allExtTemp)
            {
                //retval.Add(lineTemp);
                CecmProgramAreaLineDTO lineIntersect = Intersect(p1, p2, p3, p4, lineTemp);
                if(lineIntersect != null)
                {
                    retval.Add(lineIntersect);
                }
                //    Point3dCollection ptColl = new Point3dCollection();
                //    lineTemp.IntersectWith(pline, Intersect.ExtendThis, ptColl, System.IntPtr.Zero, System.IntPtr.Zero);
                //    if (ptColl.Count == 2)
                //        retval.Add(new Line(ptColl[1], ptColl[0]));
            }

            return retval;
        }

        private List<Point2d> GetPDistance(Boundary oluoi)
        {
            List<Point2d> retVal = new List<Point2d>();

            // Trai tren
            Point2d pt0 = new Point2d(oluoi.maxLat, oluoi.minLong);
            // Trai duoi
            Point2d pt1 = new Point2d(oluoi.minLat, oluoi.minLong);
            // Phai duoi
            Point2d pt2 = new Point2d(oluoi.minLat, oluoi.maxLong);
            // Phai tren
            Point2d pt3 = new Point2d(oluoi.maxLat, oluoi.maxLong);

            // Giua tren
            Point2d pt4 = new Point2d((pt0.X + pt3.X) / 2, (pt0.Y + pt3.Y) / 2);
            // Giua duoi
            Point2d pt5 = new Point2d((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
            // Giua trai
            Point2d pt6 = new Point2d((pt0.X + pt1.X) / 2, (pt0.Y + pt1.Y) / 2);
            // Giua phai
            Point2d pt7 = new Point2d((pt2.X + pt3.X) / 2, (pt2.Y + pt3.Y) / 2);
            // Giua
            Point2d pt8 = new Point2d((pt4.X + pt5.X) / 2, (pt4.Y + pt5.Y) / 2);

            retVal.Add(pt0);
            retVal.Add(pt1);
            retVal.Add(pt2);
            retVal.Add(pt3);
            retVal.Add(pt4);
            retVal.Add(pt5);
            retVal.Add(pt6);
            retVal.Add(pt7);
            retVal.Add(pt8);

            return retVal;
        }

        //private List<ObjectId> CreateSectionLine(List<Line> lExt, double khoangCach, Autodesk.AutoCAD.Colors.Color color)
        //{
        //    List<ObjectId> retVal = new List<ObjectId>();

        //    var ed = AutoCADApp.DocumentManager.MdiActiveDocument.Editor;
        //    var db = ed.Document.Database;

        //    using (Transaction tr = AutoCADApp.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
        //    {
        //        BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
        //        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
        //        var oidLayer = DBUtils.CheckLayer(db, tr, AppUtils.MatCatTuTruong);
        //        int index = 1;
        //        foreach (var ext in lExt)
        //        {
        //            MgdAcDbVNTerrainSectionLine mSectionLine = new MgdAcDbVNTerrainSectionLine();
        //            mSectionLine.Create(ext.StartPoint, ext.EndPoint, index.ToString());

        //            mSectionLine.Radius = khoangCach / 2;
        //            mSectionLine.TextHeight = khoangCach / 4;

        //            btr.AppendEntity(mSectionLine);
        //            tr.AddNewlyCreatedDBObject(mSectionLine, true);

        //            mSectionLine.LayerId = oidLayer;

        //            if (color != null)
        //                mSectionLine.Color = (uint)color.ColorIndex;

        //            index++;

        //            retVal.Add(mSectionLine.Id);
        //        }

        //        tr.Commit();
        //    }

        //    return retVal;
        //}

        //private bool CheckInterLineBoundary(MgdAcDbVNTerrainBaseRegion mDuongBao, Line mLine)
        //{
        //    Point3dCollection ptColl = new Point3dCollection();
        //    mLine.IntersectWith(mDuongBao, Intersect.ExtendThis, ptColl, System.IntPtr.Zero, System.IntPtr.Zero);

        //    if (ptColl.Count != 0)
        //        return true;
        //    else
        //        return false;
        //}

        private CecmProgramAreaLineDTO GetOffsetCurves(CecmProgramAreaLineDTO line, double khoangCach, double rad)
        {
            CecmProgramAreaLineDTO newLine = new CecmProgramAreaLineDTO();
            newLine.start_x = line.start_x + khoangCach * Math.Cos(rad);
            newLine.start_y = line.start_y + khoangCach * Math.Sin(rad);
            newLine.end_x = line.end_x + khoangCach * Math.Cos(rad);
            newLine.end_y = line.end_y + khoangCach * Math.Sin(rad);
            return newLine;
        }

        private List<CecmProgramAreaLineDTO> GetAllLine(CecmProgramAreaLineDTO line, bool huongDo, double khoangCach, int soRanh, double rad)
        {
            List<CecmProgramAreaLineDTO> retVal = new List<CecmProgramAreaLineDTO>();

            if (line != null)
            {
                //using (Transaction tr = AutoCADApp.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
                //{
                retVal.Add(line);

                if (huongDo)
                    khoangCach = -khoangCach;

                for (int i = 1; i < soRanh; i++)
                {
                    //var acDbObjColl = line.GetOffsetCurves(khoangCach * i);

                    //foreach (Line acEnt in acDbObjColl)
                    //    retVal.Add(acEnt);
                    CecmProgramAreaLineDTO newLine = GetOffsetCurves(line, khoangCach * i, rad);
                    retVal.Add(newLine);
                }
                //    tr.Commit();
                //}
            }

            return retVal;
        }
    }

    public class ChiaMatCatOLuoiData
    {
        public double khoangCachChia1 { get; set; }

        public double khoangCachChia2 { get; set; }

        public double khoangCachChia3 { get; set; }

        public double khoangCachChia4 { get; set; }

        // Bac nam =1
        // dong tay =2
        // goc =3
        public int isBacNamGoc1 { get; set; }

        public double gocTuyChon1 { get; set; }

        public int isBacNamGoc2 { get; set; }
        public double gocTuyChon2 { get; set; }

        public int isBacNamGoc3 { get; set; }
        public double gocTuyChon3 { get; set; }

        public int isBacNamGoc4 { get; set; }
        public double gocTuyChon4 { get; set; }

        //1 du an
        //2 vung
        //3 chon
        public int kieuChia { get; set; }

        public long oluoi_id { get; set; }

        //public long cecm_program_area_map_id { get; set; }

        //public long cecm_program_id { get; set; }
    }
}