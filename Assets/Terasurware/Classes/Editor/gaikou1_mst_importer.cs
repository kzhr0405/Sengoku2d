using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class gaikou1_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/gaikou1_mst.xls";
    private static readonly string[] sheetNames = { "gaikou1_mst", };
    
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
                    var data = (Entity_gaikou1_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_gaikou1_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_gaikou1_mst>();
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
                        
                        var p = new Entity_gaikou1_mst.Param();
			
					cell = row.GetCell(0); p.daimyoId = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.daimyo1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.daimyo5 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(3); p.daimyo6 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(4); p.daimyo7 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(5); p.daimyo8 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.daimyo10 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.daimyo12 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.daimyo13 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.daimyo14 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.daimyo15 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.daimyo16 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.daimyo17 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.daimyo18 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.daimyo19 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.daimyo20 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.daimyo21 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.daimyo22 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.daimyo23 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.daimyo24 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.daimyo25 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.daimyo26 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.daimyo27 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.daimyo28 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.daimyo29 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.daimyo30 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.daimyo31 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.daimyo32 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(28); p.daimyo33 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(29); p.daimyo34 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(30); p.daimyo36 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(31); p.daimyo37 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(32); p.daimyo38 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(33); p.daimyo39 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(34); p.daimyo40 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(35); p.daimyo41 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(36); p.daimyo42 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(37); p.daimyo43 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(38); p.daimyo44 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(39); p.daimyo45 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(40); p.daimyo46 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(41); p.daimyo47 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(42); p.daimyo48 = (int)(cell == null ? 0 : cell.NumericCellValue);

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
