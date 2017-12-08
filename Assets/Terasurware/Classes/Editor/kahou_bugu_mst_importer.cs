using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class kahou_bugu_mst_importer : AssetPostprocessor
{
    private static readonly string filePath = "Assets/Resources/Data/kahou_bugu_mst.xls";
    private static readonly string[] sheetNames = { "kahou_bugu_mst", };
    
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
                    var data = (Entity_kahou_bugu_mst)AssetDatabase.LoadAssetAtPath(exportPath, typeof(Entity_kahou_bugu_mst));
                    if (data == null)
                    {
                        data = ScriptableObject.CreateInstance<Entity_kahou_bugu_mst>();
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
                        
                        var p = new Entity_kahou_bugu_mst.Param();
			
					cell = row.GetCell(0); p.id = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.kahouType = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.kahouName = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.kahouRank = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.kahouExp = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.kahouTarget = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(6); p.kahouEffect = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.unit = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.kahouBuy = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.kahouSell = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(10); p.kahouRatio = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(11); p.kahouNameEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(12); p.kahouExpEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(13); p.kahouTargetEng = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(14); p.kahouNameSChn = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(15); p.kahouExpSChn = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(16); p.kahouTargetSChn = (cell == null ? "" : cell.StringCellValue);

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
