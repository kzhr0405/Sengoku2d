using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class senpou_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/senpou_mst.xls";
    private static readonly string[] sheetNames = { "senpou_mst", };
    
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
                    var data = (Entity_senpou_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_senpou_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_senpou_mst>();
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
                        
                        var p = new Entity_senpou_mst.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.typ = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.effection = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.each = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.ratio = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.term = (float)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.lv1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.lv2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.lv3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.lv4 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.lv5 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.lv6 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.lv7 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.lv8 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.lv9 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.lv10 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.lv11 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.lv12 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.lv13 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.lv14 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.lv15 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.lv16 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.lv17 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.lv18 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.lv19 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.lv20 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.shipFlg = (cell == null ? false : cell.BooleanCellValue);
					cell = row.GetCell(28); p.onlySeaFlg = (cell == null ? false : cell.BooleanCellValue);
					cell = row.GetCell(29); p.nameEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(30); p.effectionEng = (cell == null ? "" : cell.StringCellValue);

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
