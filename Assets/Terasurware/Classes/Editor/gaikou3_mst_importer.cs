using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class gaikou3_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/gaikou3_mst.xls";
    private static readonly string[] sheetNames = { "gaikou3_mst", };
    
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
                    var data = (Entity_gaikou3_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_gaikou3_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_gaikou3_mst>();
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
                        
                        var p = new Entity_gaikou3_mst.Param();
			
					cell = row.GetCell(0); p.daimyoId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.daimyo1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.daimyo17 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.daimyo20 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.daimyo23 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.daimyo24 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.daimyo25 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.daimyo27 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.daimyo28 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.daimyo30 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.daimyo38 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.daimyo40 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.daimyo45 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.daimyo46 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.daimyo47 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.daimyo48 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.daimyo50 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.daimyo52 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.daimyo54 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.daimyo56 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.daimyo58 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.daimyo59 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.daimyo60 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.daimyo62 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.daimyo63 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.daimyo64 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.daimyo65 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.daimyo67 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(28); p.daimyo68 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(29); p.daimyo69 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(30); p.daimyo70 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(31); p.daimyo71 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(32); p.daimyo72 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(33); p.daimyo73 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(34); p.daimyo74 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(35); p.daimyo76 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(36); p.daimyo77 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(37); p.daimyo78 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(38); p.daimyo79 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(39); p.daimyo80 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(40); p.daimyo81 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(41); p.daimyo83 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(42); p.daimyo84 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(43); p.daimyo85 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(44); p.daimyo87 = (int)(cell == null ? 0 : cell.NumericCellValue);

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
