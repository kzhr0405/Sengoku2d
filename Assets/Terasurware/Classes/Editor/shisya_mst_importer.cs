using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class shisya_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/shisya_mst.xls";
    private static readonly string[] sheetNames = { "shisya_mst", };
    
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
                    var data = (Entity_shisya_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_shisya_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_shisya_mst>();
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
                        
                        var p = new Entity_shisya_mst.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.selectFlg = (cell == null ? false : cell.BooleanCellValue);
					cell = row.GetCell(2); p.name = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.Slot = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.Serihu1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.Serihu2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.Serihu3 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(7); p.YesRequried1 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.YesRequried2 = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(9); p.yesEffect = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(10); p.yesEffectValue = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(11); p.noEffect = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.noEffectValue = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.OKSerihu = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(14); p.NGSerihu = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(15); p.nameEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(16); p.Serihu1Eng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(17); p.Serihu2Eng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(18); p.Serihu3Eng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(19); p.OKSerihuEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(20); p.NGSerihuEng = (cell == null ? "" : cell.StringCellValue);

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
