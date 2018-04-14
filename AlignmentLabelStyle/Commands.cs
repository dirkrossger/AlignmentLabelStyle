using System;

#region Autodesk
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
#endregion

[assembly: CommandClass(typeof(AcSpeedy.Commands))]


namespace AcSpeedy
{
    public class Commands
    {
        [CommandMethod("cdGetName")]
        public void GetAlignmentLabelStyle()
        {
            CivilDocument civilDoc = CivilApplication.ActiveDocument;

            using (Transaction trans = Active.Database.TransactionManager.StartTransaction())
            {
                // Get the desired Lable Styles collection
                LabelStyleCollection lblStyleColl = civilDoc.Styles.LabelStyles.AlignmentLabelStyles.GeometryPointLabelStyles;

                try
                {
                    Active.Editor.WriteMessage("\n{0} Labelstyles found.", lblStyleColl.Count);
                    foreach (ObjectId item in lblStyleColl)
                    {
                        LabelStyle style = (LabelStyle)trans.GetObject(item, OpenMode.ForRead);
                        Active.Editor.WriteMessage("\n Expression Name : " + style.Name);
                    }
                    trans.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Active.Editor.WriteMessage("\n Exception message :" + ex.Message);
                }
            }

        }

        [CommandMethod("cdChangeName")]
        public void ChangeAlignmentLabelStyle()
        {
            PromptEntityOptions options = new PromptEntityOptions("\nSelect Alignment: ");
            options.SetRejectMessage("\nThe selected object is not a Alignment!");
            options.AddAllowedClass(typeof(Alignment), false);
            PromptEntityResult pres = Active.Editor.GetEntity(options);
            if (pres.Status != PromptStatus.OK)
                return;

            using (Transaction tr = HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction())
            {
                try
                {
                    Alignment align = (Alignment)tr.GetObject(pres.ObjectId, OpenMode.ForRead);
                    //ObjectIdCollection ids = align.GetAlignmentLabelIds();
                    ObjectIdCollection ids = align.GetAlignmentLabelGroupIds();
                    foreach (ObjectId id in ids)
                    {
                        AlignmentLabelGroup style = (AlignmentLabelGroup)tr.GetObject(id, OpenMode.ForRead);
                        Active.Editor.WriteMessage("\n " + style.ToString());
                        //result:
                        //Autodesk.Civil.DatabaseServices.AlignmentDesignSpeedLabelGroup
                        //Autodesk.Civil.DatabaseServices.AlignmentGeometryPointLabelGroup
                        //Autodesk.Civil.DatabaseServices.AlignmentMinorStationLabelGroup
                        //Autodesk.Civil.DatabaseServices.AlignmentStationEquationLabelGroup
                        //Autodesk.Civil.DatabaseServices.AlignmentStationLabelGroup

                        //LabelType lblType = (LabelType)tr.GetObject(style.ObjectId, OpenMode.ForRead);     //Autodesk.Civil.DatabaseServices.LabelType
                        //LabelStylesAlignmentRoot root = (LabelStylesRoot)tr.GetObject(id, OpenMode.ForRead); 



                    }

                    //Active.Editor.WriteMessage("\nAlignment Curve Label Style Name (before Change) : " + alignLabelStyle.Name);

                    //// The following Style Name is specific to Tutorial DWG file - "Labels-6a.dwg"
                    //alignLabelStyle.Name = "Design Data";
                    //Active.Editor.WriteMessage("\nAlignment Curve Label Style Name (after Change) : " + alignLabelStyle.Name);

                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Active.Editor.WriteMessage("\n Exception message :" + ex.Message);
                }
            }
        }


        [CommandMethod("addStationOffsetLabel")]
        public static void CmdAddStationOffsetLabel()
        {
            CivilDocument civilDoc = CivilApplication.ActiveDocument;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId alignId in civilDoc.GetSitelessAlignmentIds())
                {
                    Alignment align = trans.GetObject(alignId, OpenMode.ForRead) as Alignment;

                    for (double s = align.StartingStation; s < align.EndingStation; s += align.StationIndexIncrement)
                    {
                        double easting = 0;
                        double northing = 0;
                        align.PointLocation(s, 0, ref easting, ref northing);

                        Point2d p = new Point2d(easting, northing);
                        StationOffsetLabel.Create(
                          alignId,
                          civilDoc.Styles.LabelStyles.AlignmentLabelStyles.StationOffsetLabelStyles[0],
                          civilDoc.Styles.MarkerStyles[0],
                          p);
                    }
                }
                trans.Commit();
            }
        }


        [CommandMethod("CreateDemoPointLabelStyle")]
        public void CDS_CreateDemoPointLabelStyle()
        {
            AcSpeedy.cdAddLabelStyle lbl = new cdAddLabelStyle();
            lbl.createPointLabelStyle("Demo");
        }
    }

    public class AlignmentLabelStyle : IExtensionApplication
    {
        [CommandMethod("info")]
        public void Initialize()
        {
            Active.Editor.WriteMessage("\n-> Get AlignmentLabelStyleNames: cdGetName");
            Active.Editor.WriteMessage("\n-> Change AlignmentLabelStyleNames: cdChangeName");
            Active.Editor.WriteMessage("\n-> addStationOffsetLabel");
            Active.Editor.WriteMessage("\n-> CreateDemoPointLabelStyle");
        }

        public void Terminate()
        {
        }
    }

}
