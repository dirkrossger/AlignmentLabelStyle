using System;

#region Autodesk
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
#endregion

[assembly: CommandClass(typeof(AlignmentLabelStyle.Commands))]


namespace AlignmentLabelStyle
{
    public class Commands
    {
        [CommandMethod("xx")]
        public void CreateStyle()
        {
            CivilDocument civilDoc = CivilApplication.ActiveDocument;

            //string oAlignmentSTName = "Alignment_Level_Style";
            //Autodesk.Civil.DatabaseServices.Styles.AlignmentStyle oAlignmentStyle = null;
            //ObjectId alignmentStyleId;
            //  Check if a style by this name already exists. 
            try
            {
                for(int i = 0; i < civilDoc.Styles.AlignmentStyles.Count; i++)
                {
                    Active.Editor.WriteMessage(string.Format("{0} Label named: {1}", i, civilDoc.Styles.AlignmentStyles[i].ToString()));
                }
                //alignmentStyleId = civilDoc.Styles.AlignmentStyles.Item[oAlignmentSTName];
                //if (alignmentStyleId.IsValid)
                //{
                //    MsgBox(("Style : "
                //                    + (oAlignmentSTName.ToString + (" " + "Already exists in thsi Dwg !"))));
                //    return;
                //}

            }
            catch (System.Exception ex)
            {
                //Autodesk.AutoCAD.DatabaseServices.ObjectId.Null;
            }

            //if (alignmentStyleId.IsNull)
            //{
            //    alignmentStyleId = civilDoc.Styles.AlignmentStyles.Add(oAlignmentSTName);
            //    if ((alignmentStyleId == null))
            //    {
            //        MsgBox(("Error setting an Alignment Style: " + Err.Description));
            //    }

            //}

            //oAlignmentStyle = trans.GetObject(alignmentStyleId, OpenMode.ForWrite);
            ////  Getting and setting style attributes for StyleBase objects
            //// requires using a GetDisplayStyle*() method rather than a property.
            //oAlignmentStyle.GetDisplayStyleModel(AlignmentDisplayStyleType.Arrow).Visible = false;
            //oAlignmentStyle.GetDisplayStylePlan(AlignmentDisplayStyleType.Arrow).Visible = false;
            //oAlignmentStyle.GetDisplayStyleModel(AlignmentDisplayStyleType.Curve).Color = Autodesk.AutoCAD.Colors.Color.FromRgb(58, 191, 13);
            //oAlignmentStyle.GetDisplayStylePlan(AlignmentDisplayStyleType.Curve).Color = Autodesk.AutoCAD.Colors.Color.FromRgb(58, 191, 13);
            //oAlignmentStyle.GetDisplayStyleModel(AlignmentDisplayStyleType.Curve).Visible = true;
            //oAlignmentStyle.GetDisplayStylePlan(AlignmentDisplayStyleType.Curve).Visible = true;
            //oAlignmentStyle.GetDisplayStyleModel(AlignmentDisplayStyleType.Line).Color = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 0, 255);
            ////  blue
            //oAlignmentStyle.GetDisplayStylePlan(AlignmentDisplayStyleType.Line).Color = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 0, 255);
            ////  blue
            //oAlignmentStyle.GetDisplayStyleModel(AlignmentDisplayStyleType.Line).Visible = true;
            //oAlignmentStyle.GetDisplayStylePlan(AlignmentDisplayStyleType.Line).Visible = true;
            //oAlignmentStyle.EnableRadiusSnap = true;
            //oAlignmentStyle.RadiusSnapValue = 0.05;
            ////  set a Radius Snap Value
            //trans.Commit();
        }
    }

    public class AlignmentLabelStyle : IExtensionApplication
    {
        [CommandMethod("info")]
        public void Initialize()
        {
            Active.Editor.WriteMessage("\n-> Get AlignmentLabelStyleNames: xx");
        }

        public void Terminate()
        {
        }
    }

}
