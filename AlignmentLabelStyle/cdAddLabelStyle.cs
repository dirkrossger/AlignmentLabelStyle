using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Autodesk
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;
using Autodesk.Civil;
#endregion



namespace AcSpeedy

{
    class cdAddLabelStyle
    {
        private void removeAllComponents(ObjectId styleId)
        {
            IEnumerable<string> componentNames = getTextComponentNames(styleId);
            removeComponents(styleId, componentNames);
        }

        private IEnumerable<string> getTextComponentNames(ObjectId styleId)
        {
            List<string> names = new List<string>();
            using (Transaction tr = Active.Database.TransactionManager.StartTransaction())
            {
                LabelStyle style = styleId.GetObject(OpenMode.ForRead)
                  as LabelStyle;
                foreach (ObjectId id in style.GetComponents(
                  LabelStyleComponentType.Text))
                {
                    LabelStyleComponent component =
                      id.GetObject(OpenMode.ForRead) as LabelStyleComponent;
                    names.Add(component.Name);
                }
            }
            return names;
        }

        private void removeComponents(ObjectId styleId, IEnumerable<string> componentNames)
        {
            using (Transaction tr = Active.Database.TransactionManager.StartTransaction())
            {
                LabelStyle style = styleId.GetObject(OpenMode.ForWrite)
                  as LabelStyle;
                foreach (string name in componentNames)
                {
                    style.RemoveComponent(name);
                }

                tr.Commit();
            }
        }

        private void customizeStyle(ObjectId styleId)
        {
            using (Transaction tr = Active.Database.TransactionManager.StartTransaction())
            {
                addStyleComponents(styleId);
                tr.Commit();
            }
        }

        private void addStyleComponents(ObjectId styleId)
        {
            LabelStyle style = styleId.GetObject(OpenMode.ForWrite)
              as LabelStyle;
            addLeaderComponent(style);
            addPointNumberComponent(style);
            addLocationComponent(style);
        }

        private void addPointNumberComponent(LabelStyle style)
        {
            ObjectId id = style.AddComponent("PN", LabelStyleComponentType.Text);
            LabelStyleTextComponent component = id.GetObject(OpenMode.ForWrite)
              as LabelStyleTextComponent;
            component.Text.Attachment.Value = LabelTextAttachmentType.MiddleLeft;
            //component.Text.Contents.Value = _pointNumber;
            component.General.AnchorComponent.Value = "Leader";
            component.General.AnchorLocation.Value = AnchorPointType.End;
        }

        private void addLocationComponent(LabelStyle style)
        {
            ObjectId id = style.AddComponent("Location",
              LabelStyleComponentType.Text);
            LabelStyleTextComponent component = id.GetObject(OpenMode.ForWrite)
              as LabelStyleTextComponent;
            component.Text.Attachment.Value = LabelTextAttachmentType.TopLeft;
            //string value = String.Format("({0}, {1}, {2})", _northing, _easting, _elevation);
            //component.Text.Contents.Value = value;
            component.General.AnchorComponent.Value = "PN";
            component.General.AnchorLocation.Value = AnchorPointType.BottomLeft;
        }

        private void addLeaderComponent(LabelStyle style)
        {
            ObjectId id = style.AddComponent("Leader",
            LabelStyleComponentType.Line);
            LabelStyleLineComponent component = id.GetObject(OpenMode.ForWrite)
              as LabelStyleLineComponent;
            component.General.StartAnchorPoint.Value = AnchorPointType.MiddleCenter;
        }


        public void createPointLabelStyle(string name)
        {
            ObjectId styleId = _pointLabelStyles.Add(name);
            removeAllComponents(styleId);
            customizeStyle(styleId);
        }

        private LabelStyleCollection _pointLabelStyles
        {
            get
            {
                CivilDocument civilDoc = CivilApplication.ActiveDocument;
                return civilDoc.Styles.LabelStyles.PointLabelStyles.LabelStyles;
            }
        }
    }
}
