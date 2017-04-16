using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class naisei_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/naisei_mst.xls";
    private static readonly string[] sheetNames = { "naisei_mst", };
    
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
                    var data = (Entity_naisei_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_naisei_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_naisei_mst>();
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
                        
                        var p = new Entity_naisei_mst.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.common = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(2); p.code = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.exp = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.target = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.effect1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.effect2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(8); p.effect3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.effect4 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.effect5 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.effect6 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(12); p.effect7 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(13); p.effect8 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(14); p.effect9 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(15); p.effect10 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(16); p.effect11 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(17); p.effect12 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(18); p.effect13 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(19); p.effect14 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(20); p.effect15 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(21); p.effect16 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(22); p.effect17 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(23); p.effect18 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(24); p.effect19 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(25); p.effect20 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(26); p.money1 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(27); p.money2 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(28); p.money3 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(29); p.money4 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(30); p.money5 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(31); p.money6 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(32); p.money7 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(33); p.money8 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(34); p.money9 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(35); p.money10 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(36); p.money11 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(37); p.money12 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(38); p.money13 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(39); p.money14 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(40); p.money15 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(41); p.money16 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(42); p.money17 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(43); p.money18 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(44); p.money19 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(45); p.money20 = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(46); p.hyourou = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(47); p.nameEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(48); p.expEng = (cell == null ? "" : cell.StringCellValue);

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
