using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace NamesAcadTable
{
    public class BlockAttributes
    {
        protected BlockAttributes()
        {
        }

        public void Cleanup()
        {
            // Add Cleanup or disposing
        }

        public static void Use(Action<BlockAttributes> codeblock)
        {
            BlockAttributes blkAtt = new BlockAttributes();
            try
            {
                codeblock(blkAtt);
            }
            finally
            {
                blkAtt.Cleanup();
            }
        }
        public void UpdateAcadBlockAttributes(string blockName, IndustryName prtNo)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            ObjectId msObjectId, psObjectId;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                msObjectId = bt[BlockTableRecord.ModelSpace];
                psObjectId = bt[BlockTableRecord.PaperSpace];

                tr.Commit();
            }

            //Convert PartNumber object to dictionary
            Dictionary<string, string> attributeTagValue = new Dictionary<string, string>();
            attributeTagValue.Add("IdDesigner", prtNo.DesignedBy);
            attributeTagValue.Add("IdDeveloper", prtNo.DevelopedBy);
            attributeTagValue.Add("IdVerifier", prtNo.VerifiedBy);
            attributeTagValue.Add("IdIndustry", prtNo.GetIndustryName);

            UpdateAttributesInBlock(msObjectId, blockName, attributeTagValue);
            UpdateAttributesInBlock(psObjectId, blockName, attributeTagValue);
        }

        private void UpdateAttributesInBlock(ObjectId btrId, string blockName, Dictionary<string, string> attributeTagValue)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(btrId, OpenMode.ForRead);

                foreach (ObjectId entId in btr)
                {
                    Entity ent = trans.GetObject(entId, OpenMode.ForRead) as Entity;

                    if (ent != null)
                    {
                        BlockReference br = ent as BlockReference;
                        if (br != null)
                        {
                            BlockTableRecord btrec = (BlockTableRecord)trans.GetObject(br.BlockTableRecord, OpenMode.ForRead);
                            if (string.Compare(btrec.Name, blockName, true) == 0)
                            {
                                foreach (ObjectId arObjectIdId in br.AttributeCollection)
                                {
                                    DBObject obj = trans.GetObject(arObjectIdId, OpenMode.ForRead);
                                    AttributeReference ar = obj as AttributeReference;
                                    if (ar != null)
                                    {
                                        foreach (KeyValuePair<string, string> kvp in attributeTagValue)
                                        {
                                            if (string.Compare(ar.Tag, kvp.Key, true) == 0)
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = kvp.Value;
                                                ar.DowngradeOpen();
                                            }
                                        }
                                    }
                                }
                            }

                            // Recurse for nested blocks
                            UpdateAttributesInBlock(br.BlockTableRecord, blockName, attributeTagValue);
                        }
                    }
                }

                trans.Commit();
            }
        }
    }
}
