using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

[assembly: ExtensionApplication(typeof(NamesAcadTable.AcadCommand))]

namespace NamesAcadTable
{
    public class AcadCommand : IExtensionApplication
    {
        public void Initialize()
        {
            //throw new NotImplementedException();
        }

        public void Terminate()
        {
            //throw new NotImplementedException();
        }

        [CommandMethod("FillTablesWithIndustryNames")]
        public void FillTablesWithIndustryNames()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            PromptStringOptions prOpNum = new PromptStringOptions("Choose industry type Electrical/Sanitary/Road<Electrical>: ")
            {
                AllowSpaces = true,
                DefaultValue = "Electrical",
                UseDefaultValue = true
            };

            PromptResult industryType = doc.Editor.GetString(prOpNum);

            IndustryName indType;
            switch (industryType.StringResult.ToUpper())
            {
                case "ELECTRICAL":
                    indType = new Electrical().FillTable();
                    break;
                case "SANITARY":
                    indType = new Sanitary().FillTable();
                    break;
                case "ROAD":
                     indType = new Road().FillTable();
                    break;
                default:
                    indType = new Electrical().FillTable();
                    break;
            }
            BlockAttributes.Use(blkAtt => {blkAtt.UpdateAcadBlockAttributes("A$C3E58180D", indType);});
        }
    }
}
