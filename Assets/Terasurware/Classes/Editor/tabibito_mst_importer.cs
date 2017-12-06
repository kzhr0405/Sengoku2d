using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class tabibito_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/tabibito_mst.xls";
    private static readonly string[] sheetNames = { "tabibito_mst", };
    
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (!filePath.Equals(asset))
                continue;

            using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read))
            {
                var book = new HSSFWorkbook(stream);

                foreach (string sheetName in sheetNames)
                {
                    var exportPath = "Assets/Resources/Data/" + sheetName + ".asset";
                    
                    // check scriptable object
                    var data = (Entity_tabibito_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_tabibito_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_tabibito_mst>();
                        AssetDatabase.CreateAsset((ScriptableObject)data, exportPath);
                        data.hideFlags = HideFlags.NotEditable;
                    }
                    data.param.Clear();

					// check sheet
                    var sheet = book.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        Debug.LogError("[QuestData] sheet not found:" + sheetName);
                        continue;
                    }

                	// add infomation
                    for (int i=1; i<= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        ICell cell = null;
                        
                        var p = new Entity_tabibito_mst.Param();
			
					cell = row.GetCell(0); p.Id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.Typ = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.GrpID = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.Grp = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.Name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.Rank = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.Exp = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.ItemMst = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.ItemMstId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.ItemQty = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.GrpEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.NameEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.ExpEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.GrpSChn = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(14); p.NameSChn = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(15); p.ExpSChn = (cell == null ? "" : cell.StringCellValue);

                        data.param.Add(p);
                    }
                    
                    // save scriptable object
                    ScriptableObject obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(ScriptableObject)) as ScriptableObject;
                    EditorUtility.SetDirty(obj);
                }
            }

        }
    }
}
